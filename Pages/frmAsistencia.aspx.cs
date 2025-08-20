using ProyectoGE.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoGE.Pages
{
    public partial class frmAsistencia : System.Web.UI.Page
    {
        private readonly AsistenciaApiClient _api = new AsistenciaApiClient();
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
                var dto = new AsistenciaCreateDto
                {
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    Fecha = ParseFecha(txtFecha.Text) ?? DateTime.Today,
                    HoraEntrada = ParseHora(txtFecha.Text, txtEntrada.Text),
                    HoraSalida = ParseHora(txtFecha.Text, txtSalida.Text),
                    Observacion = string.IsNullOrWhiteSpace(txtObs.Text) ? null : txtObs.Text.Trim(),
                    Adicionado_Por = "webforms"
                };
                if (dto.HoraEntrada.HasValue && dto.HoraSalida.HasValue && dto.HoraSalida < dto.HoraEntrada)
                { lblMsg.Text = "Salida < Entrada."; return; }

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
                if (string.IsNullOrEmpty(hfIdAsistencia.Value)) { lblMsg.Text = "Selecciona un registro."; return; }

                var dto = new AsistenciaUpdateDto
                {
                    IdAsistencia = int.Parse(hfIdAsistencia.Value),
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    Fecha = ParseFecha(txtFecha.Text) ?? DateTime.Today,
                    HoraEntrada = ParseHora(txtFecha.Text, txtEntrada.Text),
                    HoraSalida = ParseHora(txtFecha.Text, txtSalida.Text),
                    Observacion = string.IsNullOrWhiteSpace(txtObs.Text) ? null : txtObs.Text.Trim(),
                    Modificado_Por = "webforms"
                };
                if (dto.HoraEntrada.HasValue && dto.HoraSalida.HasValue && dto.HoraSalida < dto.HoraEntrada)
                { lblMsg.Text = "Salida < Entrada."; return; }

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
                if (string.IsNullOrEmpty(hfIdAsistencia.Value)) { lblMsg.Text = "Selecciona un registro."; return; }
                bool ok = await _api.DeleteAsync(int.Parse(hfIdAsistencia.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al eliminar: " + ex.Message; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); lblMsg.Text = ""; }

        protected void gvAsis_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvAsis.SelectedRow; if (row == null) return;
            hfIdAsistencia.Value = gvAsis.SelectedDataKey.Value.ToString();

            txtFecha.Text = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
            txtEntrada.Text = row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text;
            txtSalida.Text = row.Cells[5].Text == "&nbsp;" ? "" : row.Cells[5].Text;
            txtObs.Text = row.Cells[6].Text == "&nbsp;" ? "" : row.Cells[6].Text;

            var empIdText = row.Cells[2].Text == "&nbsp;" ? "" : row.Cells[2].Text;
            int empId;
            if (int.TryParse(empIdText, out empId))
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
                gvAsis.DataSource = lista;
                gvAsis.DataBind();
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
            hfIdAsistencia.Value = "";
            if (ddlEmpleado.Items.Count > 0) ddlEmpleado.SelectedIndex = 0;
            txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtEntrada.Text = txtSalida.Text = "";
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

        private static DateTime? ParseHora(string fecha, string hora)
        {
            if (string.IsNullOrWhiteSpace(hora)) return null;
            var f = ParseFecha(fecha) ?? DateTime.Today;
            DateTime dt;
            // acepta "HH:mm"
            if (DateTime.TryParseExact(hora.Trim(), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return new DateTime(f.Year, f.Month, f.Day, dt.Hour, dt.Minute, 0);
            // acepta hora local general
            if (DateTime.TryParse(hora, out dt))
                return new DateTime(f.Year, f.Month, f.Day, dt.Hour, dt.Minute, 0);
            return null;
        }
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

    }
}
