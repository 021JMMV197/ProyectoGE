<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDepartamentos.aspx.cs"
    Inherits="ProyectoGE.Pages.frmDepartamentos" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Departamentos (API)</title></head>
<body>
<form id="form1" runat="server">

    <h2>Departamentos</h2>
    <asp:HiddenField ID="hfIdDepto" runat="server" />

    Nombre: <asp:TextBox ID="txtNombre" runat="server" />
    &nbsp;Descripción: <asp:TextBox ID="txtDescripcion" runat="server" Width="250px" />

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
    <asp:GridView ID="gvDeptos" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdDepartamento" OnSelectedIndexChanged="gvDeptos_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdDepartamento" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
        </Columns>
    </asp:GridView>

</form>
</body>
</html>
