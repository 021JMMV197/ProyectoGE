<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPuestos.aspx.cs"
    Inherits="ProyectoGE.Pages.frmPuestos" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Puestos (API)</title></head>
<body>
<form id="form1" runat="server">
    <h2>Puestos</h2>
    <asp:HiddenField ID="hfIdPuesto" runat="server" />

    <table>
        <tr>
            <td>Nombre:</td>
            <td><asp:TextBox ID="txtNombre" runat="server" /></td>
            <td>Descripción:</td>
            <td><asp:TextBox ID="txtDescripcion" runat="server" Width="250px" /></td>
        </tr>
        <tr>
            <td>Salario Base:</td>
            <td><asp:TextBox ID="txtSalarioBase" runat="server" /></td>
            <td>Departamento:</td>
            <td><asp:DropDownList ID="ddlDepto" runat="server" DataTextField="Nombre" DataValueField="IdDepartamento"></asp:DropDownList></td>
        </tr>
    </table>

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
    <asp:GridView ID="gvPuestos" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdPuesto" OnSelectedIndexChanged="gvPuestos_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdPuesto" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
            <asp:BoundField DataField="SalarioBase" HeaderText="Salario Base" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="IdDepartamento" HeaderText="Depto" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
