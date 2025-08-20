using ProyectoGE.Infrastructure;
using System;

namespace ProyectoGE.Pages
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.Logout(this);
        }
    }
}
