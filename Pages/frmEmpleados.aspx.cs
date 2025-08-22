using ProyectoGE.Infrastructure;
using ProyectoGE.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace ProyectoGE.Pages
{
    public partial class frmEmpleados : System.Web.UI.Page
    {
        private readonly EmpleadoApiClient _api = new EmpleadoApiClient();
        private readonly DepartamentoApiClient _depApi = new DepartamentoApiClient();

        private class EmpleadoRow
        {
            public int IdEmpleado { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Correo { get; set; }
            public DateTime? FechaIngreso { get; set; }
            public decimal? Salario { get; set; }
            public int IdDepartamento { get; set; }
            public string DepartamentoNombre { get; set; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (AuthGuard.Require(this)) return; 
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

        private static string FromCell(TableCell c)
        {
            var t = HttpUtility.HtmlDecode(c.Text);
            if (string.IsNullOrWhiteSpace(t) || t == "\u00A0") return "";
            return t;
        }

        protected void gvEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            var row = gvEmpleados.SelectedRow;
            if (row == null) return;

            hfIdEmpleado.Value = gvEmpleados.SelectedDataKey["IdEmpleado"].ToString();
            txtIdDepto.Text = gvEmpleados.SelectedDataKey["IdDepartamento"].ToString();

           
            txtNombre.Text = FromCell(row.Cells[2]);
            txtApellido.Text = FromCell(row.Cells[3]);
            txtCorreo.Text = FromCell(row.Cells[4]);
            txtFechaIng.Text = FromCell(row.Cells[6]);

            var sal = FromCell(row.Cells[7]);
            txtSalario.Text = sal.Replace(",", ""); 
        }

        private async Task CargarAsync()
        {
            try
            {
                var empResp = await _api.ListAsync(page: 1, pageSize: 50);

                var dictDepto = new Dictionary<int, string>();
                try
                {
                    var depResp = await _depApi.ListAsync(page: 1, pageSize: 1000);
                    if (depResp != null && depResp.Items != null)
                    {
                        foreach (var it in depResp.Items)
                        {
                            int id = GetInt(it, "IdDepartamento");
                            string nom = GetString(it, "Nombre");
                            if (id != 0 && !dictDepto.ContainsKey(id))
                                dictDepto[id] = nom ?? "";
                        }
                    }
                }
                catch
                {
                }

                var rows = new List<EmpleadoRow>();
                if (empResp != null && empResp.Items != null)
                {
                    foreach (var it in empResp.Items)
                    {
                        int idDep = GetInt(it, "IdDepartamento");
                        rows.Add(new EmpleadoRow
                        {
                            IdEmpleado = GetInt(it, "IdEmpleado"),
                            Nombre = GetString(it, "Nombre"),
                            Apellido = GetString(it, "Apellido"),
                            Correo = GetString(it, "Correo"),
                            FechaIngreso = GetDate(it, "FechaIngreso"),
                            Salario = GetDecimal(it, "Salario"),
                            IdDepartamento = idDep,
                            DepartamentoNombre = dictDepto.TryGetValue(idDep, out var nom) ? nom : ""
                        });
                    }
                }

                gvEmpleados.DataSource = rows;
                gvEmpleados.DataBind();
                lblTotal.Text = "Total: " + (empResp?.Total ?? rows.Count);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al cargar: " + ex.Message;
            }
        }

        private static string GetString(object o, string prop)
        {
            var p = o?.GetType().GetProperty(prop);
            var v = p?.GetValue(o, null);
            return v?.ToString() ?? "";
        }

        private static int GetInt(object o, string prop)
        {
            var p = o?.GetType().GetProperty(prop);
            var v = p?.GetValue(o, null);
            if (v == null) return 0;
            try { return Convert.ToInt32(v, CultureInfo.InvariantCulture); } catch { return 0; }
        }

        private static DateTime? GetDate(object o, string prop)
        {
            var p = o?.GetType().GetProperty(prop);
            var v = p?.GetValue(o, null);
            if (v == null) return null;
            if (v is DateTime dt) return dt;
            if (DateTime.TryParse(v.ToString(), out var d)) return d;
            return null;
        }

        private static decimal? GetDecimal(object o, string prop)
        {
            var p = o?.GetType().GetProperty(prop);
            var v = p?.GetValue(o, null);
            if (v == null) return null;
            if (v is decimal de) return de;
            if (decimal.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var dv)) return dv;
            if (decimal.TryParse(v.ToString(), out dv)) return dv;
            return null;
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
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var d)) return d;
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
