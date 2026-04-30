using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.ObjectModel;

namespace ControleBiblioteca.Wpf.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        // Lembre-se de verificar se a porta 7118 é a correta da sua API
        private const string BaseUrl = "https://localhost:7118/api/Emprestimos";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<EmprestimoModel>?> ObterEmprestimosAsync()
        {
            return await _httpClient.GetFromJsonAsync<ObservableCollection<EmprestimoModel>>(BaseUrl);
        }

        public async Task<bool> RegistrarEmprestimoAsync(int livroId, int usuarioId)
        {
            var payload = new { LivroId = livroId, UsuarioId = usuarioId };
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DevolverLivroAsync(int emprestimoId)
        {
            var response = await _httpClient.PutAsync($"{BaseUrl}/{emprestimoId}/devolver", null);
            return response.IsSuccessStatusCode;
        }
    }

    // A Model espelha o DTO de resposta da nossa API
    public class EmprestimoModel
    {
        public int Id { get; set; }
        public string TituloLivro { get; set; } = string.Empty;
        public string NomeUsuario { get; set; } = string.Empty;
        public DateTime DataRetirada { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }
        public decimal MultaAtraso { get; set; }
        public string StatusEmprestimo { get; set; } = string.Empty;
    }
}