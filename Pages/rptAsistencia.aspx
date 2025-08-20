<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="rptAsistencia.aspx.cs"
    Inherits="ProyectoGE.Pages.rptAsistencia"
    Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Reporte – Asistencia (Resumen)</title>
    <meta charset="utf-8" />
</head>
<body>
    <form id="form1" runat="server">
        <h2>Reporte – Asistencia (Resumen)</h2>

        <div style="margin-bottom:10px;">
            <table>
                <tr>
                    <td>Empleado:</td>
                    <td>
                        <asp:DropDownList ID="ddlEmpleado" runat="server" Width="260" />
                    </td>
                    <td style="padding-left:15px;">Desde (yyyy-MM-dd):</td>
                    <td><asp:TextBox ID="txtDesde" runat="server" Width="110" /></td>
                    <td style="padding-left:15px;">Hasta (yyyy-MM-dd):</td>
                    <td><asp:TextBox ID="txtHasta" runat="server" Width="110" /></td>
                    <td style="padding-left:15px;">Hora tarde (HH:mm):</td>
                    <td><asp:TextBox ID="txtHoraTarde" runat="server" Width="70" Text="09:15" /></td>
                </tr>
            </table>
        </div>

        <div style="margin-bottom:10px;">
            <asp:Button ID="btnGenerar" runat="server" Text="Generar" OnClick="btnGenerar_Click" />
            <asp:Button ID="btnPdf" runat="server" Text="Exportar PDF" OnClick="btnPdf_Click" />
            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
            <asp:Button ID="btnAtras" runat="server"
    Text="Atrás"
    OnClick="btnAtras_Click"
    CausesValidation="false" />

        </div>

        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" /><br />

        <asp:GridView ID="gvResumen" runat="server" AutoGenerateColumns="False" GridLines="Both">
            <Columns>
                <asp:BoundField DataField="IdEmpleado" HeaderText="ID" />
                <asp:BoundField DataField="Empleado" HeaderText="Empleado" />
                <asp:BoundField DataField="Registros" HeaderText="Registros" />
                <asp:BoundField DataField="LlegadasTarde" HeaderText="Tarde" />
                <asp:BoundField DataField="SinSalida" HeaderText="Sin Salida" />
                <asp:BoundField DataField="MinutosTrabajados" HeaderText="Min. Trabajados" />
                <asp:BoundField DataField="PromedioMinutosPorRegistro" HeaderText="Promedio/Reg." />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
