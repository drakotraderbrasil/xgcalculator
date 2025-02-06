using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XGCalculator
{
    public class APIClient
    {
        private readonly HttpClient _httpClient;
        private string _biabCustomerToken;

        public APIClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "XGCalculator/1.0");
        }

        // Método para realizar login e armazenar o token
        public async Task<bool> Login(string username, string password)
        {
            var url = "https://bolsadeaposta-staging.com/client/api/auth/login"; // URL corrigida
            var requestData = $"{{\"login\":\"{username}\",\"password\":\"{password}\"}}";
            var content = new StringContent(requestData, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                // Obter o corpo da resposta
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var cookies = response.Headers.GetValues("Set-Cookie");
                    _biabCustomerToken = cookies.FirstOrDefault(c => c.StartsWith("BIAB_CUSTOMER="))
                        ?.Split('=')[1].Split(';')[0];

                    if (!string.IsNullOrEmpty(_biabCustomerToken))
                    {
                        MessageBox.Show("Login realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true;
                    }
                }
                else
                {
                    // Mostrar mensagem detalhada de erro
                    MessageBox.Show($"Falha ao realizar login. Detalhes: {responseBody}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar à API: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }
    }
}
