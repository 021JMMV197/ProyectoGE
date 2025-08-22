<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUsuarios.aspx.cs"
    Inherits="ProyectoGE.Pages.frmUsuarios" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <title>Usuarios (API)</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />

  
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  
  <link href="~/Styles/Estilo.css?v=3" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-4">
  <asp:HiddenField ID="hfId" runat="server" />

  <div class="card form-card">
    <div class="card-header">
      <h4 class="m-0">Usuarios</h4>
    </div>

    <div class="card-body">
      <!-- Formulario -->
      <div class="row g-3">
        <div class="col-12 col-md-4">
          <label for="txtUsuario" class="form-label">Usuario</label>
          <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Nombre de usuario" />
        </div>

        <div class="col-12 col-md-4">
          <label for="txtRol" class="form-label">Rol</label>
          <asp:TextBox ID="txtRol" runat="server" CssClass="form-control" placeholder="Ej: Admin, Operador" />
        </div>

        <div class="col-12 col-md-4">
          <label for="txtPass" class="form-label">Contraseña (opcional al editar)</label>
          <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••" />
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
        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False"
                      CssClass="table table-striped table-bordered table-sm"
                      DataKeyNames="IdUsuario"
                      OnSelectedIndexChanged="gvUsers_SelectedIndexChanged"
                      GridLines="None">
          <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdUsuario" HeaderText="ID" />
            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
            <asp:BoundField DataField="Rol" HeaderText="Rol" />
          </Columns>
          <HeaderStyle CssClass="table-dark" />
        </asp:GridView>
      </div>
    </div>
  </div>

 
</form>
</body>
</html>
