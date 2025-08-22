<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEmpleados.aspx.cs"
    Inherits="ProyectoGE.Pages.frmEmpleados" Async="true" %>

<!DOCTYPE html>
<html>
<head runat="server">
  <title>Gestión de Empleados (API)</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />

  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  <link href="~/Styles/Estilo.css?v=3" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-4">

  <asp:HiddenField ID="hfIdEmpleado" runat="server" />

  <div class="card form-card">
    <div class="card-header">
      <h4 class="m-0">Empleados</h4>
    </div>

    <div class="card-body">
      <!-- Formulario -->
      <div class="row g-3">
        <div class="col-12 col-md-3">
          <label for="txtNombre" class="form-label">Nombre</label>
          <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-3">
          <label for="txtApellido" class="form-label">Apellido</label>
          <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
        </div>
        <div class="col-12 col-md-3">
          <label for="txtCorreo" class="form-label">Correo</label>
          <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode="Email" />
        </div>
        <div class="col-12 col-md-3">
          <label for="txtTelefono" class="form-label">Teléfono</label>
          <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
        </div>

        <div class="col-12">
          <label for="txtDireccion" class="form-label">Dirección</label>
          <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
        </div>

        <div class="col-6 col-md-3">
          <label for="txtFechaNac" class="form-label">Fecha Nac.</label>
          <asp:TextBox ID="txtFechaNac" runat="server" CssClass="form-control" TextMode="Date" />
        </div>
        <div class="col-6 col-md-3">
          <label for="txtFechaIng" class="form-label">Ingreso</label>
          <asp:TextBox ID="txtFechaIng" runat="server" CssClass="form-control" TextMode="Date" />
        </div>
        <div class="col-6 col-md-3">
          <label for="txtSalario" class="form-label">Salario</label>
          <asp:TextBox ID="txtSalario" runat="server" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="col-6 col-md-3">
          <label for="txtIdDepto" class="form-label">Departamento Id</label>
          <asp:TextBox ID="txtIdDepto" runat="server" CssClass="form-control" />
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
        <asp:GridView ID="gvEmpleados" runat="server" AutoGenerateColumns="False"
                      CssClass="table table-striped table-bordered table-sm"
                      DataKeyNames="IdEmpleado,IdDepartamento"
                      OnSelectedIndexChanged="gvEmpleados_SelectedIndexChanged"
                      GridLines="None">
          <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdEmpleado" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Correo" HeaderText="Correo" />
            <asp:TemplateField HeaderText="Departamento">
              <ItemTemplate>
                <%# Eval("DepartamentoNombre") %>
              </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FechaIngreso" HeaderText="Ingreso" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="Salario" HeaderText="Salario" DataFormatString="{0:N2}" />
          </Columns>
          <HeaderStyle CssClass="table-dark" />
        </asp:GridView>
      </div>
    </div>
  </div>
</form>
</body>
</html>
