<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="frmAsistencia.aspx.cs"
    Inherits="ProyectoGE.Pages.frmAsistencia"
    Async="true"
    ValidateRequest="false" %>

<!DOCTYPE html>
<html>
<head runat="server">
  <title>Asistencia (API)</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />

  <!-- Bootstrap + tu tema -->
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  <link href="~/Styles/Estilo.css?v=2" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-4">

  <asp:HiddenField ID="hfIdAsistencia" runat="server" />

  <div class="card form-card">
    <div class="card-header">
      <h4 class="m-0">Asistencia</h4>
    </div>

    <div class="card-body">
      <!-- Formulario -->
      <div class="row g-3">
        <div class="col-12 col-md-4">
          <label for="ddlEmpleado" class="form-label">Empleado</label>
          <asp:DropDownList ID="ddlEmpleado" runat="server"
                            DataTextField="NombreCompleto" DataValueField="IdEmpleado"
                            CssClass="form-select" />
        </div>

        <div class="col-6 col-md-2">
          <label for="txtFecha" class="form-label">Fecha</label>
          <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
        </div>

        <div class="col-6 col-md-2">
          <label for="txtEntrada" class="form-label">Entrada</label>
          <asp:TextBox ID="txtEntrada" runat="server" CssClass="form-control" TextMode="Time" />
        </div>

        <div class="col-6 col-md-2">
          <label for="txtSalida" class="form-label">Salida</label>
          <asp:TextBox ID="txtSalida" runat="server" CssClass="form-control" TextMode="Time" />
        </div>

        <div class="col-12">
          <label for="txtObs" class="form-label">Observación</label>
          <asp:TextBox ID="txtObs" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
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
        <asp:GridView ID="gvAsis" runat="server" AutoGenerateColumns="False"
                      CssClass="table table-striped table-bordered table-sm"
                      DataKeyNames="IdAsistencia,Observacion"
                      OnSelectedIndexChanged="gvAsis_SelectedIndexChanged"
                      GridLines="None">
          <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdAsistencia" HeaderText="ID" />
            <asp:BoundField DataField="IdEmpleado" HeaderText="EmpleadoId" />
            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="HoraEntrada" HeaderText="Entrada" DataFormatString="{0:HH:mm}" />
            <asp:BoundField DataField="HoraSalida" HeaderText="Salida" DataFormatString="{0:HH:mm}" />
            <asp:BoundField DataField="Observacion" HeaderText="Observación" HtmlEncode="false" />
          </Columns>
          <HeaderStyle CssClass="table-dark" />
        </asp:GridView>
      </div>
    </div>
  </div>

</form>
</body>
</html>
