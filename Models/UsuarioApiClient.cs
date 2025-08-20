using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoGE.Models
{
    public class UsuarioView
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
    }

    public class UsuarioCreateDto
    {
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; }
        public string Adicionado_Por { get; set; }
    }

    public class UsuarioUpdateDto : UsuarioCreateDto
    {
        public int IdUsuario { get; set; }
        public string Modificado_Por { get; set; }
    }

    public class LoginRequest { public string NombreUsuario { get; set; } public string Contrasena { get; set; } }

    public class UsuarioApiClient
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        private HttpClient NewClient()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return http;
        }

        public async Task<PagedResult<UsuarioView>> ListAsync(int page = 1, int pageSize = 20, string buscar = null)
        {
            using (var http = NewClient())
            {
                var url = $"{_baseUrl}api/usuarios?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrWhiteSpace(buscar)) url += $"&buscar={System.Uri.EscapeDataString(buscar)}";
                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PagedResult<UsuarioView>>(json);
            }
        }

        public async Task<int> CreateAsync(UsuarioCreateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PostAsync($"{_baseUrl}api/usuarios",
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                resp.EnsureSuccessStatusCode();
                dynamic o = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());
                return (int)o.id;
            }
        }

        public async Task<bool> UpdateAsync(UsuarioUpdateDto d)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PutAsync($"{_baseUrl}api/usuarios/{d.IdUsuario}",
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
                var resp = await http.DeleteAsync($"{_baseUrl}api/usuarios/{id}");
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return false;
                resp.EnsureSuccessStatusCode();
                return true;
            }
        }

        public async Task<UsuarioView> LoginAsync(LoginRequest req)
        {
            using (var http = NewClient())
            {
                var payload = JsonConvert.SerializeObject(req);
                var resp = await http.PostAsync($"{_baseUrl}api/auth/login",
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) return null;
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioView>(json);
            }
        }
    }
}
