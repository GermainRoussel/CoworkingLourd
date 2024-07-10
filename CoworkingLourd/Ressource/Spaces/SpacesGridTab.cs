using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoworkingLourd
{
    public partial class SpacesGridTab : UserControl
    {
        #region variables
        public event EventHandler Clicked;
        public event EventHandler ModifyButtonClicked;
        public event EventHandler DeleteButtonClicked;
        public string Label1Text => label1.Text;
        public string Label2Text => label2.Text;
        public string Label3Text => label3.Text;
        public string Label4Text => label4.Text;
        public string Label5Text => label5.Text;
        public string Label6Text => label6.Text;
        public string Label7Text => label7.Text;
        public string Label8Text => label8.Text;
        public string Label9Text => label9.Text;

        private readonly HttpClient _httpClient;

        #endregion

        #region constructeur
        public SpacesGridTab()
        {
            InitializeComponent();
            button_delete.Click += button_delete_Click;
            button_modify.Click += button_modify_Click;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://127.0.0.1:8000/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
        #endregion

        #region gestion des données
        public void RemplirDonneesSpaces(string id, string nameSpace, string descriptionSpace, string nbSeatsTotals, string types, string adressSpace, string spacecreatedAt, string spacecreatedBy, string space_id)
        {
            label1.Text = id;
            label2.Text = nameSpace;
            label3.Text = descriptionSpace;
            label4.Text = nbSeatsTotals;
            label5.Text = types;
            label6.Text = adressSpace;
            label7.Text = spacecreatedAt;
            label8.Text = spacecreatedBy;
            label9.Text = space_id;



        }
        public bool ContainsSearchTerm(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return true;
            }
            if (label1.Text.Contains(searchTerm) ||
                label2.Text.Contains(searchTerm) ||
                label3.Text.Contains(searchTerm) ||
                label4.Text.Contains(searchTerm) ||
                label5.Text.Contains(searchTerm) ||
                label6.Text.Contains(searchTerm) ||
                label7.Text.Contains(searchTerm) ||
                label8.Text.Contains(searchTerm) ||
                label9.Text.Contains(searchTerm)
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Interactions
        private void button_modify_Click(object sender, EventArgs e)
        {
            ModifyButtonClicked?.Invoke(this, EventArgs.Empty);
        }
        private async void button_delete_Click(object sender, EventArgs e)
        {
            // Obtenez l'identifiant de l'entreprise à partir de l'étiquette ou d'une autre source fiable
            string spaceId = label1.Text;


            DialogResult result = MessageBox.Show("Voulez-vous vraiment supprimer cette salle ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Appelez la méthode pour supprimer l'entreprise via l'API
                if (await DeleteCompanyAsync(spaceId))
                {

                    MessageBox.Show("La salle a été supprimée avec succès.");
                }
                else
                {
                    MessageBox.Show("La suppression de la salle a échoué.");
                }
            }
        }

        private async Task<bool> DeleteCompanyAsync(string spaceId)
        {
            try
            {
                // Construire l'URL de l'API pour supprimer l'entreprise spécifiée
                var url = $"http://127.0.0.1:8000/api/space/delete/{spaceId}";

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

        #endregion

        #region Non utilisé
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void CompanyGridTab_Click(object sender, EventArgs e)
        {

            Clicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
