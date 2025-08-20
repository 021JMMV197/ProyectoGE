using ProyectoGE.Models;
using ProyectoGE.Infrastructure;
using System;
using System.Web;           // <- para HttpContext

namespace ProyectoGE.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly UsuarioApiClient _api = new UsuarioApiClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["Usuario"] != null)
            {
                // Redirección segura (no aborta hilo)
                var destino = ResolveUrl("~/Pages/Menu.aspx");
                Response.Redirect(destino, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }
        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            try
            {
                var req = new LoginRequest
                {
                    NombreUsuario = txtUsuario.Text?.Trim(),
                    Contrasena = txtPass.Text
                };

                if (string.IsNullOrWhiteSpace(req.NombreUsuario) || string.IsNullOrWhiteSpace(req.Contrasena))
                {
                    lblMsg.Text = "Usuario y contraseña son obligatorios.";
                    return;
                }

                UsuarioView u = await _api.LoginAsync(req);
                if (u == null)
                {
                    lblMsg.Text = "Credenciales inválidas.";
                    return;
                }

                AuthGuard.SignIn(this, u);

                // Validar returnUrl (solo URLs locales y con .aspx)
                var returnUrl = Request.QueryString["returnUrl"];
                string destino;
                if (!string.IsNullOrWhiteSpace(returnUrl)
                    && returnUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase)
                    && returnUrl.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                {
                    destino = returnUrl; // e.g. /Pages/frmEmpleados.aspx
                }
                else
                {
                    destino = "~/Pages/Menu.aspx";
                }

                Response.Redirect(ResolveUrl(destino), false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al iniciar sesión: " + ex.Message;
            }
        }
    }
}
