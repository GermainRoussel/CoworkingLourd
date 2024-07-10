using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoworkingLourd.Class
{
    public class ApiSpaces
    {

        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://127.0.0.1:8000/api/space/";
        private const string BaseUrlUpdate = "http://127.0.0.1:8000/api/space/update";
        private const string BaseUrlDelete = "http://127.0.0.1:8000/api/space/delete";
        private const string BaseUrlAdd = "http://127.0.0.1:8000/api/space/create/";



        public ApiSpaces()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<SpacesDto>> GetSpacesAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SpacesDto>>(content);
        }
        public async Task<List<SpacesDto>> GetSpacesAsync(string searchTerm)
        {
            // Construisez votre URL en fonction du terme de recherche
            var url = BaseUrl + "?search=" + searchTerm;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SpacesDto>>(content);
        }

        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int nbSeatsTotal { get; set; }
        public string type { get; set; }
        public string adress { get; set; }

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int spaceCreatedBy { get; set; }

        public int spaceUpdatedBy { get; set; }
        public int space_id { get; set; }
    

    public async Task<SpacesDto> UpdateSpacesAsync(int id, string nameSpace, string descriptionSpace, int nbSeatsTotals, string types, string adressSpace, string spacecreatedAt, string spacecreatedBy, string space_id)
        {
            // Construisez votre URL en fonction de l'ID de l'entreprise
            var url = $"{BaseUrlUpdate}/{id}";

            // Créez un objet contenant les données à mettre à jour
            var data = new
            {
                name = nameSpace,
                description = descriptionSpace,
                nbSeatsTotal = nbSeatsTotals,
                type = types,
                adress = adressSpace,
                createdAt = spacecreatedAt,
                createdBy = spacecreatedBy,
                spaceid = space_id
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
            return JsonConvert.DeserializeObject<SpacesDto>(updatedContent);
        }

        public async Task<SpacesDto> AddCompanyAsync(int id, string nameSpace, string descriptionSpace, int nbSeatsTotals, string types, string adressSpace, string spacecreatedAt, string spacecreatedBy, string space_id)
        {
            var url = BaseUrlAdd;
            // Créez un objet contenant les données de la nouvelle entreprise
            var data = new

            {
                name = nameSpace,
                description = descriptionSpace,
                nbSeatsTotal = nbSeatsTotals,
                type = types,
                adress = adressSpace,
                createdAt = spacecreatedAt,
                createdBy = spacecreatedBy,
                spaceid = space_id
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
            return JsonConvert.DeserializeObject<SpacesDto>(addedContent);
        }
        public async Task<bool> DeleteSpaceAsync(string spaceId)
        {
            try
            {
                // Construire l'URL de l'API pour supprimer l'entreprise spécifiée
                var url = $"{BaseUrlDelete}/{spaceId}";

                // Effectuer une requête HTTP DELETE pour supprimer l'entreprise
                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                // Si la suppression est réussie, retourner true
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la suppression de la salle : " + ex.Message);
                return false;
            }
        }


    }

    public class SpacesDto
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int nbSeatsTotal { get; set; }
        public string type { get; set; }
        public string adress { get; set; }

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int spaceCreatedBy { get; set; }

        public int spaceUpdatedBy { get; set; }
        public int space_id { get; set;}
    }
}

