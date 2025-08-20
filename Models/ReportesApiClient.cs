using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProyectoGE.Models
{
    // =======================
    // DTOs (para deserializar)
    // =======================
    public class VacacionesResumenView
    {
        public int IdEmpleado { get; set; }
        public string Empleado { get; set; }
        public int Solicitudes { get; set; }
        public int TotalDias { get; set; }
        public int DiasAprobados { get; set; }
        public int DiasPendientes { get; set; }
        public int DiasRechazados { get; set; }
    }

    public class AsistenciaResumenView
    {
        public int IdEmpleado { get; set; }
        public string Empleado { get; set; }
        public int Registros { get; set; }
        public int LlegadasTarde { get; set; }
        public int SinSalida { get; set; }
        public int MinutosTrabajados { get; set; }
        public int PromedioMinutosPorRegistro { get; set; }
    }

    // =======================
    // Cliente de Reportes API
    // =======================
    public class ReportesApiClient
    {
        // Normaliza con una barra al final
        private readonly string _baseUrl =
            ((ConfigurationManager.AppSettings["ApiBaseUrl"] ?? string.Empty).TrimEnd('/')) + "/";

        private HttpClient NewClient()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return http;
        }

        private static string Append(string url, string name, string value, ref string sep)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                url += $"{sep}{name}={Uri.EscapeDataString(value)}";
                sep = "&";
            }
            return url;
        }

        // -------- Vacaciones (resumen) --------
        public async Task<VacacionesResumenView[]> GetVacacionesResumenAsync(
            DateTime? desde, DateTime? hasta, int? idEmpleado, string estado)
        {
            using (var http = NewClient())
            {
                var url = _baseUrl + "api/reportes/vacaciones-resumen";
                var sep = "?";

                if (desde.HasValue) url = Append(url, "desde", desde.Value.ToString("yyyy-MM-dd"), ref sep);
                if (hasta.HasValue) url = Append(url, "hasta", hasta.Value.ToString("yyyy-MM-dd"), ref sep);
                if (idEmpleado.HasValue) url = Append(url, "idEmpleado", idEmpleado.Value.ToString(), ref sep);
                url = Append(url, "estado", estado, ref sep);

                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<VacacionesResumenView[]>(json);
            }
        }

        // -------- Asistencia (resumen) --------
        public async Task<AsistenciaResumenView[]> GetAsistenciaResumenAsync(
            DateTime? desde, DateTime? hasta, int? idEmpleado, string horaTarde = "09:15")
        {
            using (var http = NewClient())
            {
                var url = _baseUrl + "api/reportes/asistencia-resumen";
                var sep = "?";

                if (desde.HasValue) url = Append(url, "desde", desde.Value.ToString("yyyy-MM-dd"), ref sep);
                if (hasta.HasValue) url = Append(url, "hasta", hasta.Value.ToString("yyyy-MM-dd"), ref sep);
                if (idEmpleado.HasValue) url = Append(url, "idEmpleado", idEmpleado.Value.ToString(), ref sep);
                url = Append(url, "horaTarde", string.IsNullOrWhiteSpace(horaTarde) ? "09:15" : horaTarde, ref sep);

                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AsistenciaResumenView[]>(json);
            }
        }
    }
}
