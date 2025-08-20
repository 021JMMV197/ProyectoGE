using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoGE.Models
{
    public class DepartamentoView
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class DepartamentoCreateDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Adicionado_Por { get; set; }
    }

    public class DepartamentoUpdateDto : DepartamentoCreateDto
    {
        public int IdDepartamento { get; set; }
        public string Modificado_Por { get; set; }
    }

    public class DepartamentoApiClient
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        private HttpClient NewClient()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return http;
        }

        public async Task<PagedResult<DepartamentoView>> ListAsync(int page = 1, int pageSize = 20, string buscar = null)
        {
            using (var http = NewClient())
            {
                var url = $"{_baseUrl}api/departamentos?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrWhiteSpace(buscar)) url += $"&buscar={Uri.EscapeDataString(buscar)}";
                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PagedResult<DepartamentoView>>(json);
            }
        }

        public async Task<int> CreateAsync(DepartamentoCreateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PostAsync($"{_baseUrl}api/departamentos",
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                resp.EnsureSuccessStatusCode();
                dynamic o = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());
                return (int)o.id;
            }
        }

        public async Task<bool> UpdateAsync(DepartamentoUpdateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PutAsync($"{_baseUrl}api/departamentos/{d.IdDepartamento}",
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
                var resp = await http.DeleteAsync($"{_baseUrl}api/departamentos/{id}");
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return false;
                resp.EnsureSuccessStatusCode();
                return true;
            }
        }
    }
}
