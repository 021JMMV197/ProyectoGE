using ProyectoGE.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ProyectoGE.Pages
{
    public partial class frmBeneficios : System.Web.UI.Page
    {
        private readonly BeneficioApiClient _api = new BeneficioApiClient();

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
                { lblMsg.Text = "Nombre es obligatorio."; return; }

                var dto = new BeneficioCreateDto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Tipo = string.IsNullOrWhiteSpace(txtTipo.Text) ? null : txtTipo.Text.Trim(),
                    MontoMensual = ParseDecimal(txtMonto.Text),
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
                if (string.IsNullOrEmpty(hfIdBeneficio.Value)) { lblMsg.Text = "Selecciona un registro."; return; }

                var dto = new BeneficioUpdateDto
                {
                    IdBeneficio = int.Parse(hfIdBeneficio.Value),
                    Nombre = txtNombre.Text.Trim(),
                    Tipo = string.IsNullOrWhiteSpace(txtTipo.Text) ? null : txtTipo.Text.Trim(),
                    MontoMensual = ParseDecimal(txtMonto.Text),
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
                if (string.IsNullOrEmpty(hfIdBeneficio.Value)) { lblMsg.Text = "Selecciona un registro."; return; }
                bool ok = await _api.DeleteAsync(int.Parse(hfIdBeneficio.Value));
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                Limpiar();
                await CargarAsync();
            }
            catch (Exception ex) { lblMsg.Text = "Error al eliminar: " + ex.Message; }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e) { Limpiar(); lblMsg.Text = ""; }

        protected void gvBenef_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvBenef.SelectedRow; if (row == null) return;
            hfIdBeneficio.Value = gvBenef.SelectedDataKey.Value.ToString();

            // celdas: 0=select,1=ID,2=Nombre,3=Tipo,4=Monto,5=Descripcion
            txtNombre.Text = row.Cells[2].Text == "&nbsp;" ? "" : row.Cells[2].Text;
            txtTipo.Text = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
            txtMonto.Text = row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text.Replace(",", "");
            txtDescripcion.Text = row.Cells[5].Text == "&nbsp;" ? "" : row.Cells[5].Text;
        }

        private async Task CargarAsync()
        {
            try
            {
                var r = await _api.ListAsync(1, 100); // página única grandecita
                gvBenef.DataSource = r.Items;
                gvBenef.DataBind();
                lblTotal.Text = "Total: " + r.Total;
            }
            catch (Exception ex) { lblMsg.Text = "Error al cargar: " + ex.Message; }
        }

        private void Limpiar()
        {
            hfIdBeneficio.Value = "";
            txtNombre.Text = "";
            txtTipo.Text = "";
            txtMonto.Text = "";
            txtDescripcion.Text = "";
        }

        private static decimal? ParseDecimal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            decimal v;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v)) return v;
            if (decimal.TryParse(s, out v)) return v;
            return null;
        }
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

    }
}
