<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEmpleados.aspx.cs"
    Inherits="ProyectoGE.Pages.frmEmpleados" Async="true" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Gestión de Empleados (API)</title>
</head>
<body>
<form id="form1" runat="server">

    <h2>Empleados</h2>

    <asp:HiddenField ID="hfIdEmpleado" runat="server" />

    <table>
        <tr>
            <td>Nombre:</td>
            <td><asp:TextBox ID="txtNombre" runat="server" /></td>
            <td>Apellido:</td>
            <td><asp:TextBox ID="txtApellido" runat="server" /></td>
            <td>Correo:</td>
            <td><asp:TextBox ID="txtCorreo" runat="server" /></td>
        </tr>
        <tr>
            <td>Teléfono:</td>
            <td><asp:TextBox ID="txtTelefono" runat="server" /></td>
            <td>Dirección:</td>
            <td><asp:TextBox ID="txtDireccion" runat="server" Width="200px" /></td>
            <td>Fecha Nac. (yyyy-MM-dd):</td>
            <td><asp:TextBox ID="txtFechaNac" runat="server" /></td>
        </tr>
        <tr>
            <td>Ingreso (yyyy-MM-dd):</td>
            <td><asp:TextBox ID="txtFechaIng" runat="server" /></td>
            <td>Salario:</td>
            <td><asp:TextBox ID="txtSalario" runat="server" /></td>
            <td>Departamento Id:</td>
            <td><asp:TextBox ID="txtIdDepto" runat="server" /></td>
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

    <asp:GridView ID="gvEmpleados" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdEmpleado" OnSelectedIndexChanged="gvEmpleados_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdEmpleado" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Correo" HeaderText="Correo" />
            <asp:BoundField DataField="IdDepartamento" HeaderText="Depto" />
            <asp:BoundField DataField="FechaIngreso" HeaderText="Ingreso" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="Salario" HeaderText="Salario" DataFormatString="{0:N2}" />
        </Columns>
    </asp:GridView>

</form>
</body>
</html>
