using ProyectoGE.Infrastructure;
using System;

namespace ProyectoGE.Pages
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            // Requiere sesión; si no hay usuario, redirige a Login
            if (AuthGuard.Require(this)) return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var u = Session["Usuario"];
                string nombre = (u?.GetType().GetProperty("NombreUsuario")?.GetValue(u, null) as string)
                                ?? u?.ToString()
                                ?? "(desconocido)";
                lblUser.Text = "Conectado como: " + nombre;
            }
        }
    }
}
