using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoGE.Models
{
    public class PuestoView
    {
        public int IdPuesto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal? SalarioBase { get; set; }
        public int IdDepartamento { get; set; }
    }

    public class PuestoCreateDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal? SalarioBase { get; set; }
        public int IdDepartamento { get; set; }
        public string Adicionado_Por { get; set; }
    }

    public class PuestoUpdateDto : PuestoCreateDto
    {
        public int IdPuesto { get; set; }
        public string Modificado_Por { get; set; }
    }

    public class PuestoApiClient
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        private HttpClient NewClient()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return http;
        }

        public async Task<PagedResult<PuestoView>> ListAsync(int page = 1, int pageSize = 20, string buscar = null, int? idDepartamento = null)
        {
            using (var http = NewClient())
            {
                var url = $"{_baseUrl}api/puestos?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrWhiteSpace(buscar)) url += $"&buscar={Uri.EscapeDataString(buscar)}";
                if (idDepartamento.HasValue) url += $"&idDepartamento={idDepartamento.Value}";
                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PagedResult<PuestoView>>(json);
            }
        }

        public async Task<int> CreateAsync(PuestoCreateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PostAsync($"{_baseUrl}api/puestos",
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                resp.EnsureSuccessStatusCode();
                dynamic o = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());
                return (int)o.id;
            }
        }

        public async Task<bool> UpdateAsync(PuestoUpdateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PutAsync($"{_baseUrl}api/puestos/{d.IdPuesto}",
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
                var resp = await http.DeleteAsync($"{_baseUrl}api/puestos/{id}");
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return false;
                resp.EnsureSuccessStatusCode();
                return true;
            }
        }
    }
}
