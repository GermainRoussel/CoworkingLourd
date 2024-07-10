using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CoworkingLourd.Class
{
    public class ApiCompany
    {

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://127.0.0.1:8000/api/company/";
        private const string BaseUrlUpdate = "http://127.0.0.1:8000/api/company/update";
        private const string BaseUrlDelete = "http://127.0.0.1:8000/api/company/delete";
        private const string BaseUrlAdd = "http://127.0.0.1:8000/api/company/create/";



        public ApiCompany()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<CompanyDto>> GetCompaniesAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CompanyDto>>(content);
        }
        public async Task<List<CompanyDto>> GetCompaniesAsync(string searchTerm)
        {
            // Construisez votre URL en fonction du terme de recherche
            var url = BaseUrl + "?search=" + searchTerm;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CompanyDto>>(content);
        }



        public async Task<CompanyDto> UpdateCompanyAsync(int id, string namecomp, string siretcomp, string emailcomp, string tvanumbercomp, string companyCreatedBycomp, string companyUpdatedBycomp)
        {
            // Construisez votre URL en fonction de l'ID de l'entreprise
            var url = $"{BaseUrlUpdate}/{id}";

            // Créez un objet contenant les données à mettre à jour
            var data = new
            {
                siret = siretcomp,
                name = namecomp,
                email = emailcomp,
                tvaNumber = tvanumbercomp,
                companyCreatedBy = companyCreatedBycomp,
                companyUpdatedBy = companyUpdatedBycomp
            };

            {
   
    }


            // Convertissez les données en JSON

            var jsonData = JsonConvert.SerializeObject(data);
            Console.WriteLine(jsonData);
            
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Effectuez une requête HTTP PUT pour mettre à jour l'entreprise
            var response = await _httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            // Si la mise à jour est réussie, retournez les données de l'entreprise mises à jour
            var updatedContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CompanyDto>(updatedContent);
        }

        public async Task<CompanyDto> AddCompanyAsync(string namecomp, string siretcomp, string emailcomp, string tvanumbercomp, string companyCreatedBycomp, string companyUpdatedBycomp)
        {
            var url = BaseUrlAdd;
            // Créez un objet contenant les données de la nouvelle entreprise
            var data = new

            {
                siret = siretcomp,
                name = namecomp,
                email = emailcomp,
                tvaNumber = tvanumbercomp,
                companyCreatedBy = companyCreatedBycomp,
                companyUpdatedBy = companyUpdatedBycomp
            };
            Console.WriteLine(data);
            // Convertissez les données en JSON
            var jsonData = JsonConvert.SerializeObject(data);
            Console.WriteLine(jsonData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Effectuez une requête HTTP POST pour ajouter la nouvelle entreprise
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            // Si l'ajout est réussi, retournez les données de la nouvelle entreprise
            var addedContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CompanyDto>(addedContent);
        }
        public async Task<bool> DeleteCompanyAsync(string companyId)
        {
            try
            {
                // Construire l'URL de l'API pour supprimer l'entreprise spécifiée
                var url = $"{BaseUrlDelete}/{companyId}";

                // Effectuer une requête HTTP DELETE pour supprimer l'entreprise
                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                // Si la suppression est réussie, retourner true
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la suppression de l'entreprise : " + ex.Message);
                return false;
            }
        }


    }

    public class CompanyDto
    {
        public int Id { get; set; }
        public string Siret { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string TvAnumber { get; set; }

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int CompanyCreatedBy { get; set; }

        public int CompanyUpdatedBy { get; set; }
    }
}



