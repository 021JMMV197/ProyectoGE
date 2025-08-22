<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmBeneficios.aspx.cs"
    Inherits="ProyectoGE.Pages.frmBeneficios" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <title>Beneficios (API)</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  
  <link href="~/Styles/Estilo.css?v=2" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-4">

  <asp:HiddenField ID="hfIdBeneficio" runat="server" />

  <div class="card form-card">
    <div class="card-header">
      <h4 class="m-0">Beneficios</h4>
    </div>

    <div class="card-body">
      <!-- Formulario -->
      <div class="row g-3">
        <div class="col-12 col-md-4">
          <label for="txtNombre" class="form-label">Nombre</label>
          <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
        </div>

        <div class="col-12 col-md-4">
          <label for="txtTipo" class="form-label">Tipo</label>
          <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control" />
        </div>

        <div class="col-12 col-md-4">
          <label for="txtMonto" class="form-label">Monto mensual</label>
          <asp:TextBox ID="txtMonto" runat="server" CssClass="form-control" TextMode="Number" />
        </div>

        <div class="col-12">
          <label for="txtDescripcion" class="form-label">Descripción</label>
          <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
        </div>
      </div>

      <!-- Acciones -->
      <div class="d-flex flex-wrap gap-2 mt-3">
        <asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="btn btn-pink" OnClick="btnCrear_Click" />
        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CssClass="btn btn-pink" OnClick="btnActualizar_Click" />
        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-outline-light" OnClick="btnEliminar_Click" />
        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-light" OnClick="btnLimpiar_Click" />
        <asp:Button ID="btnCargar" runat="server" Text="Cargar" CssClass="btn btn-outline-light" OnClick="btnCargar_Click" />
        <asp:Button ID="btnAtras" runat="server" Text="Atrás" CssClass="btn btn-outline-light"
                    OnClick="btnAtras_Click" CausesValidation="false" />
      </div>

      <div class="mt-3">
        <asp:Label ID="lblMsg" runat="server" CssClass="text-light d-block" />
        <asp:Label ID="lblTotal" runat="server" CssClass="text-light d-block" Text="Total: 0" />
      </div>

      <!-- Grilla -->
      <div class="table-responsive mt-3">
        <asp:GridView ID="gvBenef" runat="server" AutoGenerateColumns="False"
                      CssClass="table table-striped table-bordered table-sm"
                      DataKeyNames="IdBeneficio"
                      OnSelectedIndexChanged="gvBenef_SelectedIndexChanged"
                      GridLines="None">
          <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdBeneficio" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
            <asp:BoundField DataField="MontoMensual" HeaderText="Monto" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
          </Columns>
          <HeaderStyle CssClass="table-dark" />
        </asp:GridView>
      </div>
    </div>
  </div>

  
</form>
</body>
</html>
