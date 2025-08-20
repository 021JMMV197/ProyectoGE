<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptVacaciones.aspx.cs"
    Inherits="ProyectoGE.Pages.rptVacaciones" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Reporte: Vacaciones</title></head>
<body>
<form id="form1" runat="server">
    <h2>Reporte de Vacaciones (Resumen por Empleado)</h2>

    Empleado:
    <asp:DropDownList ID="ddlEmpleado" runat="server" />
    &nbsp; Desde (yyyy-MM-dd): <asp:TextBox ID="txtDesde" runat="server" />
    &nbsp; Hasta (yyyy-MM-dd): <asp:TextBox ID="txtHasta" runat="server" />
    &nbsp; Estado:
    <asp:DropDownList ID="ddlEstado" runat="server">
        <asp:ListItem Text="(Todos)" Value="" />
        <asp:ListItem Text="Pendiente" Value="Pendiente" />
        <asp:ListItem Text="Aprobado" Value="Aprobado" />
        <asp:ListItem Text="Rechazado" Value="Rechazado" />
    </asp:DropDownList>

    <br /><br />
    <asp:Button ID="btnGenerar" runat="server" Text="Generar" OnClick="btnGenerar_Click" />
    <asp:Button ID="btnPdf" runat="server" Text="Exportar PDF" OnClick="btnPdf_Click" />
    <asp:Button ID="btnLimpiar"  runat="server" Text="Limpiar"  OnClick="btnLimpiar_Click" />
    <asp:Button ID="btnAtras" runat="server"
    Text="Atrás"
    OnClick="btnAtras_Click"
    CausesValidation="false" />

    <br /><br />
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />

    <br /><br />
    <asp:GridView ID="gvResumen" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="IdEmpleado" HeaderText="Empleado ID" />
            <asp:BoundField DataField="Empleado" HeaderText="Empleado" />
            <asp:BoundField DataField="Solicitudes" HeaderText="Solicitudes" />
            <asp:BoundField DataField="TotalDias" HeaderText="Total Días" />
            <asp:BoundField DataField="DiasAprobados" HeaderText="Aprobados" />
            <asp:BoundField DataField="DiasPendientes" HeaderText="Pendientes" />
            <asp:BoundField DataField="DiasRechazados" HeaderText="Rechazados" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
