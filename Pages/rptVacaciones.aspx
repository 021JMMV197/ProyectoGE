<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptVacaciones.aspx.cs"
    Inherits="ProyectoGE.Pages.rptVacaciones" Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Reporte: Vacaciones</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Tu tema (rosa claro/medio/oscuro) -->
    <link href="~/Styles/Estilo.css?v=2" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-4">

    <div class="card form-card">
        <div class="card-header">
            <h4 class="m-0">Reporte de Vacaciones (Resumen por Empleado)</h4>
        </div>

        <div class="card-body">
            <!-- Filtros -->
            <div class="row g-3">
                <div class="col-12 col-md-4">
                    <label for="ddlEmpleado" class="form-label">Empleado</label>
                    <asp:DropDownList ID="ddlEmpleado" runat="server" CssClass="form-select" />
                </div>

                <div class="col-6 col-md-2">
                    <label for="txtDesde" class="form-label">Desde</label>
                    <asp:TextBox ID="txtDesde" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="col-6 col-md-2">
                    <label for="txtHasta" class="form-label">Hasta</label>
                    <asp:TextBox ID="txtHasta" runat="server" CssClass="form-control" TextMode="Date" />
                </div>

                <div class="col-12 col-md-4">
                    <label for="ddlEstado" class="form-label">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                        <asp:ListItem Text="(Todos)" Value="" />
                        <asp:ListItem Text="Pendiente" Value="Pendiente" />
                        <asp:ListItem Text="Aprobado" Value="Aprobado" />
                        <asp:ListItem Text="Rechazado" Value="Rechazado" />
                    </asp:DropDownList>
                </div>
            </div>

            <!-- Acciones -->
            <div class="d-flex flex-wrap gap-2 mt-3">
                <asp:Button ID="btnGenerar" runat="server" Text="Generar"
                            CssClass="btn btn-pink" OnClick="btnGenerar_Click" />
                <asp:Button ID="btnPdf" runat="server" Text="Exportar PDF"
                            CssClass="btn btn-pink" OnClick="btnPdf_Click" />
                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar"
                            CssClass="btn btn-outline-light" OnClick="btnLimpiar_Click" />
                <asp:Button ID="btnAtras" runat="server" Text="Atrás"
                            CssClass="btn btn-outline-light" OnClick="btnAtras_Click"
                            CausesValidation="false" />
            </div>

            <asp:Label ID="lblMsg" runat="server" CssClass="d-block mt-3 text-light" />

            <!-- Resultados -->
            <div class="table-responsive mt-3">
                <asp:GridView ID="gvResumen" runat="server" AutoGenerateColumns="False"
                              CssClass="table table-striped table-bordered table-sm" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="IdEmpleado" HeaderText="Empleado ID" />
                        <asp:BoundField DataField="Empleado" HeaderText="Empleado" />
                        <asp:BoundField DataField="Solicitudes" HeaderText="Solicitudes" />
                        <asp:BoundField DataField="TotalDias" HeaderText="Total Días" />
                        <asp:BoundField DataField="DiasAprobados" HeaderText="Aprobados" />
                        <asp:BoundField DataField="DiasPendientes" HeaderText="Pendientes" />
                        <asp:BoundField DataField="DiasRechazados" HeaderText="Rechazados" />
                    </Columns>
                    <HeaderStyle CssClass="table-dark" />
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- (Opcional) Bootstrap JS si usas componentes dinámicos -->
    <!-- <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script> -->
</form>
</body>
</html>
