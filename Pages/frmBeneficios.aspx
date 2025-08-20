<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmBeneficios.aspx.cs"
    Inherits="ProyectoGE.Pages.frmBeneficios" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server"><title>Beneficios (API)</title></head>
<body>
<form id="form1" runat="server">
    <h2>Beneficios</h2>

    <asp:HiddenField ID="hfIdBeneficio" runat="server" />
    Nombre: <asp:TextBox ID="txtNombre" runat="server" />
    &nbsp; Tipo: <asp:TextBox ID="txtTipo" runat="server" />
    &nbsp; Monto Mensual: <asp:TextBox ID="txtMonto" runat="server" Width="90px" />
    <br />
    Descripción: <asp:TextBox ID="txtDescripcion" runat="server" Width="420px" />

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
    <asp:GridView ID="gvBenef" runat="server" AutoGenerateColumns="False"
        DataKeyNames="IdBeneficio" OnSelectedIndexChanged="gvBenef_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdBeneficio" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
            <asp:BoundField DataField="MontoMensual" HeaderText="Monto" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
