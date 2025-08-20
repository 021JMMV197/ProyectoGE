<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmVacaciones.aspx.cs"
    Inherits="ProyectoGE.Pages.frmVacaciones" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Vacaciones (API)</title></head>
<body>
<form id="form1" runat="server">
    <h2>Vacaciones</h2>

    <asp:HiddenField ID="hfIdVacacion" runat="server" />

    Empleado:
    <asp:DropDownList ID="ddlEmpleado" runat="server" DataTextField="NombreCompleto" DataValueField="IdEmpleado" />
    &nbsp; Inicio (yyyy-MM-dd): <asp:TextBox ID="txtInicio" runat="server" />
    &nbsp; Fin (yyyy-MM-dd): <asp:TextBox ID="txtFin" runat="server" />
    &nbsp; Días: <asp:TextBox ID="txtDias" runat="server" Width="50px" />
    &nbsp; Estado:
    <asp:DropDownList ID="ddlEstado" runat="server">
        <asp:ListItem Text="Pendiente" Value="Pendiente" />
        <asp:ListItem Text="Aprobada" Value="Aprobada" />
        <asp:ListItem Text="Rechazada" Value="Rechazada" />
    </asp:DropDownList>

    <br /><br />
    <asp:Button ID="btnCrear" runat="server" Text="Crear" OnClick="btnCrear_Click" />
    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" />
    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click" />
    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
    <asp:Button ID="btnCargar" runat="server" Text="Cargar" OnClick="btnCargar_Click" />
    <asp:Button ID="btnAtras" runat="server"
    Text="Atrás"
    OnClick="btnAtras_Click"
    CausesValidation="false" />

    <br /><br />
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />
    <br />
    <asp:Label ID="lblTotal" runat="server" Text="Total: 0" />

    <br /><br />
    <asp:GridView ID="gvVac" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdVacacion" OnSelectedIndexChanged="gvVac_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdVacacion" HeaderText="ID" />
            <asp:BoundField DataField="IdEmpleado" HeaderText="EmpleadoId" />
            <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="CantidadDias" HeaderText="Días" />
            <asp:BoundField DataField="Estado" HeaderText="Estado" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
