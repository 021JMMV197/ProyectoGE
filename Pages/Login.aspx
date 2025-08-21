<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="ProyectoGE.Pages.Login" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <title>Login</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  <link href="~/Styles/Estilo.css?v=2" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-5">
  <div class="row justify-content-center">
    <div class="col-12 col-sm-10 col-md-6 col-lg-4">
      <div class="card form-card">
        <div class="card-header">
          <h4 class="m-0">Iniciar sesión</h4>
        </div>
        <div class="card-body">
          <div class="mb-3">
            <label for="txtUsuario" class="form-label">Usuario</label>
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Tu usuario" />
          </div>

          <div class="mb-3">
            <label for="txtPass" class="form-label">Contraseña</label>
            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••" />
          </div>

          <asp:Button ID="btnLogin" runat="server" Text="Ingresar"
                      CssClass="btn btn-pink w-100" OnClick="btnLogin_Click" />

          <asp:Label ID="lblMsg" runat="server" CssClass="text-light d-block mt-3" />
        </div>
      </div>
    </div>
  </div>
</form>
</body>
</html>

