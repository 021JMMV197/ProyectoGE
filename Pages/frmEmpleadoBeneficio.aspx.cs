using ProyectoGE.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoGE.Pages
{
    public partial class frmEmpleadoBeneficio : System.Web.UI.Page
    {
        private readonly EmpleadoBeneficioApiClient _api = new EmpleadoBeneficioApiClient();
        private readonly EmpleadoApiClient _empApi = new EmpleadoApiClient();
        private readonly BeneficioApiClient _benApi = new BeneficioApiClient();

        private class EmpleadoItem { public int IdEmpleado { get; set; } public string NombreCompleto { get; set; } }
        private class BeneficioItem { public int IdBeneficio { get; set; } public string Nombre { get; set; } }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarEmpleadosAsync();
                await CargarBeneficiosAsync();
                ddlEstado.SelectedValue = "Activo";
                await CargarAsync();
            }
        }

        protected async void btnCargar_Click(object sender, EventArgs e) => await CargarAsync();

        protected async void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                var dto = new EmpleadoBeneficioCreateDto
                {
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    IdBeneficio = int.Parse(ddlBeneficio.SelectedValue),
                    FechaInicio = ParseFecha(txtInicio.Text) ?? DateTime.Today,
                    FechaFin = ParseFecha(txtFin.Text),
                    Estado = ddlEstado.SelectedValue,
                    Observacion = string.IsNullOrWhiteSpace(txtObs.Text) ? null : txtObs.Text.Trim(),
                    Adicionado_Por = "webforms"
                };
                if (dto.FechaFin.HasValue && dto.FechaFin.Value < dto.FechaInicio) { lblMsg.Text = "Fin < Inicio."; return; }

                int id = await _api.CreateAsync(dto);
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
                if (string.IsNullOrEmpty(hfIdEB.Value)) { lblMsg.Text = "Selecciona un registro."; return; }

                var dto = new EmpleadoBeneficioUpdateDto
                {
                    IdEmpleadoBeneficio = int.Parse(hfIdEB.Value),
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    IdBeneficio = int.Parse(ddlBeneficio.SelectedValue),
                    FechaInicio = ParseFecha(txtInicio.Text) ?? DateTime.Today,
                    FechaFin = ParseFecha(txtFin.Text),
                    Estado = ddlEstado.SelectedValue,
                    Observacion = string.IsNullOrWhiteSpace(txtObs.Text) ? null : txtObs.Text.Trim(),
                    Modificado_Por = "webforms"
                };
                if (dto.FechaFin.HasValue && dto.FechaFin.Value < dto.FechaInicio) { lblMsg.Text = "Fin < Inicio."; return; }

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
                if (string.IsNullOrEmpty(hfIdEB.Value)) { lblMsg.Text = "Selecciona un registro."; return; }
                bool ok = await _api.DeleteAsync(int.Parse(hfIdEB.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al eliminar: " + ex.Message; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); lblMsg.Text = ""; }

        protected void gvEB_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvEB.SelectedRow; if (row == null) return;
            hfIdEB.Value = gvEB.SelectedDataKey.Value.ToString();

            txtInicio.Text = row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text;
            txtFin.Text = row.Cells[5].Text == "&nbsp;" ? "" : row.Cells[5].Text;
            var estado = row.Cells[6].Text == "&nbsp;" ? "Activo" : row.Cells[6].Text;
            var estItem = ddlEstado.Items.FindByValue(estado);
            if (estItem != null) ddlEstado.SelectedValue = estado;
            txtObs.Text = row.Cells[7].Text == "&nbsp;" ? "" : row.Cells[7].Text;

            var empIdText = row.Cells[2].Text == "&nbsp;" ? "" : row.Cells[2].Text;
            int empId; if (int.TryParse(empIdText, out empId))
            {
                var item = ddlEmpleado.Items.FindByValue(empId.ToString());
                if (item != null) ddlEmpleado.SelectedValue = empId.ToString();
            }

            var benIdText = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
            int benId; if (int.TryParse(benIdText, out benId))
            {
                var item = ddlBeneficio.Items.FindByValue(benId.ToString());
                if (item != null) ddlBeneficio.SelectedValue = benId.ToString();
            }
        }

        private async Task CargarAsync()
        {
            try
            {
                var lista = await _api.ListAsync();
                gvEB.DataSource = lista;
                gvEB.DataBind();
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
                ddlEmpleado.DataSource = lista; ddlEmpleado.DataBind();
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar empleados: " + ex.Message; }
        }

        private async Task CargarBeneficiosAsync()
        {
            try
            {
                var r = await _benApi.ListAsync(1, 500);
                var lista = r.Items.Select(x => new BeneficioItem
                {
                    IdBeneficio = x.IdBeneficio,
                    Nombre = x.Nombre
                }).ToList();
                ddlBeneficio.DataSource = lista; ddlBeneficio.DataBind();
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar beneficios: " + ex.Message; }
        }

        private void Limpiar()
        {
            hfIdEB.Value = "";
            if (ddlEmpleado.Items.Count > 0) ddlEmpleado.SelectedIndex = 0;
            if (ddlBeneficio.Items.Count > 0) ddlBeneficio.SelectedIndex = 0;
            ddlEstado.SelectedValue = "Activo";
            txtInicio.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFin.Text = "";
            txtObs.Text = "";
        }

        private static DateTime? ParseFecha(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            DateTime d;
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d)) return d;
            if (DateTime.TryParse(s, out d)) return d;
            return null;
        }
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

    }
}
