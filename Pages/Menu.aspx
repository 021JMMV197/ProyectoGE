<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Menu.aspx.cs"
    Inherits="ProyectoGE.Pages.Menu" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <title>Menú</title>
  <meta charset="utf-8" />
</head>
<body>
<form id="form1" runat="server">
  <h2>Menú principal</h2>
  <asp:Label ID="lblUser" runat="server" /><br /><br />

  <h3>Gestión</h3>
  <ul>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmEmpleados.aspx"          Text="Empleados" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmDepartamentos.aspx"      Text="Departamentos" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmPuestos.aspx"            Text="Puestos" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmUsuarios.aspx"           Text="Usuarios" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmBeneficios.aspx"         Text="Beneficios" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmEmpleadoBeneficio.aspx"  Text="Empleado-Beneficio" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmAsistencia.aspx"         Text="Asistencia" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmEvaluaciones.aspx"       Text="Evaluaciones" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/frmVacaciones.aspx"         Text="Vacaciones" /></li>
  </ul>

  <h3>Reportes</h3>
  <ul>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/rptVacaciones.aspx" Text="Reporte de Vacaciones" /></li>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/rptAsistencia.aspx" Text="Reporte de Asistencia" /></li>
  </ul>

  <h3>Cuenta</h3>
  <ul>
    <li><asp:HyperLink runat="server" NavigateUrl="~/Pages/Logout.aspx" Text="Cerrar sesión" /></li>
  </ul>
</form>
</body>
</html>
