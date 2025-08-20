<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEmpleadoBeneficio.aspx.cs"
    Inherits="ProyectoGE.Pages.frmEmpleadoBeneficio" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Empleado - Beneficios (API)</title></head>
<body>
<form id="form1" runat="server">
    <h2>Empleado - Beneficios</h2>

    <asp:HiddenField ID="hfIdEB" runat="server" />

    Empleado:
    <asp:DropDownList ID="ddlEmpleado" runat="server" DataTextField="NombreCompleto" DataValueField="IdEmpleado" />
    &nbsp; Beneficio:
    <asp:DropDownList ID="ddlBeneficio" runat="server" DataTextField="Nombre" DataValueField="IdBeneficio" />
    <br />
    Inicio (yyyy-MM-dd): <asp:TextBox ID="txtInicio" runat="server" />
    &nbsp; Fin (yyyy-MM-dd): <asp:TextBox ID="txtFin" runat="server" />
    &nbsp; Estado:
    <asp:DropDownList ID="ddlEstado" runat="server">
        <asp:ListItem Text="Activo" Value="Activo" />
        <asp:ListItem Text="Inactivo" Value="Inactivo" />
        <asp:ListItem Text="Suspendido" Value="Suspendido" />
    </asp:DropDownList>
    <br />
    Observación: <asp:TextBox ID="txtObs" runat="server" Width="420px" />

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
    <asp:GridView ID="gvEB" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdEmpleadoBeneficio" OnSelectedIndexChanged="gvEB_SelectedIndexChanged">
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
    </asp:GridView>
</form>
</body>
</html>
