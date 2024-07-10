using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingLourd.Class
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public class ApiUser
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://127.0.0.1:8000/api/users/";

        public ApiUser()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> IsValidUserAsync(string username, string password)
        {
            try
            {
                // Construire l'URL de l'API pour vérifier l'utilisateur
                var url = $"{BaseUrl}?username={username}&password={password}";

                // Effectuer une requête HTTP GET pour vérifier l'utilisateur
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Lire le contenu de la réponse
                var content = await response.Content.ReadAsStringAsync();

                // Si la réponse contient "true", cela signifie que l'utilisateur est valide
                return content.ToLower().Contains("true");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la vérification de l'utilisateur : " + ex.Message);
                return false;
            }
        }
    }

}
