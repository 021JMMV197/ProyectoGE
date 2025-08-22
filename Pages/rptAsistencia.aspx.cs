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
    public partial class rptAsistencia : System.Web.UI.Page
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
                if (string.IsNullOrWhiteSpace(txtHoraTarde.Text)) txtHoraTarde.Text = "09:15";
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
                string horaTarde = string.IsNullOrWhiteSpace(txtHoraTarde.Text) ? "09:15" : txtHoraTarde.Text.Trim();

                var data = await _api.GetAsistenciaResumenAsync(desde, hasta, idEmp, horaTarde);
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
            txtDesde.Text = "";
            txtHasta.Text = "";
            txtHoraTarde.Text = "09:15";
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
                string horaTarde = string.IsNullOrWhiteSpace(txtHoraTarde.Text) ? "09:15" : txtHoraTarde.Text.Trim();

                var data = await _api.GetAsistenciaResumenAsync(desde, hasta, idEmp, horaTarde);
                if (data == null || data.Length == 0)
                {
                    lblMsg.Text = "No hay datos para exportar.";
                    return;
                }

                byte[] pdfBytes = BuildAsistenciaPdf_StandardFonts(
                    data,
                    desde, hasta,
                    idEmp.HasValue ? ddlEmpleado.SelectedItem.Text : "(Todos)",
                    horaTarde
                );

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=AsistenciaResumen.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(pdfBytes);
                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al exportar: " + ex.Message;
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
            if (DateTime.TryParseExact(s.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)) return d;
            if (DateTime.TryParse(s, out d)) return d;
            return null;
        }

        private static int? ParseIntNullable(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return int.TryParse(s, out var v) ? v : (int?)null;
        }

        private static byte[] BuildAsistenciaPdf_StandardFonts(
            AsistenciaResumenView[] data,
            DateTime? desde, DateTime? hasta,
            string empleadoFiltro,
            string horaTarde)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                var writer = new PdfWriter(ms, new WriterProperties());
                try { writer.SetSmartMode(false); } catch {  }

                var pdf = new PdfDocument(writer);
                pdf.SetDefaultPageSize(PageSize.A4);

                var doc = new Document(pdf);
                doc.SetMargins(36, 36, 36, 36);

                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                doc.Add(new Paragraph("Reporte: Asistencia (Resumen por Empleado)")
                    .SetFont(fontBold).SetFontSize(14).SetMarginBottom(8));

                var filtros = $"Empleado: {empleadoFiltro}   " +
                              $"Desde: {(desde.HasValue ? desde.Value.ToString("yyyy-MM-dd") : "(N/D)")}   " +
                              $"Hasta: {(hasta.HasValue ? hasta.Value.ToString("yyyy-MM-dd") : "(N/D)")}   " +
                              $"Hora tarde: {horaTarde}";
                doc.Add(new Paragraph(filtros).SetFont(font).SetFontSize(10).SetMarginBottom(10));

                float[] widths = { 12, 28, 12, 12, 12, 12, 12 };
                var table = new Table(UnitValue.CreatePercentArray(widths)).UseAllAvailableWidth();

                AddHeaderCell(table, "ID", fontBold);
                AddHeaderCell(table, "Empleado", fontBold);
                AddHeaderCell(table, "Reg.", fontBold);
                AddHeaderCell(table, "Tarde", fontBold);
                AddHeaderCell(table, "Sin Sal.", fontBold);
                AddHeaderCell(table, "Min. Trab.", fontBold);
                AddHeaderCell(table, "Prom./Reg.", fontBold);

                int tReg = 0, tTar = 0, tSin = 0, tMin = 0, tProm = 0;

                foreach (var r in data)
                {
                    AddCell(table, r.IdEmpleado.ToString(), font, TextAlignment.LEFT);
                    AddCell(table, r.Empleado ?? "", font, TextAlignment.LEFT);
                    AddCell(table, r.Registros.ToString(), font, TextAlignment.RIGHT);
                    AddCell(table, r.LlegadasTarde.ToString(), font, TextAlignment.RIGHT);
                    AddCell(table, r.SinSalida.ToString(), font, TextAlignment.RIGHT);
                    AddCell(table, r.MinutosTrabajados.ToString(), font, TextAlignment.RIGHT);
                    AddCell(table, r.PromedioMinutosPorRegistro.ToString(), font, TextAlignment.RIGHT);

                    tReg += r.Registros;
                    tTar += r.LlegadasTarde;
                    tSin += r.SinSalida;
                    tMin += r.MinutosTrabajados;
                    tProm += r.PromedioMinutosPorRegistro;
                }

                var tot = new Cell(1, 2)
                    .Add(new Paragraph("Totales").SetFont(fontBold).SetFontSize(9))
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetPadding(4)
                    .SetTextAlignment(TextAlignment.LEFT);
                table.AddCell(tot);
                AddCell(table, tReg.ToString(), fontBold, TextAlignment.RIGHT);
                AddCell(table, tTar.ToString(), fontBold, TextAlignment.RIGHT);
                AddCell(table, tSin.ToString(), fontBold, TextAlignment.RIGHT);
                AddCell(table, tMin.ToString(), fontBold, TextAlignment.RIGHT);
                AddCell(table, (data.Length > 0 ? (tProm / data.Length) : 0).ToString(), fontBold, TextAlignment.RIGHT);

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
