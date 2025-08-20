<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAsistencia.aspx.cs"
    Inherits="ProyectoGE.Pages.frmAsistencia" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Asistencia (API)</title></head>
<body>
<form id="form1" runat="server">
    <h2>Asistencia</h2>

    <asp:HiddenField ID="hfIdAsistencia" runat="server" />
    Empleado: <asp:DropDownList ID="ddlEmpleado" runat="server" DataTextField="NombreCompleto" DataValueField="IdEmpleado" />
    &nbsp; Fecha (yyyy-MM-dd): <asp:TextBox ID="txtFecha" runat="server" />
    &nbsp; Entrada (HH:mm): <asp:TextBox ID="txtEntrada" runat="server" Width="70px" />
    &nbsp; Salida (HH:mm): <asp:TextBox ID="txtSalida" runat="server" Width="70px" />
    &nbsp; Obs: <asp:TextBox ID="txtObs" runat="server" Width="220px" />

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
    <asp:GridView ID="gvAsis" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdAsistencia" OnSelectedIndexChanged="gvAsis_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdAsistencia" HeaderText="ID" />
            <asp:BoundField DataField="IdEmpleado" HeaderText="EmpleadoId" />
            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="HoraEntrada" HeaderText="Entrada" DataFormatString="{0:HH:mm}" />
            <asp:BoundField DataField="HoraSalida" HeaderText="Salida" DataFormatString="{0:HH:mm}" />
            <asp:BoundField DataField="Observacion" HeaderText="Obs" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
