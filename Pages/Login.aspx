<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="ProyectoGE.Pages.Login" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Login</title></head>
<body>
<form id="form1" runat="server">
    <h2>Iniciar sesión</h2>
    Usuario: <asp:TextBox ID="txtUsuario" runat="server" />
    <br />
    Contraseña: <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
    <br /><br />
    <asp:Button ID="btnLogin" runat="server" Text="Ingresar" OnClick="btnLogin_Click" />
    <br /><br />
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />
</form>
</body>
</html>
