using ProyectoGE.Infrastructure;
using ProyectoGE.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using iText.Kernel.Pdf;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace ProyectoGE.Pages
{
    public partial class rptVacaciones : System.Web.UI.Page
    {
        private readonly ReportesApiClient _api = new ReportesApiClient();
        private readonly EmpleadoApiClient _empApi = new EmpleadoApiClient();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (AuthGuard.Require(this)) return;
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarEmpleadosAsync();
                txtDesde.Text = new DateTime(DateTime.Today.Year, 1, 1).ToString("yyyy-MM-dd");
                txtHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        protected async void btnGenerar_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            try
            {
                int? idEmp = ParseIntNullable(ddlEmpleado.SelectedValue);
                DateTime? desde = ParseFecha(txtDesde.Text);
                DateTime? hasta = ParseFecha(txtHasta.Text);
                string estado = ddlEstado.SelectedValue;

                var data = await _api.GetVacacionesResumenAsync(desde, hasta, idEmp, estado);
                gvResumen.DataSource = data;
                gvResumen.DataBind();

                if (data == null || data.Length == 0)
                    lblMsg.Text = "Sin datos para el filtro.";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al generar: " + ex.Message;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ddlEmpleado.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            txtDesde.Text = "";
            txtHasta.Text = "";
            gvResumen.DataSource = null;
            gvResumen.DataBind();
            lblMsg.Text = "";
        }

        protected async void btnPdf_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            try
            {
                int? idEmp = ParseIntNullable(ddlEmpleado.SelectedValue);
                DateTime? desde = ParseFecha(txtDesde.Text);
                DateTime? hasta = ParseFecha(txtHasta.Text);
                string estado = ddlEstado.SelectedValue;

                var data = await _api.GetVacacionesResumenAsync(desde, hasta, idEmp, estado);
                if (data == null || data.Length == 0)
                {
                    lblMsg.Text = "No hay datos para exportar.";
                    return;
                }

                byte[] pdfBytes = BuildVacacionesPdf_StandardFonts(
                    data,
                    desde, hasta,
                    idEmp.HasValue ? ddlEmpleado.SelectedItem.Text : "(Todos)",
                    string.IsNullOrWhiteSpace(estado) ? "(Todos)" : estado
                );

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=VacacionesResumen.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (iText.Kernel.Exceptions.PdfException pex)
            {
                lblMsg.Text = "iText PdfException:<br/><pre>" + Server.HtmlEncode(pex.ToString()) + "</pre>";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al exportar:<br/><pre>" + Server.HtmlEncode(ex.ToString()) + "</pre>";
            }
        }






        // ================== Helpers ==================

        private async Task CargarEmpleadosAsync()
        {
            try
            {
                var r = await _empApi.ListAsync(1, 1000);
                var lista = r.Items
                    .Select(x => new { x.IdEmpleado, Nombre = (x.Nombre ?? "") + " " + (x.Apellido ?? "") })
                    .OrderBy(x => x.Nombre)
                    .ToList();

                ddlEmpleado.Items.Clear();
                ddlEmpleado.Items.Add(new System.Web.UI.WebControls.ListItem("(Todos)", ""));
                foreach (var it in lista)
                    ddlEmpleado.Items.Add(new System.Web.UI.WebControls.ListItem(it.Nombre, it.IdEmpleado.ToString()));
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al cargar empleados: " + ex.Message;
            }
        }

        private static DateTime? ParseFecha(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var d)) return d;
            if (DateTime.TryParse(s, out d)) return d;
            return null;
        }

        private static int? ParseIntNullable(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return int.TryParse(s, out var v) ? v : (int?)null;
        }


        private byte[] BuildVacacionesPdf_StandardFonts(
            VacacionesResumenView[] data,
            DateTime? desde, DateTime? hasta,
            string empleadoFiltro,
            string estadoFiltro)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                var writer = new PdfWriter(ms);
                
                writer.SetSmartMode(false);

                var pdf = new PdfDocument(writer);
                pdf.SetDefaultPageSize(PageSize.A4);

                var doc = new Document(pdf);
                doc.SetMargins(36, 36, 36, 36);

                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                doc.Add(new Paragraph("Reporte: Vacaciones (Resumen por Empleado)")
                    .SetFont(fontBold).SetFontSize(14).SetMarginBottom(8));

                var filtros = $"Empleado: {empleadoFiltro}   Estado: {estadoFiltro}   " +
                              $"Desde: {(desde.HasValue ? desde.Value.ToString("yyyy-MM-dd") : "(N/D)")}   " +
                              $"Hasta: {(hasta.HasValue ? hasta.Value.ToString("yyyy-MM-dd") : "(N/D)")}";
                doc.Add(new Paragraph(filtros).SetFont(font).SetFontSize(10).SetMarginBottom(10));

                float[] widths = { 12, 30, 12, 12, 12, 12, 12 };
                var table = new Table(UnitValue.CreatePercentArray(widths)).UseAllAvailableWidth();

                AddHeaderCell(table, "Empleado ID", fontBold);
                AddHeaderCell(table, "Empleado", fontBold);
                AddHeaderCell(table, "Solic.", fontBold);
                AddHeaderCell(table, "Total", fontBold);
                AddHeaderCell(table, "Aprob.", fontBold);
                AddHeaderCell(table, "Pend.", fontBold);
                AddHeaderCell(table, "Rech.", fontBold);

                int tSolic = 0, tTotal = 0, tApr = 0, tPen = 0, tRec = 0;

                foreach (var r in data)
                {
                    AddCell(table, r.IdEmpleado.ToString(), font, TextAlignment.LEFT);
                    AddCell(table, r.Empleado ?? "", font, TextAlignment.LEFT);
                    AddCell(table, r.Solicitudes.ToString(), font, TextAlignment.RIGHT);
                    AddCell(table, r.TotalDias.ToString(), font, TextAlignment.RIGHT);
                    AddCell(table, r.DiasAprobados.ToString(), font, TextAlignment.RIGHT);
                    AddCell(table, r.DiasPendientes.ToString(), font, TextAlignment.RIGHT);
                    AddCell(table, r.DiasRechazados.ToString(), font, TextAlignment.RIGHT);

                    tSolic += r.Solicitudes;
                    tTotal += r.TotalDias;
                    tApr += r.DiasAprobados;
                    tPen += r.DiasPendientes;
                    tRec += r.DiasRechazados;
                }

                
                var tot = new Cell(1, 2)
                    .Add(new Paragraph("Totales").SetFont(fontBold).SetFontSize(9))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetPadding(4)
                    .SetTextAlignment(TextAlignment.LEFT);
                table.AddCell(tot);

                AddCell(table, tSolic.ToString(), fontBold, TextAlignment.RIGHT);
                AddCell(table, tTotal.ToString(), fontBold, TextAlignment.RIGHT);
                AddCell(table, tApr.ToString(), fontBold, TextAlignment.RIGHT);
                AddCell(table, tPen.ToString(), fontBold, TextAlignment.RIGHT);
                AddCell(table, tRec.ToString(), fontBold, TextAlignment.RIGHT);

                doc.Add(table);
                doc.Close();

                return ms.ToArray();
            }
        }

        private static void AddHeaderCell(Table t, string text, PdfFont f)
        {
            t.AddCell(new Cell()
                .Add(new Paragraph(text).SetFont(f).SetFontSize(9))
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetPadding(5)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE));
        }

        private static void AddCell(Table t, string text, PdfFont f, TextAlignment align)
        {
            t.AddCell(new Cell()
                .Add(new Paragraph(text).SetFont(f).SetFontSize(9))
                .SetPadding(4)
                .SetTextAlignment(align)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE));
        }
        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Menu.aspx");
        }

    }
}
