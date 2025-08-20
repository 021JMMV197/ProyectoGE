using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProyectoGE.Models
{
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

    public class ReportesApiClient
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        private HttpClient NewClient()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return http;
        }

        public async Task<VacacionesResumenView[]> GetVacacionesResumenAsync(DateTime? desde, DateTime? hasta, int? idEmpleado, string estado)
        {
            using (var http = NewClient())
            {
                var url = _baseUrl + "api/reportes/vacaciones-resumen";
                var sep = "?";
                if (desde.HasValue) { url += $"{sep}desde={desde:yyyy-MM-dd}"; sep = "&"; }
                if (hasta.HasValue) { url += $"{sep}hasta={hasta:yyyy-MM-dd}"; sep = "&"; }
                if (idEmpleado.HasValue) { url += $"{sep}idEmpleado={idEmpleado.Value}"; sep = "&"; }
                if (!string.IsNullOrWhiteSpace(estado)) { url += $"{sep}estado={Uri.EscapeDataString(estado)}"; }

                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<VacacionesResumenView[]>(json);
            }
        }
    }
}
