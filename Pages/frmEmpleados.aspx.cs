using Newtonsoft.Json; 
using ProyectoGE.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ProyectoGE.Infrastructure;

namespace ProyectoGE.Pages
{
    public partial class frmEmpleados : System.Web.UI.Page
    {
        private readonly EmpleadoApiClient _api = new EmpleadoApiClient();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (AuthGuard.Require(this)) return; // si redirige, salir
        }

        protected async void Page_Load(object sender, EventArgs e)
        { 
            if (!IsPostBack)
                await CargarAsync();
        }

        protected async void btnCargar_Click(object sender, EventArgs e)
        {
            await CargarAsync();
        }

        protected async void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                var dto = new EmpleadoCreateDto
                {
                    Nombre = txtNombre.Text?.Trim(),
                    Apellido = txtApellido.Text?.Trim(),
                    Correo = txtCorreo.Text?.Trim(),
                    Telefono = txtTelefono.Text?.Trim(),
                    Direccion = txtDireccion.Text?.Trim(),
                    FechaNacimiento = ParseFecha(txtFechaNac.Text),
                    FechaIngreso = ParseFecha(txtFechaIng.Text),
                    Salario = ParseDecimal(txtSalario.Text),
                    IdDepartamento = ParseInt(txtIdDepto.Text),
                    Adicionado_Por = "webforms"
                };

                if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Apellido))
                {
                    lblMsg.Text = "Nombre y Apellido son obligatorios.";
                    return;
                }

                int nuevoId = await _api.CreateAsync(dto);
                lblMsg.Text = "Creado con Id: " + nuevoId;
                LimpiarForm();
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
                if (string.IsNullOrEmpty(hfIdEmpleado.Value))
                {
                    lblMsg.Text = "Selecciona un empleado en la grilla para actualizar.";
                    return;
                }

                var dto = new EmpleadoUpdateDto
                {
                    IdEmpleado = int.Parse(hfIdEmpleado.Value),
                    Nombre = txtNombre.Text?.Trim(),
                    Apellido = txtApellido.Text?.Trim(),
                    Correo = txtCorreo.Text?.Trim(),
                    Telefono = txtTelefono.Text?.Trim(),
                    Direccion = txtDireccion.Text?.Trim(),
                    FechaNacimiento = ParseFecha(txtFechaNac.Text),
                    FechaIngreso = ParseFecha(txtFechaIng.Text),
                    Salario = ParseDecimal(txtSalario.Text),
                    IdDepartamento = ParseInt(txtIdDepto.Text),
                    Modificado_Por = "webforms"
                };

                bool ok = await _api.UpdateAsync(dto);
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
                if (string.IsNullOrEmpty(hfIdEmpleado.Value))
                {
                    lblMsg.Text = "Selecciona un empleado para eliminar.";
                    return;
                }

                int id = int.Parse(hfIdEmpleado.Value);
                bool ok = await _api.DeleteAsync(id);
                lblMsg.Text = ok ? "Eliminado." : "No encontrado.";
                LimpiarForm();
                await CargarAsync();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al eliminar: " + ex.Message;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarForm();
            lblMsg.Text = "";
        }

        protected void gvEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Toma valores de la fila seleccionada
            var row = gvEmpleados.SelectedRow;
            if (row == null) return;

            hfIdEmpleado.Value = gvEmpleados.SelectedDataKey.Value.ToString();
            txtNombre.Text = row.Cells[2].Text == "&nbsp;" ? "" : row.Cells[2].Text;
            txtApellido.Text = row.Cells[3].Text == "&nbsp;" ? "" : row.Cells[3].Text;
            txtCorreo.Text = row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text;
            txtIdDepto.Text = row.Cells[5].Text == "&nbsp;" ? "" : row.Cells[5].Text;
            txtFechaIng.Text = row.Cells[6].Text == "&nbsp;" ? "" : row.Cells[6].Text;
            txtSalario.Text = row.Cells[7].Text == "&nbsp;" ? "" : row.Cells[7].Text.Replace(",", ""); // por formato N2
        }

        private async Task CargarAsync()
        {
            try
            {
                var r = await _api.ListAsync(page: 1, pageSize: 20);
                gvEmpleados.DataSource = r.Items;
                gvEmpleados.DataBind();
                lblTotal.Text = "Total: " + r.Total;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al cargar: " + ex.Message;
            }
        }

        private void LimpiarForm()
        {
            hfIdEmpleado.Value = "";
            txtNombre.Text = txtApellido.Text = txtCorreo.Text = txtTelefono.Text = txtDireccion.Text = "";
            txtFechaNac.Text = txtFechaIng.Text = txtSalario.Text = txtIdDepto.Text = "";
        }

        private static DateTime? ParseFecha(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                return d;
            return DateTime.TryParse(s, out d) ? d : (DateTime?)null;
        }

        private static decimal? ParseDecimal(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v)) return v;
            if (decimal.TryParse(s, out v)) return v;
            return null;
        }

        private static int ParseInt(string s) => int.TryParse(s, out var v) ? v : 0;
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

    }
}
