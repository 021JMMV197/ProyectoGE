<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUsuarios.aspx.cs"
    Inherits="ProyectoGE.Pages.frmUsuarios" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Usuarios (API)</title></head>
<body>
<form id="form1" runat="server">
    <h2>Usuarios</h2>

    <asp:HiddenField ID="hfId" runat="server" />
    Usuario: <asp:TextBox ID="txtUsuario" runat="server" />
    &nbsp; Rol: <asp:TextBox ID="txtRol" runat="server" />
    &nbsp; Contraseña (opcional al editar): <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />

    <br /><br />
    <asp:Button ID="btnCrear" runat="server" Text="Crear" OnClick="btnCrear_Click" />
    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" />
    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" />
    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
    <asp:Button ID="btnCargar" runat="server" Text="Cargar" OnClick="btnCargar_Click" />

    <br /><br />
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />
    <br />
    <asp:Label ID="lblTotal" runat="server" Text="Total: 0" />

    <br /><br />
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdUsuario" OnSelectedIndexChanged="gvUsers_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdUsuario" HeaderText="ID" />
            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
            <asp:BoundField DataField="Rol" HeaderText="Rol" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
