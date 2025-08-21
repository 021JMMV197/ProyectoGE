using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProyectoGE.Models
{
    

    public class EmpleadoView
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public decimal? Salario { get; set; }
        public int IdDepartamento { get; set; }
    }

    public class EmpleadoCreateDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public decimal? Salario { get; set; }
        public int IdDepartamento { get; set; }
        public string Adicionado_Por { get; set; }
    }

    public class EmpleadoUpdateDto : EmpleadoCreateDto
    {
        public int IdEmpleado { get; set; }
        public string Modificado_Por { get; set; }
    }

    public class EmpleadoApiClient
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];

        public async Task<PagedResult<EmpleadoView>> ListAsync(int page = 1, int pageSize = 20, string buscar = null, int? idDepartamento = null)
        {
            using (var http = new HttpClient())
            {
                var url = $"{_baseUrl}api/empleados?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrWhiteSpace(buscar)) url += $"&buscar={Uri.EscapeDataString(buscar)}";
                if (idDepartamento.HasValue) url += $"&idDepartamento={idDepartamento.Value}";

                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PagedResult<EmpleadoView>>(json);
            }
        }

        public async Task<int> CreateAsync(EmpleadoCreateDto d)
        {
            using (var http = new HttpClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PostAsync($"{_baseUrl}api/empleados", new StringContent(payload, Encoding.UTF8, "application/json"));
                resp.EnsureSuccessStatusCode();
                dynamic o = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync());
                return (int)o.id;
            }
        }

        public async Task<bool> UpdateAsync(EmpleadoUpdateDto d)
        {
            using (var http = new HttpClient())
            {
                var payload = JsonConvert.SerializeObject(d);
                var resp = await http.PutAsync($"{_baseUrl}api/empleados/{d.IdEmpleado}", new StringContent(payload, Encoding.UTF8, "application/json"));
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return false;
                resp.EnsureSuccessStatusCode();
                return true;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var http = new HttpClient())
            {
                var resp = await http.DeleteAsync($"{_baseUrl}api/empleados/{id}");
                if (resp.StatusCode == System.Net.HttpStatusCode.NoContent) return true;
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return false;
                resp.EnsureSuccessStatusCode();
                return true;
            }
        }
    }
}
