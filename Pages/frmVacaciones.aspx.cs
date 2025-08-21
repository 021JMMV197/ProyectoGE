using ProyectoGE.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarEmpleadosAsync();
                await CargarAsync();
            }
        }

        protected async void btnCargar_Click(object sender, EventArgs e) => await CargarAsync();

        protected async void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlEmpleado.Items.Count == 0) { lblMsg.Text = "No hay empleados."; return; }
                var d = new VacacionCreateDto
                {
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    FechaInicio = ParseFecha(txtInicio.Text) ?? throw new Exception("Fecha inicio inválida."),
                    FechaFin = ParseFecha(txtFin.Text) ?? throw new Exception("Fecha fin inválida."),
                    CantidadDias = ParseInt(txtDias.Text),
                    Estado = ddlEstado.SelectedValue,
                    Adicionado_Por = "webforms"
                };
                if (d.FechaFin < d.FechaInicio) { lblMsg.Text = "Fin < Inicio."; return; }

                int id = await _api.CreateAsync(d);
                lblMsg.Text = "Creado Id = " + id;
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
                var d = new VacacionUpdateDto
                {
                    IdVacacion = int.Parse(hfIdVacacion.Value),
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    FechaInicio = ParseFecha(txtInicio.Text) ?? throw new Exception("Fecha inicio inválida."),
                    FechaFin = ParseFecha(txtFin.Text) ?? throw new Exception("Fecha fin inválida."),
                    CantidadDias = ParseInt(txtDias.Text),
                    Estado = ddlEstado.SelectedValue,
                    Modificado_Por = "webforms"
                };
                if (d.FechaFin < d.FechaInicio) { lblMsg.Text = "Fin < Inicio."; return; }

                bool ok = await _api.UpdateAsync(d);
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

        protected void gvVac_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvVac.SelectedRow; if (row == null) return;
            hfIdVacacion.Value = gvVac.SelectedDataKey.Value.ToString();

            // Celdas: 0=Select, 1=ID, 2=IdEmpleado, 3=Inicio, 4=Fin, 5=Días, 6=Estado
            txtInicio.Text = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
            txtFin.Text = row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text;
            txtDias.Text = row.Cells[5].Text == "&nbsp;" ? "" : row.Cells[5].Text;
            var estado = row.Cells[6].Text == "&nbsp;" ? "Pendiente" : row.Cells[6].Text;
            var estItem = ddlEstado.Items.FindByValue(estado);
            if (estItem != null) ddlEstado.SelectedValue = estado;

            var empIdText = row.Cells[2].Text == "&nbsp;" ? "" : row.Cells[2].Text;
            if (int.TryParse(empIdText, out var empId))
            {
                var item = ddlEmpleado.Items.FindByValue(empId.ToString());
                if (item != null) ddlEmpleado.SelectedValue = empId.ToString();
            }
        }

        private async Task CargarAsync()
        {
            try
            {
                var lista = await _api.ListAsync();   
                gvVac.DataSource = lista;
                gvVac.DataBind();
                lblTotal.Text = "Total: " + (lista == null ? 0 : lista.Length);
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
                    NombreCompleto = (x.Nombre ?? "") + " " + (x.Apellido ?? "")
                }).ToList();
                ddlEmpleado.DataSource = lista;
                ddlEmpleado.DataBind();
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar empleados: " + ex.Message; }
        }

        private void Limpiar()
        {
            hfIdVacacion.Value = "";
            if (ddlEmpleado.Items.Count > 0) ddlEmpleado.SelectedIndex = 0;
            ddlEstado.SelectedValue = "Pendiente";
            txtInicio.Text = txtFin.Text = txtDias.Text = "";
        }

        private static DateTime? ParseFecha(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var d)) return d;
            return DateTime.TryParse(s, out d) ? d : (DateTime?)null;
        }

        private static int ParseInt(string s) => int.TryParse(s, out var v) ? v : 0;
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }
        protected string GetEmpleadoNombre(object idObj)
        {
            if (idObj == null) return string.Empty;
            if (!int.TryParse(idObj.ToString(), out var id)) return string.Empty;

            // ddlEmpleado ya está cargado con IdEmpleado -> NombreCompleto
            var item = ddlEmpleado.Items.FindByValue(id.ToString());
            return item?.Text ?? string.Empty;
        }


    }
}
