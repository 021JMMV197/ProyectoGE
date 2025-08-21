<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmEvaluaciones.aspx.cs"
    Inherits="ProyectoGE.Pages.frmEvaluaciones" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
  <title>Evaluaciones de Desempeño (API)</title>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />

  
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
  
  <link href="~/Styles/Estilo.css?v=3" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-4">

  <asp:HiddenField ID="hfIdEval" runat="server" />

  <div class="card form-card">
    <div class="card-header">
      <h4 class="m-0">Evaluaciones de Desempeño</h4>
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
          <label for="txtInicio" class="form-label">Inicio</label>
          <asp:TextBox ID="txtInicio" runat="server" CssClass="form-control" TextMode="Date" />
        </div>

        <div class="col-6 col-md-2">
          <label for="txtFin" class="form-label">Fin</label>
          <asp:TextBox ID="txtFin" runat="server" CssClass="form-control" TextMode="Date" />
        </div>

        <div class="col-12 col-md-2">
          <label for="txtCalif" class="form-label">Calificación (0–10)</label>
          <asp:TextBox ID="txtCalif" runat="server" CssClass="form-control" TextMode="Number" />
        </div>

        <div class="col-12 col-md-2">
          <label for="txtEvaluador" class="form-label">Id Evaluador (opcional)</label>
          <asp:TextBox ID="txtEvaluador" runat="server" CssClass="form-control" />
        </div>

        <div class="col-12">
          <label for="txtComentarios" class="form-label">Comentarios</label>
          <asp:TextBox ID="txtComentarios" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
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
        <asp:GridView ID="gvEval" runat="server" AutoGenerateColumns="False"
                      CssClass="table table-striped table-bordered table-sm"
                      DataKeyNames="IdEvaluacion"
                      OnSelectedIndexChanged="gvEval_SelectedIndexChanged"
                      GridLines="None">
          <Columns>
            <asp:CommandField ShowSelectButton="true" SelectText="Editar" />
            <asp:BoundField DataField="IdEvaluacion" HeaderText="ID" />
            <asp:BoundField DataField="IdEmpleado" HeaderText="EmpleadoId" />
            <asp:TemplateField HeaderText="Empleado">
              <ItemTemplate>
                <%# GetEmpleadoNombre(Eval("IdEmpleado")) %>
              </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="PeriodoInicio" HeaderText="Inicio" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="PeriodoFin" HeaderText="Fin" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="Calificacion" HeaderText="Calificación" DataFormatString="{0:N2}" />
            <asp:BoundField DataField="IdEvaluador" HeaderText="EvaluadorId" />
            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
          </Columns>
          <HeaderStyle CssClass="table-dark" />
        </asp:GridView>
      </div>
    </div>
  </div>

  <!-- (Opcional) Bootstrap JS -->
  <!-- <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script> -->
</form>
</body>
</html>
