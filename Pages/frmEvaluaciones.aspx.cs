using ProyectoGE.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoGE.Pages
{
    public partial class frmEvaluaciones : System.Web.UI.Page
    {
        private readonly EvaluacionApiClient _api = new EvaluacionApiClient();
        private readonly EmpleadoApiClient _empApi = new EmpleadoApiClient();

        private class EmpleadoItem { public int IdEmpleado { get; set; } public string NombreCompleto { get; set; } }

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
                var dto = new EvaluacionCreateDto
                {
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    PeriodoInicio = ParseFecha(txtInicio.Text) ?? DateTime.Today,
                    PeriodoFin = ParseFecha(txtFin.Text) ?? DateTime.Today,
                    Calificacion = ParseDecimal(txtCalif.Text) ?? 0m,
                    Comentarios = string.IsNullOrWhiteSpace(txtComentarios.Text) ? null : txtComentarios.Text.Trim(),
                    IdEvaluador = ParseIntNullable(txtEvaluador.Text),
                    Adicionado_Por = "webforms"
                };
                if (dto.PeriodoFin < dto.PeriodoInicio) { lblMsg.Text = "Fin < Inicio."; return; }
                if (dto.Calificacion < 0 || dto.Calificacion > 10) { lblMsg.Text = "Calificación 0..10."; return; }

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
                if (string.IsNullOrEmpty(hfIdEval.Value)) { lblMsg.Text = "Selecciona un registro."; return; }

                var dto = new EvaluacionUpdateDto
                {
                    IdEvaluacion = int.Parse(hfIdEval.Value),
                    IdEmpleado = int.Parse(ddlEmpleado.SelectedValue),
                    PeriodoInicio = ParseFecha(txtInicio.Text) ?? DateTime.Today,
                    PeriodoFin = ParseFecha(txtFin.Text) ?? DateTime.Today,
                    Calificacion = ParseDecimal(txtCalif.Text) ?? 0m,
                    Comentarios = string.IsNullOrWhiteSpace(txtComentarios.Text) ? null : txtComentarios.Text.Trim(),
                    IdEvaluador = ParseIntNullable(txtEvaluador.Text),
                    Modificado_Por = "webforms"
                };
                if (dto.PeriodoFin < dto.PeriodoInicio) { lblMsg.Text = "Fin < Inicio."; return; }
                if (dto.Calificacion < 0 || dto.Calificacion > 10) { lblMsg.Text = "Calificación 0..10."; return; }

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
                if (string.IsNullOrEmpty(hfIdEval.Value)) { lblMsg.Text = "Selecciona un registro."; return; }
                bool ok = await _api.DeleteAsync(int.Parse(hfIdEval.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al eliminar: " + ex.Message; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); lblMsg.Text = ""; }

        protected void gvEval_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvEval.SelectedRow; if (row == null) return;
            hfIdEval.Value = gvEval.SelectedDataKey.Value.ToString();

            // celdas: 0=select,1=ID,2=IdEmpleado,3=Inicio,4=Fin,5=Calif,6=EvaluadorId,7=Comentarios
            txtInicio.Text = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
            txtFin.Text = row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text;
            txtCalif.Text = row.Cells[5].Text == "&nbsp;" ? "" : row.Cells[5].Text;
            txtEvaluador.Text = row.Cells[6].Text == "&nbsp;" ? "" : row.Cells[6].Text;
            txtComentarios.Text = row.Cells[7].Text == "&nbsp;" ? "" : row.Cells[7].Text;

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
                gvEval.DataSource = lista;
                gvEval.DataBind();
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
            hfIdEval.Value = "";
            if (ddlEmpleado.Items.Count > 0) ddlEmpleado.SelectedIndex = 0;
            txtInicio.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFin.Text = "";
            txtCalif.Text = "";
            txtEvaluador.Text = "";
            txtComentarios.Text = "";
        }

        private static DateTime? ParseFecha(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            DateTime d;
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d)) return d;
            if (DateTime.TryParse(s, out d)) return d;
            return null;
        }

        private static decimal? ParseDecimal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            decimal v;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;
            if (decimal.TryParse(s, out v)) return v;
            return null;
        }

        private static int? ParseIntNullable(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            int v; return int.TryParse(s, out v) ? v : (int?)null;
        }
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

    }
}
