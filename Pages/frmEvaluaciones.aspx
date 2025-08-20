<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEvaluaciones.aspx.cs"
    Inherits="ProyectoGE.Pages.frmEvaluaciones" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Evaluaciones de Desempeño (API)</title></head>
<body>
<form id="form1" runat="server">
    <h2>Evaluaciones de Desempeño</h2>

    <asp:HiddenField ID="hfIdEval" runat="server" />
    Empleado:
    <asp:DropDownList ID="ddlEmpleado" runat="server" DataTextField="NombreCompleto" DataValueField="IdEmpleado" />
    &nbsp; Inicio (yyyy-MM-dd): <asp:TextBox ID="txtInicio" runat="server" />
    &nbsp; Fin (yyyy-MM-dd): <asp:TextBox ID="txtFin" runat="server" />
    &nbsp; Calificación (0..10): <asp:TextBox ID="txtCalif" runat="server" Width="60px" />
    <br />
    Comentarios: <asp:TextBox ID="txtComentarios" runat="server" Width="420px" />
    &nbsp; IdEvaluador (opcional): <asp:TextBox ID="txtEvaluador" runat="server" Width="80px" />

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
    <asp:GridView ID="gvEval" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdEvaluacion" OnSelectedIndexChanged="gvEval_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdEvaluacion" HeaderText="ID" />
            <asp:BoundField DataField="IdEmpleado" HeaderText="EmpleadoId" />
            <asp:BoundField DataField="PeriodoInicio" HeaderText="Inicio" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="PeriodoFin" HeaderText="Fin" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="Calificacion" HeaderText="Calificación" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="IdEvaluador" HeaderText="EvaluadorId" />
            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
