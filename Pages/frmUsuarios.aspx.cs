using ProyectoGE.Models;
using System;
using System.Threading.Tasks;

namespace ProyectoGE.Pages
{
    public partial class frmUsuarios : System.Web.UI.Page
    {
        private readonly UsuarioApiClient _api = new UsuarioApiClient();

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) await CargarAsync();
        }

        protected async void btnCargar_Click(object sender, EventArgs e) => await CargarAsync();

        protected async void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPass.Text) || string.IsNullOrWhiteSpace(txtRol.Text))
                { lblMsg.Text = "Usuario, Contraseña y Rol son obligatorios."; return; }

                var dto = new UsuarioCreateDto
                {
                    NombreUsuario = txtUsuario.Text.Trim(),
                    Contrasena = txtPass.Text,
                    Rol = txtRol.Text.Trim(),
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
                if (string.IsNullOrEmpty(hfId.Value)) { lblMsg.Text = "Selecciona un usuario."; return; }

                var dto = new UsuarioUpdateDto
                {
                    IdUsuario = int.Parse(hfId.Value),
                    NombreUsuario = txtUsuario.Text.Trim(),
                    Contrasena = string.IsNullOrWhiteSpace(txtPass.Text) ? null : txtPass.Text, // null -> mantiene hash
                    Rol = txtRol.Text.Trim(),
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
                if (string.IsNullOrEmpty(hfId.Value)) { lblMsg.Text = "Selecciona un usuario."; return; }
                bool ok = await _api.DeleteAsync(int.Parse(hfId.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al eliminar: " + ex.Message; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); lblMsg.Text = ""; }

        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvUsers.SelectedRow; if (row == null) return;
            hfId.Value = gvUsers.SelectedDataKey.Value.ToString();

            // celdas: 0=select,1=ID,2=Usuario,3=Rol
            txtUsuario.Text = row.Cells[2].Text == "&nbsp;" ? "" : row.Cells[2].Text;
            txtRol.Text = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
            txtPass.Text = ""; // nunca mostramos contraseñas
        }

        private async Task CargarAsync()
        {
            try
            {
                var r = await _api.ListAsync(1, 100);
                gvUsers.DataSource = r.Items;
                gvUsers.DataBind();
                lblTotal.Text = "Total: " + r.Total;
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar: " + ex.Message; }
        }

        private void Limpiar()
        {
            hfId.Value = "";
            txtUsuario.Text = "";
            txtRol.Text = "";
            txtPass.Text = "";
        }
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

    }
}
