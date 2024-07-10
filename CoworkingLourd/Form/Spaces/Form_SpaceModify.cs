using CoworkingLourd.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CoworkingLourd
{
    public partial class Form_SpaceModify : Form
    {

        private int Idc;


        public Form_SpaceModify(int Id)
        {
            InitializeComponent();
            this.Idc = Id;
            LoadData();
            ConnectedUser.CheckLoggedInUser();
            ShowUserConnected();

        }
        private void ShowUserConnected()
        {
            label_userconnected.Text = ConnectedUser.GetUserIdConnected();
        }

        private async void LoadData()
        {
            try
            {
                List<SpacesDto> datas = await GetDataAsync();
                RefreshData(datas); // Assurez-vous que cette méthode est appelée correctement

                // Recherchez l'entreprise avec l'ID correspondant dans la liste des données
                SpacesDto spaces = datas.FirstOrDefault(c => c.Id == Idc);

                if (spaces != null)
                {
                    // Préremplir les champs Text avec les données de l'entreprise
                    textBox_name.Text = spaces.name;
                    textBox_description.Text = spaces.description;
                    textBox_seats.Text = spaces.nbSeatsTotal.ToString();
                    textBox_type.Text = spaces.type;
                    textBox_adress.Text = spaces.adress;
                    textBox_companyid.Text = spaces.space_id.ToString();
                    textBox_createdby.Text = spaces.spaceCreatedBy.ToString();
                    textBox_createdat.Text = spaces.createdAt.ToString("g");

                    
                }
                else
                {
                    MessageBox.Show("Company non trouvé.");
                    this.Close(); // Fermer le formulaire si l'entreprise n'est pas trouvée
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
            }
        }

        private async Task<List<SpacesDto>> GetDataAsync()
        {
            try
            {
                var apiSpace = new ApiSpaces();
                return await apiSpace.GetSpacesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
                return new List<SpacesDto>();
            }
        }
        private void RefreshData(List<SpacesDto> sortedData)
        {

            int top = 0;
            foreach (var data in sortedData)
            {
                SpacesGridTab tab = new SpacesGridTab();
                tab.RemplirDonneesSpaces(data.Id.ToString(), data.name, data.description, data.nbSeatsTotal.ToString(), data.type, data.adress, data.createdAt.ToString("g"), data.spaceCreatedBy.ToString(), data.space_id.ToString());
                tab.Tag = data.Id; // Stockez l'ID de l'entreprise dans la propriété Tag
                tab.Location = new Point(0, top);
                tab.Dock = DockStyle.Top;
                //panel_todata.Controls.Add(tab);
                //top += tab.Height;

                //tab.Clicked += CompanyGridTab_Clicked;
                //tab.ModifyButtonClicked += Tab_ModifyButtonClicked;
            }
        }


        private async void button_save_Click(object sender, EventArgs e)
        {
            // Récupérer les valeurs saisies dans les TextBox
            string nom = textBox_name.Text;
            string description = textBox_description.Text;
            int nbSeatsTotal;
            if (!int.TryParse(textBox_seats.Text, out nbSeatsTotal))
            {
                MessageBox.Show("Nombre total de sièges invalide", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            string type = textBox_type.Text;
            string adress = textBox_adress.Text;
            string companyId = textBox_companyid.Text;
            string spacecreatedBy = textBox_createdby.Text;
            string spacecreatedAt = textBox_createdat.Text;

            // Vérifier si tous les champs sont remplis
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Veuillez remplir tous les champs", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Créer une instance de l'API Company
                var apiClient = new ApiSpaces();

                // Appeler la méthode d'API pour mettre à jour l'espace
                var updatedSpace = await apiClient.UpdateSpacesAsync(Idc, nom, description, nbSeatsTotal, type, adress, companyId, spacecreatedBy, spacecreatedAt);

                if (updatedSpace != null)
                {
                    MessageBox.Show("Espace modifié avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Fermer le formulaire après la mise à jour
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification de l'espace", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification de l'espace : " + ex.Message);
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
