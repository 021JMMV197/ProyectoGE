using System.Web;
using System.Web.UI;
using ProyectoGE.Models;

namespace ProyectoGE.Infrastructure
{
    public static class AuthGuard
    {
        public static bool Require(Page page)
        {
            if (page.Session["Usuario"] == null)
            {
                // Ruta del archivo actual, siempre con .aspx (ej: "~/Pages/frmEmpleados.aspx")
                var appRel = page.Request.AppRelativeCurrentExecutionFilePath;
                var absolute = VirtualPathUtility.ToAbsolute(appRel); // "/Pages/frmEmpleados.aspx"
                var returnUrl = HttpUtility.UrlEncode(absolute);

                // Redirige al login .aspx
                var url = page.ResolveUrl("~/Pages/Login.aspx?returnUrl=" + returnUrl);
                page.Response.Redirect(url, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return true;
            }
            return false;
        }

        public static UsuarioView Current(Page page) => page.Session["Usuario"] as UsuarioView;

        public static void SignIn(Page page, UsuarioView u) => page.Session["Usuario"] = u;

        public static void Logout(Page page)
        {
            page.Session.Clear();
            page.Session.Abandon();
            page.Response.Redirect(page.ResolveUrl("~/Pages/Login.aspx"), false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
