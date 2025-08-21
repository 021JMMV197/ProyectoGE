<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEmpleadoBeneficio.aspx.cs"
    Inherits="ProyectoGE.Pages.frmEmpleadoBeneficio" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <title>Empleado - Beneficios (API)</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
 
  <link href="~/Styles/Estilo.css?v=2" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-4">

  <asp:HiddenField ID="hfIdEB" runat="server" />

  <div class="card form-card">
    <div class="card-header">
      <h4 class="m-0">Empleado - Beneficios</h4>
    </div>

    <div class="card-body">
      <!-- Formulario -->
      <div class="row g-3">
        <div class="col-12 col-md-4">
          <label for="ddlEmpleado" class="form-label">Empleado</label>
          <asp:DropDownList ID="ddlEmpleado" runat="server"
                            DataTextField="NombreCompleto" DataValueField="IdEmpleado"
                            CssClass="form-select" />
        </div>

        <div class="col-12 col-md-4">
          <label for="ddlBeneficio" class="form-label">Beneficio</label>
          <asp:DropDownList ID="ddlBeneficio" runat="server"
                            DataTextField="Nombre" DataValueField="IdBeneficio"
                            CssClass="form-select" />
        </div>

        <div class="col-6 col-md-2">
          <label for="txtInicio" class="form-label">Inicio</label>
          <asp:TextBox ID="txtInicio" runat="server" CssClass="form-control" TextMode="Date" />
        </div>

        <div class="col-6 col-md-2">
          <label for="txtFin" class="form-label">Fin</label>
          <asp:TextBox ID="txtFin" runat="server" CssClass="form-control" TextMode="Date" />
        </div>

        <div class="col-6 col-md-3">
          <label for="ddlEstado" class="form-label">Estado</label>
          <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
            <asp:ListItem Text="Activo" Value="Activo" />
            <asp:ListItem Text="Inactivo" Value="Inactivo" />
            <asp:ListItem Text="Suspendido" Value="Suspendido" />
          </asp:DropDownList>
        </div>

        <div class="col-12 col-md-9">
          <label for="txtObs" class="form-label">Observación</label>
          <asp:TextBox ID="txtObs" runat="server" CssClass="form-control" />
        </div>
      </div>

      <!-- Acciones -->
      <div class="d-flex flex-wrap gap-2 mt-3">
        <asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="btn btn-pink" OnClick="btnCrear_Click" />
        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CssClass="btn btn-pink" OnClick="btnActualizar_Click" />
        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-outline-light" OnClick="btnEliminar_Click" />
        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-light" OnClick="btnLimpiar_Click" />
        <asp:Button ID="btnCargar" runat="server" Text="Cargar" CssClass="btn btn-outline-light" OnClick="btnCargar_Click" />
        <asp:Button ID="btnAtras" runat="server" Text="Atrás" CssClass="btn btn-outline-light"
                    OnClick="btnAtras_Click" CausesValidation="false" />
      </div>

      <div class="mt-3">
        <asp:Label ID="lblMsg" runat="server" CssClass="text-light d-block" />
        <asp:Label ID="lblTotal" runat="server" CssClass="text-light d-block" Text="Total: 0" />
      </div>

      <!-- Grilla -->
      <div class="table-responsive mt-3">
        <asp:GridView ID="gvEB" runat="server" AutoGenerateColumns="False"
                      CssClass="table table-striped table-bordered table-sm"
                      DataKeyNames="IdEmpleadoBeneficio"
                      OnSelectedIndexChanged="gvEB_SelectedIndexChanged"
                      GridLines="None">
          <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdEmpleadoBeneficio" HeaderText="ID" />
            <asp:BoundField DataField="IdEmpleado" HeaderText="EmpleadoId" />
            <asp:BoundField DataField="IdBeneficio" HeaderText="BeneficioId" />
            <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="Estado" HeaderText="Estado" />
            <asp:BoundField DataField="Observacion" HeaderText="Observación" />
          </Columns>
          <HeaderStyle CssClass="table-dark" />
        </asp:GridView>
      </div>
    </div>
  </div>

  <!-- (Opcional) Bootstrap JS si usas componentes dinámicos -->
  <!-- <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script> -->
</form>
</body>
</html>
