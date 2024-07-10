using CoworkingLourd.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoworkingLourd
{
        
    public partial class Form_CompanyModify : Form
    {
        
        private int Idc;
        

        public Form_CompanyModify(int Id)
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
                List<CompanyDto> datas = await GetDataAsync();
                RefreshData(datas); // Assurez-vous que cette méthode est appelée correctement

                // Recherchez l'entreprise avec l'ID correspondant dans la liste des données
                CompanyDto company = datas.FirstOrDefault(c => c.Id == Idc);

                if (company != null)
                {
                    // Préremplir les champs Text avec les données de l'entreprise
                    textBox_name.Text = company.Name;
                    textBox_siret.Text = company.Siret;
                    textBox_mail.Text = company.Email;
                    textBox_tva.Text = company.TvAnumber;
                    textBox3.Text = company.CompanyCreatedBy.ToString();
                    textBox4.Text = company.CompanyUpdatedBy.ToString();
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

        private async Task<List<CompanyDto>> GetDataAsync()
        {
            try
            {
                var apiClient = new ApiCompany();
                return await apiClient.GetCompaniesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
                return new List<CompanyDto>();
            }
        }
        private void RefreshData(List<CompanyDto> sortedData)
        {
            
            int top = 0;
            foreach (var data in sortedData)
            {
                CompanyGridTab tab = new CompanyGridTab();
                tab.RemplirDonneesCompany(data.Id.ToString(), data.Name, data.Siret, data.Email, data.TvAnumber, data.CompanyCreatedBy.ToString(), data.CompanyUpdatedBy.ToString(), data.createdAt.ToString("G"),data.updatedAt.ToString("G") ) ;
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
            
            string nom= textBox_name.Text;
            string siret = textBox_siret.Text;
            string email = textBox_mail.Text;
            string tvanumber = textBox_tva.Text;
            string companycreatedby = textBox4.Text;
            string companyupdatedby = textBox4.Text;

            // Vérifier si tous les champs sont remplis
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(siret) || string.IsNullOrWhiteSpace(tvanumber) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Veuillez remplir tous les champs", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Créer une instance de l'API Company
                var apiClient = new ApiCompany();

                // Appeler la méthode d'API pour mettre à jour l'entreprise
                var updatedCompany = await apiClient.UpdateCompanyAsync(Idc, nom, siret, email, tvanumber, companycreatedby, companyupdatedby);

                if (updatedCompany != null)
                {
                    MessageBox.Show(updatedCompany.Name);
                    MessageBox.Show("Fournisseur modifié avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Fermer le formulaire après la mise à jour
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la modification : " + ex.Message);
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
