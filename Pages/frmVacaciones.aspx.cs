using ProyectoGE.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ProyectoGE.Infrastructure;

namespace ProyectoGE.Pages
{
    public partial class frmVacaciones : System.Web.UI.Page
    {
        private readonly VacacionesApiClient _api = new VacacionesApiClient();
        private readonly EmpleadoApiClient _empApi = new EmpleadoApiClient();

        private class EmpleadoItem
        {
            public int IdEmpleado { get; set; }
            public string NombreCompleto { get; set; }
        }

        private class VacRow
        {
            public int IdVacacion { get; set; }
            public int IdEmpleado { get; set; }
            public string EmpleadoNombre { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public int CantidadDias { get; set; }
            public string Estado { get; set; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (AuthGuard.Require(this)) return;
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarEmpleadosAsync();
                txtInicio.Text = DateTime.Today.ToString("yyyy-MM-dd");
                ddlEstado.SelectedValue = "Pendiente";
                await CargarAsync();
            }
        }

        protected async void btnCargar_Click(object sender, EventArgs e) => await CargarAsync();

        protected async void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fiVal = ParseFecha(txtInicio.Text) ?? DateTime.Today;
                DateTime ffVal = ParseFecha(txtFin.Text) ?? fiVal;

                if (ffVal < fiVal) { lblMsg.Text = "Fin < Inicio."; return; }

                int diasVal = 0;
                int.TryParse(txtDias.Text, out diasVal);

                var dto = new VacacionCreateDto
                {
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    FechaInicio = fiVal,          
                    FechaFin = ffVal,             
                    CantidadDias = diasVal,
                    Estado = ddlEstado.SelectedValue,
                    Adicionado_Por = "webforms"
                };

                int nuevoId = await _api.CreateAsync(dto);
                lblMsg.Text = "Creado Id = " + nuevoId;
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al crear: " + ex.Message; }
        }

        protected async void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hfIdVacacion.Value)) { lblMsg.Text = "Selecciona un registro."; return; }

                DateTime fiVal = ParseFecha(txtInicio.Text) ?? DateTime.Today;
                DateTime ffVal = ParseFecha(txtFin.Text) ?? fiVal;
                if (ffVal < fiVal) { lblMsg.Text = "Fin < Inicio."; return; }

                int diasVal = 0;
                int.TryParse(txtDias.Text, out diasVal);

                var dto = new VacacionUpdateDto
                {
                    IdVacacion = int.Parse(hfIdVacacion.Value),
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    FechaInicio = fiVal,          
                    FechaFin = ffVal,             
                    CantidadDias = diasVal,
                    Estado = ddlEstado.SelectedValue,
                    Modificado_Por = "webforms"
                };

                bool ok = await _api.UpdateAsync(dto);
                lblMsg.Text = ok ? "Actualizado." : "No encontrado.";
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al actualizar: " + ex.Message; }
        }

        protected async void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hfIdVacacion.Value)) { lblMsg.Text = "Selecciona un registro."; return; }
                bool ok = await _api.DeleteAsync(int.Parse(hfIdVacacion.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al eliminar: " + ex.Message; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); lblMsg.Text = ""; }

        protected void btnAtras_Click(object sender, EventArgs e) => Response.Redirect("~/Pages/Menu.aspx");

        protected void gvVac_SelectedIndexChanged(object sender, EventArgs e)
        {
            var keys = gvVac.SelectedDataKey?.Values;
            if (keys == null) return;

            hfIdVacacion.Value = keys["IdVacacion"]?.ToString() ?? "";

            var idEmp = keys["IdEmpleado"]?.ToString() ?? "";
            if (!string.IsNullOrEmpty(idEmp) && ddlEmpleado.Items.FindByValue(idEmp) != null)
                ddlEmpleado.SelectedValue = idEmp;

            var fi = keys["FechaInicio"];
            var ff = keys["FechaFin"];
            txtInicio.Text = (fi == null || fi is DBNull) ? "" : Convert.ToDateTime(fi).ToString("yyyy-MM-dd");
            txtFin.Text = (ff == null || ff is DBNull) ? "" : Convert.ToDateTime(ff).ToString("yyyy-MM-dd");

            var dias = keys["CantidadDias"];
            txtDias.Text = (dias == null || dias is DBNull) ? "" : dias.ToString();

            var est = keys["Estado"]?.ToString() ?? "";
            if (!string.IsNullOrWhiteSpace(est) && ddlEstado.Items.FindByValue(est) != null)
                ddlEstado.SelectedValue = est;
        }

        private async Task CargarAsync()
        {
            try
            {
                var empPaged = await _empApi.ListAsync(1, 1000);
                var empDict = empPaged.Items.ToDictionary(
                    e => e.IdEmpleado,
                    e => $"{e.Nombre ?? ""} {e.Apellido ?? ""}".Trim()
                );

                var vacs = await _api.ListAsync(); 

                var rows = vacs.Select(v => new VacRow
                {
                    IdVacacion = v.IdVacacion,
                    IdEmpleado = v.IdEmpleado,
                    EmpleadoNombre = empDict.ContainsKey(v.IdEmpleado) ? empDict[v.IdEmpleado] : "",
                    FechaInicio = v.FechaInicio,
                    FechaFin = v.FechaFin,
                    CantidadDias = v.CantidadDias,
                    Estado = v.Estado
                }).ToList();

                gvVac.DataSource = rows;
                gvVac.DataBind();
                lblTotal.Text = "Total: " + rows.Count;
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar: " + ex.Message; }
        }

        private async Task CargarEmpleadosAsync()
        {
            try
            {
                var r = await _empApi.ListAsync(1, 500);
                var lista = r.Items.Select(x => new EmpleadoItem
                {
                    IdEmpleado = x.IdEmpleado,
                    NombreCompleto = ((x.Nombre ?? "") + " " + (x.Apellido ?? "")).Trim()
                }).OrderBy(x => x.NombreCompleto).ToList();

                ddlEmpleado.DataSource = lista;
                ddlEmpleado.DataBind();
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar empleados: " + ex.Message; }
        }

        private void Limpiar()
        {
            hfIdVacacion.Value = "";
            if (ddlEmpleado.Items.Count > 0) ddlEmpleado.SelectedIndex = 0;
            txtInicio.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFin.Text = "";
            txtDias.Text = "";
            ddlEstado.SelectedValue = "Pendiente";
        }

        // Helpers
        private static DateTime? ParseFecha(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)) return d;
            if (DateTime.TryParse(s, out d)) return d;
            return null;
        }

        private static int ParseInt(string s) => int.TryParse(s, out var v) ? v : 0;
    }
}
