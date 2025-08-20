using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoGE.Models
{
    public class EvaluacionView
    {
        public int IdEvaluacion { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFin { get; set; }
        public decimal Calificacion { get; set; }
        public string Comentarios { get; set; }
        public int? IdEvaluador { get; set; }
    }

    public class EvaluacionCreateDto
    {
        public int IdEmpleado { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFin { get; set; }
        public decimal Calificacion { get; set; }
        public string Comentarios { get; set; }
        public int? IdEvaluador { get; set; }
        public string Adicionado_Por { get; set; }
    }

    public class EvaluacionUpdateDto : EvaluacionCreateDto
    {
        public int IdEvaluacion { get; set; }
        public string Modificado_Por { get; set; }
    }

    public class EvaluacionApiClient
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        private HttpClient NewClient()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return http;
        }

        public async Task<EvaluacionView[]> ListAsync(int? idEmpleado = null, DateTime? desde = null, DateTime? hasta = null)
        {
            using (var http = NewClient())
            {
                var url = $"{_baseUrl}api/evaluaciones";
                var sep = "?";
                if (idEmpleado.HasValue) { url += $"{sep}idEmpleado={idEmpleado.Value}"; sep = "&"; }
                if (desde.HasValue) { url += $"{sep}desde={desde.Value:yyyy-MM-dd}"; sep = "&"; }
                if (hasta.HasValue) { url += $"{sep}hasta={hasta.Value:yyyy-MM-dd}"; }
                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EvaluacionView[]>(json);
            }
        }

        public async Task<int> CreateAsync(EvaluacionCreateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PostAsync($"{_baseUrl}api/evaluaciones",
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                resp.EnsureSuccessStatusCode();
                dynamic o = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());
                return (int)o.id;
            }
        }

        public async Task<bool> UpdateAsync(EvaluacionUpdateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PutAsync($"{_baseUrl}api/evaluaciones/{d.IdEvaluacion}",
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return false;
                resp.EnsureSuccessStatusCode();
                return true;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var http = NewClient())
            {
                var resp = await http.DeleteAsync($"{_baseUrl}api/evaluaciones/{id}");
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return false;
                resp.EnsureSuccessStatusCode();
                return true;
            }
        }
    }
}
