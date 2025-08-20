using ProyectoGE.Models;
using System;
using System.Threading.Tasks;

namespace ProyectoGE.Pages
{
    public partial class frmDepartamentos : System.Web.UI.Page
    {
        private readonly DepartamentoApiClient _api = new DepartamentoApiClient();

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) await CargarAsync();
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

                var dto = new DepartamentoCreateDto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
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
                if (string.IsNullOrEmpty(hfIdDepto.Value)) { lblMsg.Text = "Selecciona un registro."; return; }

                var dto = new DepartamentoUpdateDto
                {
                    IdDepartamento = int.Parse(hfIdDepto.Value),
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
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
                if (string.IsNullOrEmpty(hfIdDepto.Value)) { lblMsg.Text = "Selecciona un registro."; return; }
                bool ok = await _api.DeleteAsync(int.Parse(hfIdDepto.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al eliminar: " + ex.Message; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); lblMsg.Text = ""; }

        protected void gvDeptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvDeptos.SelectedRow; if (row == null) return;
            hfIdDepto.Value = gvDeptos.SelectedDataKey.Value.ToString();
            txtNombre.Text = row.Cells[2].Text == "&nbsp;" ? "" : row.Cells[2].Text;
            txtDescripcion.Text = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
        }

        private async Task CargarAsync()
        {
            try
            {
                var r = await _api.ListAsync(1, 50);
                gvDeptos.DataSource = r.Items;
                gvDeptos.DataBind();
                lblTotal.Text = "Total: " + r.Total;
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar: " + ex.Message; }
        }

        private void Limpiar()
        {
            hfIdDepto.Value = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
        }
    }
}
