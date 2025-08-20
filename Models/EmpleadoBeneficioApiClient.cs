using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoGE.Models
{
    public class EmpleadoBeneficioView
    {
        public int IdEmpleadoBeneficio { get; set; }
        public int IdEmpleado { get; set; }
        public int IdBeneficio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
    }

    public class EmpleadoBeneficioCreateDto
    {
        public int IdEmpleado { get; set; }
        public int IdBeneficio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public string Adicionado_Por { get; set; }
    }

    public class EmpleadoBeneficioUpdateDto : EmpleadoBeneficioCreateDto
    {
        public int IdEmpleadoBeneficio { get; set; }
        public string Modificado_Por { get; set; }
    }

    public class EmpleadoBeneficioApiClient
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        private HttpClient NewClient()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return http;
        }

        public async Task<EmpleadoBeneficioView[]> ListAsync(int? idEmpleado = null, int? idBeneficio = null, string estado = null)
        {
            using (var http = NewClient())
            {
                var url = $"{_baseUrl}api/empleadobeneficios";
                var sep = "?";
                if (idEmpleado.HasValue) { url += $"{sep}idEmpleado={idEmpleado.Value}"; sep = "&"; }
                if (idBeneficio.HasValue) { url += $"{sep}idBeneficio={idBeneficio.Value}"; sep = "&"; }
                if (!string.IsNullOrWhiteSpace(estado)) { url += $"{sep}estado={Uri.EscapeDataString(estado)}"; }

                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmpleadoBeneficioView[]>(json);
            }
        }

        public async Task<int> CreateAsync(EmpleadoBeneficioCreateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PostAsync($"{_baseUrl}api/empleadobeneficios",
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                resp.EnsureSuccessStatusCode();
                dynamic o = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());
                return (int)o.id;
            }
        }

        public async Task<bool> UpdateAsync(EmpleadoBeneficioUpdateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PutAsync($"{_baseUrl}api/empleadobeneficios/{d.IdEmpleadoBeneficio}",
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
                var resp = await http.DeleteAsync($"{_baseUrl}api/empleadobeneficios/{id}");
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return false;
                resp.EnsureSuccessStatusCode();
                return true;
            }
        }
    }
}
