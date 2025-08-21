<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="rptAsistencia.aspx.cs"
    Inherits="ProyectoGE.Pages.rptAsistencia"
    Async="true" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Reporte – Asistencia (Resumen)</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    
    <link href="~/Styles/Estilo.css?v=2" rel="stylesheet" runat="server" />
</head>
<body>
<form id="form1" runat="server" class="container py-4">

    <div class="card form-card">
        <div class="card-header">
            <h4 class="m-0">Reporte – Asistencia (Resumen)</h4>
        </div>

        <div class="card-body">
            <!-- Filtros -->
            <div class="row g-3 align-items-end">
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

                <div class="col-6 col-md-2">
                    <label for="txtHoraTarde" class="form-label">Hora tarde</label>
                    <asp:TextBox ID="txtHoraTarde" runat="server" CssClass="form-control" TextMode="Time" />
                </div>

                <div class="col-12 col-md-12 d-flex flex-wrap gap-2 mt-2">
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
            </div>

            <asp:Label ID="lblMsg" runat="server" CssClass="d-block mt-3 text-light" />

            <!-- Resultado -->
            <div class="table-responsive mt-3">
                <asp:GridView ID="gvResumen" runat="server" AutoGenerateColumns="False"
                              CssClass="table table-striped table-bordered table-sm"
                              GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="IdEmpleado" HeaderText="ID" />
                        <asp:BoundField DataField="Empleado" HeaderText="Empleado" />
                        <asp:BoundField DataField="Registros" HeaderText="Registros" />
                        <asp:BoundField DataField="LlegadasTarde" HeaderText="Tarde" />
                        <asp:BoundField DataField="SinSalida" HeaderText="Sin Salida" />
                        <asp:BoundField DataField="MinutosTrabajados" HeaderText="Min. Trabajados" />
                        <asp:BoundField DataField="PromedioMinutosPorRegistro" HeaderText="Promedio/Reg." />
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
