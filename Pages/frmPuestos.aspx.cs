using ProyectoGE.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ProyectoGE.Pages
{
    public partial class frmPuestos : System.Web.UI.Page
    {
        private readonly PuestoApiClient _api = new PuestoApiClient();
        private readonly DepartamentoApiClient _depApi = new DepartamentoApiClient(); // usamos el que creamos antes

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarDepartamentosAsync();
                await CargarAsync();
            }
        }

        protected async void btnCargar_Click(object sender, EventArgs e) => await CargarAsync();

        protected async void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    lblMsg.Text = "Nombre es obligatorio."; return;
                }

                var dto = new PuestoCreateDto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                    SalarioBase = ParseDecimal(txtSalarioBase.Text),
                    IdDepartamento = int.Parse(ddlDepto.SelectedValue),
                    Adicionado_Por = "webforms"
                };

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
                if (string.IsNullOrEmpty(hfIdPuesto.Value)) { lblMsg.Text = "Selecciona un registro."; return; }

                var dto = new PuestoUpdateDto
                {
                    IdPuesto = int.Parse(hfIdPuesto.Value),
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                    SalarioBase = ParseDecimal(txtSalarioBase.Text),
                    IdDepartamento = int.Parse(ddlDepto.SelectedValue),
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
                if (string.IsNullOrEmpty(hfIdPuesto.Value)) { lblMsg.Text = "Selecciona un registro."; return; }
                bool ok = await _api.DeleteAsync(int.Parse(hfIdPuesto.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al eliminar: " + ex.Message; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); lblMsg.Text = ""; }

        protected void gvPuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvPuestos.SelectedRow; if (row == null) return;
            hfIdPuesto.Value = gvPuestos.SelectedDataKey.Value.ToString();
            txtNombre.Text = row.Cells[2].Text == "&nbsp;" ? "" : row.Cells[2].Text;
            txtDescripcion.Text = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
            txtSalarioBase.Text = row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text.Replace(",", "");
            // ddl depto: busca el valor si existe
            var val = row.Cells[5].Text == "&nbsp;" ? "" : row.Cells[5].Text;
            if (int.TryParse(val, out var deptoId))
            {
                var item = ddlDepto.Items.FindByValue(deptoId.ToString());
                if (item != null) ddlDepto.SelectedValue = deptoId.ToString();
            }
        }

        private async Task CargarAsync()
        {
            try
            {
                var r = await _api.ListAsync(1, 50);
                gvPuestos.DataSource = r.Items;
                gvPuestos.DataBind();
                lblTotal.Text = "Total: " + r.Total;
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar: " + ex.Message; }
        }

        private async Task CargarDepartamentosAsync()
        {
            try
            {
                var r = await _depApi.ListAsync(1, 200);
                ddlDepto.DataSource = r.Items;
                ddlDepto.DataBind();
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar departamentos: " + ex.Message; }
        }

        private void Limpiar()
        {
            hfIdPuesto.Value = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtSalarioBase.Text = "";
            if (ddlDepto.Items.Count > 0) ddlDepto.SelectedIndex = 0;
        }

        private static decimal? ParseDecimal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v)) return v;
            if (decimal.TryParse(s, out v)) return v;
            return null;
        }
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }
        protected string GetDeptoNombre(object idObj)
        {
            if (idObj == null) return string.Empty;
            if (!int.TryParse(idObj.ToString(), out var id)) return string.Empty;

            // ddlDepto debe estar cargado con IdDepartamento -> Nombre antes de DataBind del Grid
            var item = ddlDepto.Items.FindByValue(id.ToString());
            return item?.Text ?? string.Empty;
        }


    }
}
