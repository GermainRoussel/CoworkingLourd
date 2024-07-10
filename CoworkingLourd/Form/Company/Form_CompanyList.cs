using CoworkingLourd.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace CoworkingLourd
{
    public partial class FormCompanyList : Form
    {
        #region Déclaration des variables
        private Form_CompanyModify modifyForm;
        private bool isSortedAscending = true;

        #endregion

        #region Constructeur    
        public FormCompanyList()
        {
           
            InitializeComponent();
            LoadData();
            ConnectedUser.CheckLoggedInUser();
            ShowUserConnected();
            


        }
        #endregion

        #region Afficher utilisateur connecté
        private void ShowUserConnected() 
        {
            label_userconnected.Text = ConnectedUser.GetUserIdConnected();
        }
        #endregion

        #region Gestion des données
        public void RemplirDonnees(string id, string name, string siret, string mail, string tvanumber)
        {
            labelcreationid.Text = id;
            label2.Text = name;
            label_modifdate.Text = siret;
            label_modifid.Text = mail;
            label_creationdate.Text = tvanumber;
           

        }

        private async void LoadData()
        {
            try
            {
                List<CompanyDto> datas = await GetDataAsync();
                RefreshData(datas); // Assurez-vous que cette méthode est appelée correctement
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
            }
        }
        public async void LoadData(string searchTerm = "")
        {
            // Le code de chargement des données ici
            try
            {
                List<CompanyDto> datas = await GetDataAsync(searchTerm);
                RefreshData(datas);
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
        private async Task<List<CompanyDto>> GetDataAsync(string searchTerm = "")
        {
            try
            {
                var apiClient = new ApiCompany();
                return await apiClient.GetCompaniesAsync(searchTerm);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
                return new List<CompanyDto>();
            }
        }
        #endregion

        #region Interractions
        // Méthode pour effacer le texte par défaut lorsqu'un utilisateur clique sur la zone de recherche
        private void textBox_search_Click(object sender, EventArgs e)
        {
           textBox_search.Text = ""; 
        }
        // Méthode pour recharger les données lorsqu'un utilisateur clique sur le bouton de rechargement
        private void RearrangeControls()
        {
            int top = 0;
            foreach (Control control in panel_todata.Controls)
            {
                control.Location = new Point(0, top);
                top += control.Height;
            }
        }
        // Méthode pour recharger les données lorsqu'un utilisateur clique sur le bouton de rechargement
        private void button_reload_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox_search.Text.Trim();
            panel_todata.Controls.Clear();
            LoadData(searchTerm); // Assurez-vous d'appeler la bonne méthode LoadData() ici
                                  // Le reste du code pour filtrer et réorganiser les données
        }
        // Ouvre un form Consultation Data avec les données de l'entreprise
        private void CompanyGridTab_Clicked(object sender, EventArgs e)
        {
            if (sender is CompanyGridTab clickedTab)
            {
                // Récupérer les données textuelles du CompanyGridTab cliqué à partir des méthodes publiques
                string id = clickedTab.Label1Text;
                string name = clickedTab.Label2Text;
                string siret = clickedTab.Label3Text;
                string mail = clickedTab.Label4Text;
                string tvanumber = clickedTab.Label5Text;
                string companyCreatedBy = clickedTab.Label6Text;
                string companyCreatedAt = clickedTab.Label7Text;
                string companyUpdatedBy = clickedTab.Label8Text;
                string companyUpdatedAt = clickedTab.Label9Text;

                // Faites quelque chose avec ces données textuelles, par exemple, les afficher dans une boîte de dialogue
                FormCompanyConsultationDatas consData = new FormCompanyConsultationDatas(id, name, siret, mail, tvanumber, companyCreatedBy, companyCreatedAt, companyUpdatedAt,companyUpdatedAt);
                consData.ShowDialog();
            }
        }
        // Méthode pour rechercher les données lorsqu'un utilisateur tape dans la zone de recherche
        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            // Réinitialiser la recherche lorsque le texte du champ de recherche change
            foreach (CompanyGridTab tab in panel_todata.Controls.OfType<CompanyGridTab>())
            {
                tab.Visible = true; // Réafficher tous les contrôles
            }
        }
        // Ouvrir le formulaire d'ajout d'une nouvelle entreprise
        private void button_addcompany_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'ajout d'une nouvelle entreprise
            FormAddCompany form = new FormAddCompany();
            form.ShowDialog();

            panel_todata.Controls.Clear();

            LoadData(); // Recharger les données après l'ajout
        }
        // Ouvrir le formulaire de modification de l'entreprise
        private  void Tab_ModifyButtonClicked(object sender, EventArgs e)
        {
            if (sender is CompanyGridTab clickedTab)
            {
                // Obtenez l'ID de l'entreprise à partir de la propriété Tag
                if (int.TryParse(clickedTab.Tag?.ToString(), out int Id))
                {
                    // Vérifiez si une instance de FormCompanyModify est déjà ouverte
                    if (modifyForm == null || modifyForm.IsDisposed)
                    {
                        // Si aucune instance n'est ouverte, créez une nouvelle instance
                        modifyForm = new Form_CompanyModify(Id);
                        // Abonnez-vous à l'événement FormClosed pour savoir quand le formulaire est fermé
                        modifyForm.FormClosed += ModifyForm_FormClosed;
                        // Affichez le formulaire
                        modifyForm.Show();
                    }
                    else
                    {
                        // Si une instance est déjà ouverte, mettez-la au premier plan
                        modifyForm.BringToFront();
                    }
                }
                else
                {
                    MessageBox.Show("ID d'entreprise invalide.");
                }
            }
        }
        // Variable pour stocker l'instance de Form_CompanyModify
        private void ModifyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           
            modifyForm = null;
        }
        // Méthode pour rafraîchir l'affichage des données avec les données triées
        private void RefreshData(List<CompanyDto> sortedData)
        {
            panel_todata.Controls.Clear();
            int top = 0;
            foreach (var data in sortedData)
            {
                CompanyGridTab tab = new CompanyGridTab();
                tab.RemplirDonneesCompany(data.Id.ToString(), data.Name, data.Siret, data.Email, data.TvAnumber, data.createdAt.ToString("g"), data.updatedAt.ToString("g"), data.CompanyCreatedBy.ToString() , data.CompanyUpdatedBy.ToString());
                tab.Tag = data.Id; // Stockez l'ID de l'entreprise dans la propriété Tag
                tab.Location = new Point(0, top);
                tab.Dock = DockStyle.Top;
                panel_todata.Controls.Add(tab);
                top += tab.Height;

                tab.Clicked += CompanyGridTab_Clicked;
                tab.ModifyButtonClicked += Tab_ModifyButtonClicked;
            }
        }
        // Méthode pour trier les données en fonction de l'ID
        private async void label_id_Click(object sender, EventArgs e)
        {
            try
            {
                // Inversez l'état du tri
                isSortedAscending = !isSortedAscending;

                // Tri des données en fonction de l'ID dans l'ordre ascendant ou descendant
                List<CompanyDto> sortedData;
                if (isSortedAscending)
                {
                    sortedData = (await GetDataAsync()).OrderBy(data => data.Id).ToList();
                }
                else
                {
                    sortedData = (await GetDataAsync()).OrderByDescending(data => data.Id).ToList();
                }

                // Rafraîchir l'affichage des données
                RefreshData(sortedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du tri des données : " + ex.Message);
            }
        }

        private void ResetSearchControls()
        {
            textBox_search.Text = "";
            foreach (CompanyGridTab tab in panel_todata.Controls.OfType<CompanyGridTab>())
            {
                tab.Visible = true;
            }
        }
        // Méthode pour trier les données en fonction du nom de l'entreprise
        private async void label_companyname_Click(object sender, EventArgs e)
        {
            try
            {
                // Inversez l'état du tri
                isSortedAscending = !isSortedAscending;

                // Tri des données en fonction du nom de l'entreprise dans l'ordre ascendant ou descendant
                List<CompanyDto> sortedData;
                if (isSortedAscending)
                {
                    sortedData = (await GetDataAsync()).OrderBy(data => data.Name).ToList();
                }
                else
                {
                    sortedData = (await GetDataAsync()).OrderByDescending(data => data.Name).ToList();
                }

                // Rafraîchir l'affichage des données
                RefreshData(sortedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du tri des données : " + ex.Message);
            }
        }
        // Méthode pour trier les données en fonction du SIRET
        private async void label_siret_Click(object sender, EventArgs e)
        {
            try
            {
                // Inversez l'état du tri
                isSortedAscending = !isSortedAscending;

                // Tri des données en fonction du numéro SIRET dans l'ordre ascendant ou descendant
                List<CompanyDto> sortedData;
                if (isSortedAscending)
                {
                    sortedData = (await GetDataAsync()).OrderBy(data => data.Siret).ToList();
                }
                else
                {
                    sortedData = (await GetDataAsync()).OrderByDescending(data => data.Siret).ToList();
                }

                // Rafraîchir l'affichage des données
                RefreshData(sortedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du tri des données : " + ex.Message);
            }
        }
        // Méthode pour trier les données en fonction du numéro de TVA
        private async void label_tvanumber_Click(object sender, EventArgs e)
        {
            try
            {
                // Inversez l'état du tri
                isSortedAscending = !isSortedAscending;

                // Tri des données en fonction du numéro de TVA dans l'ordre ascendant ou descendant
                List<CompanyDto> sortedData;
                if (isSortedAscending)
                {
                    sortedData = (await GetDataAsync()).OrderBy(data => data.TvAnumber).ToList();
                }
                else
                {
                    sortedData = (await GetDataAsync()).OrderByDescending(data => data.TvAnumber).ToList();
                }

                // Rafraîchir l'affichage des données
                RefreshData(sortedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du tri des données : " + ex.Message);
            }
        }
        // Méthode pour trier les données en fonction de l'adresse mail
        private async void label_mail_Click(object sender, EventArgs e)
        {
            try
            {
                // Inversez l'état du tri
                isSortedAscending = !isSortedAscending;

                // Tri des données en fonction de l'adresse e-mail dans l'ordre ascendant ou descendant
                List<CompanyDto> sortedData;
                if (isSortedAscending)
                {
                    sortedData = (await GetDataAsync()).OrderBy(data => data.Email).ToList();
                }
                else
                {
                    sortedData = (await GetDataAsync()).OrderByDescending(data => data.Email).ToList();
                }

                // Rafraîchir l'affichage des données
                RefreshData(sortedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du tri des données : " + ex.Message);
            }
        }
        #endregion


        #region Non utilisé
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fillDataGrid()
        {

        }

        private void CompanyGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        
       

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel_h1andbuttons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_company_Click(object sender, EventArgs e)
        {

        }

        private void panel_footer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label_userconnected_Click(object sender, EventArgs e)
        {

        }

        private void button_piece_Click(object sender, EventArgs e)
        {

        }

        private void Pannel_Tab_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel_todata_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label_delete_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
#endregion

}
