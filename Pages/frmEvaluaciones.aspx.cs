using ProyectoGE.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoGE.Pages
{
    public partial class frmEvaluaciones : System.Web.UI.Page
    {
        private readonly EvaluacionApiClient _api = new EvaluacionApiClient();
        private readonly EmpleadoApiClient _empApi = new EmpleadoApiClient();

        private class GridRow
        {
            public int IdEvaluacion { get; set; }
            public int IdEmpleado { get; set; }
            public string EmpleadoNombre { get; set; }
            public DateTime? PeriodoInicio { get; set; }
            public DateTime? PeriodoFin { get; set; }
            public decimal? Calificacion { get; set; }
            public int? IdEvaluador { get; set; }
            public string Comentarios { get; set; }
        }

        private class EmpleadoItem
        {
            public int IdEmpleado { get; set; }
            public string NombreCompleto { get; set; }
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarEmpleadosAsync();
                await CargarAsync();
            }
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

                var lista = await _api.ListAsync(); 

                var rows = lista.Select(x => new GridRow
                {
                    IdEvaluacion = x.IdEvaluacion,
                    IdEmpleado = x.IdEmpleado,
                    EmpleadoNombre = empDict.ContainsKey(x.IdEmpleado) ? empDict[x.IdEmpleado] : "",
                    PeriodoInicio = x.PeriodoInicio,
                    PeriodoFin = x.PeriodoFin,
                    Calificacion = x.Calificacion,
                    IdEvaluador = x.IdEvaluador,
                    Comentarios = x.Comentarios
                }).ToList();

                gvEval.DataSource = rows;
                gvEval.DataBind();
                lblTotal.Text = "Total: " + rows.Count;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al cargar: " + ex.Message;
            }
        }

        private async Task CargarEmpleadosAsync()
        {
            try
            {
                var r = await _empApi.ListAsync(1, 1000);
                var lista = r.Items
                    .Select(x => new EmpleadoItem
                    {
                        IdEmpleado = x.IdEmpleado,
                        NombreCompleto = ((x.Nombre ?? "") + " " + (x.Apellido ?? "")).Trim()
                    })
                    .OrderBy(x => x.NombreCompleto)
                    .ToList();

                ddlEmpleado.DataSource = lista;
                ddlEmpleado.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al cargar empleados: " + ex.Message;
            }
        }

        protected async void btnCargar_Click(object sender, EventArgs e) => await CargarAsync();

        protected async void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                var dto = new EvaluacionCreateDto
                {
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),

                    PeriodoInicio = ParseFecha(txtInicio.Text) ?? DateTime.Today,
                    PeriodoFin = ParseFecha(txtFin.Text) ?? DateTime.Today,
                    Calificacion = ParseDecimal(txtCalif.Text) ?? 0m,

                    IdEvaluador = ParseIntNullable(txtEvaluador.Text),

                    Comentarios = string.IsNullOrWhiteSpace(txtComentarios.Text) ? null : txtComentarios.Text.Trim(),
                    Adicionado_Por = "webforms"
                };

                var id = await _api.CreateAsync(dto);
                lblMsg.Text = "Creado Id = " + id;
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al crear: " + ex.Message;
            }
        }

        protected async void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hfIdEval.Value))
                {
                    lblMsg.Text = "Selecciona un registro.";
                    return;
                }

                var dto = new EvaluacionUpdateDto
                {
                    IdEvaluacion = int.Parse(hfIdEval.Value),
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),

                    PeriodoInicio = ParseFecha(txtInicio.Text) ?? DateTime.Today,
                    PeriodoFin = ParseFecha(txtFin.Text) ?? DateTime.Today,
                    Calificacion = ParseDecimal(txtCalif.Text) ?? 0m,

                    IdEvaluador = ParseIntNullable(txtEvaluador.Text),
                    Comentarios = string.IsNullOrWhiteSpace(txtComentarios.Text) ? null : txtComentarios.Text.Trim(),
                    Modificado_Por = "webforms"
                };

                var ok = await _api.UpdateAsync(dto);
                lblMsg.Text = ok ? "Actualizado." : "No encontrado.";
                await CargarAsync();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al actualizar: " + ex.Message;
            }
        }

        protected async void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hfIdEval.Value))
                {
                    lblMsg.Text = "Selecciona un registro.";
                    return;
                }

                var ok = await _api.DeleteAsync(int.Parse(hfIdEval.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al eliminar: " + ex.Message;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
            lblMsg.Text = "";
        }

        private void Limpiar()
        {
            hfIdEval.Value = "";
            if (ddlEmpleado.Items.Count > 0) ddlEmpleado.SelectedIndex = 0;
            txtInicio.Text = "";
            txtFin.Text = "";
            txtCalif.Text = "";
            txtEvaluador.Text = "";
            txtComentarios.Text = "";
        }

        protected void gvEval_SelectedIndexChanged(object sender, EventArgs e)
        {
            var keys = gvEval.SelectedDataKey?.Values;
            if (keys == null) return;

            hfIdEval.Value = keys["IdEvaluacion"]?.ToString() ?? "";

            var idEmp = keys["IdEmpleado"]?.ToString() ?? "";
            if (!string.IsNullOrEmpty(idEmp) && ddlEmpleado.Items.FindByValue(idEmp) != null)
                ddlEmpleado.SelectedValue = idEmp;

            var ini = keys["PeriodoInicio"];
            var fin = keys["PeriodoFin"];
            txtInicio.Text = (ini == null || ini is DBNull) ? "" : Convert.ToDateTime(ini).ToString("yyyy-MM-dd");
            txtFin.Text = (fin == null || fin is DBNull) ? "" : Convert.ToDateTime(fin).ToString("yyyy-MM-dd");

            var cal = keys["Calificacion"];
            txtCalif.Text = (cal == null || cal is DBNull) ? "" : Convert.ToDecimal(cal).ToString(CultureInfo.InvariantCulture);

            var idEval = keys["IdEvaluador"];
            txtEvaluador.Text = (idEval == null || idEval is DBNull) ? "" : idEval.ToString();

            var com = keys["Comentarios"];
            txtComentarios.Text = (com == null || com is DBNull) ? "" : com.ToString();
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

        private static DateTime? ParseFecha(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out var d)) return d;
            if (DateTime.TryParse(s, out d)) return d;
            return null;
        }

        private static decimal? ParseDecimal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v)) return v;
            if (decimal.TryParse(s, out v)) return v;
            return null;
        }

        private static int? ParseIntNullable(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return int.TryParse(s, out var v) ? v : (int?)null;
        }
    }
}
