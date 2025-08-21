<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Menu.aspx.cs"
    Inherits="ProyectoGE.Pages.Menu" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <title>Menú</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  
  <link href="~/Styles/Estilo.css?v=2" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="p-3">
  <div class="container">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="m-0">Menú principal</h2>
      <asp:Label ID="lblUser" runat="server" CssClass="text-muted" />
    </div>

    <!-- Gestión -->
    <div class="card form-card mb-3">
      <div class="card-header">
        <h5 class="m-0">Gestión</h5>
      </div>
      <div class="card-body">
        <div class="row g-2">
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmEmpleados.aspx" CssClass="btn btn-pink w-100" Text="Empleados" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmDepartamentos.aspx" CssClass="btn btn-pink w-100" Text="Departamentos" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmPuestos.aspx" CssClass="btn btn-pink w-100" Text="Puestos" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmUsuarios.aspx" CssClass="btn btn-pink w-100" Text="Usuarios" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmBeneficios.aspx" CssClass="btn btn-pink w-100" Text="Beneficios" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmEmpleadoBeneficio.aspx" CssClass="btn btn-pink w-100" Text="Empleado-Beneficio" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmAsistencia.aspx" CssClass="btn btn-pink w-100" Text="Asistencia" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmEvaluaciones.aspx" CssClass="btn btn-pink w-100" Text="Evaluaciones" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/frmVacaciones.aspx" CssClass="btn btn-pink w-100" Text="Vacaciones" />
          </div>
        </div>
      </div>
    </div>

    <!-- Reportes -->
    <div class="card form-card mb-3">
      <div class="card-header">
        <h5 class="m-0">Reportes</h5>
      </div>
      <div class="card-body">
        <div class="row g-2">
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/rptVacaciones.aspx" CssClass="btn btn-pink w-100" Text="Reporte de Vacaciones" />
          </div>
          <div class="col-12 col-sm-6 col-md-4 col-lg-3">
            <asp:HyperLink runat="server" NavigateUrl="~/Pages/rptAsistencia.aspx" CssClass="btn btn-pink w-100" Text="Reporte de Asistencia" />
          </div>
        </div>
      </div>
    </div>

    <!-- Cuenta -->
    <div class="card form-card">
      <div class="card-header">
        <h5 class="m-0">Cuenta</h5>
      </div>
      <div class="card-body">
        <asp:HyperLink runat="server" NavigateUrl="~/Pages/Logout.aspx" CssClass="btn btn-pink" Text="Cerrar sesión" />
      </div>
    </div>
  </div>
</form>
</body>
</html>
