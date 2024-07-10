using CoworkingLourd.Class;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace CoworkingLourd
{

    public partial class Form_UserList : Form
    {
        #region Attributs
        private UserModify modifyForm;
        private bool isSortedAscending = true;
        #endregion

        #region Constructeur
        public Form_UserList()
        {
            InitializeComponent();
            ConnectedUser.GetUserIdConnected();
            LoadData();
            ShowUserConnected();
        }

        #endregion

        #region Afficher utilisateur connecté
        private void ShowUserConnected()
        {
            label_userconnected.Text = ConnectedUser.GetUserIdConnected();
        }
        #endregion

        #region gestion des données
        // Méthode pour charger les données dans le panel
        private void LoadData()
        {

            List<Tuple<string, string, string, string, string>> datas = GetData();
            int top = 0;
            foreach (var data in datas)
            {
                if (DateTime.TryParse(data.Item5, out DateTime creationDate))
                {
                    UserGridTab tab = new UserGridTab();
                    tab.RemplirDonnees(data.Item1, data.Item2, data.Item3, data.Item4, creationDate);
                    tab.Location = new Point(0, top);
                    tab.Dock = DockStyle.Top;
                    panel_todata.Controls.Add(tab);
                    top += tab.Height;

                    tab.Clicked += CompanyGridTab_Clicked;
                    tab.ModifyButtonClicked += Tab_ModifyButtonClicked;
                }
                else
                {
                    MessageBox.Show("Erreur de conversion de la date", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //UserGridTab tab = new UserGridTab();


                //tab.RemplirDonnees(data.Item1, data.Item2, data.Item3, data.Item4, data.Item5);
                //tab.Location = new Point(0, top);
                //tab.Dock = DockStyle.Top;
                //panel_todata.Controls.Add(tab);
                //top += tab.Height;

                // Associer le gestionnaire d'événements Clicked de chaque CompanyGridTab à la méthode CompanyGridTab_Clicked
                //tab.Clicked += CompanyGridTab_Clicked;
                //tab.ModifyButtonClicked += Tab_ModifyButtonClicked;
            }
        }

        // Méthode pour rafraîchir les données dans le panel en fonction du terme de recherche spécifié
        public void LoadData(string searchTerm)
        {
            try
            {
                // Créez une liste pour stocker les données filtrées
                List<Tuple<string, string, string, string, string>> filteredData = new List<Tuple<string, string, string, string, string>>();

                // Connexion à la base de données
                string connectionString = "server=localhost;database=iptek_db;uid=root;password=root;";
                using (MySqlConnection connexion = new MySqlConnection(connectionString))
                {
                    string query = "SELECT * FROM users";

                    // Si un terme de recherche est spécifié, ajoutez une condition WHERE à la requête SQL
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += " WHERE Name LIKE @searchTerm";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, connexion))
                    {
                        // Si un terme de recherche est spécifié, ajoutez le paramètre correspondant
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        }

                        connexion.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Ajoutez les données à la liste filtrée
                                filteredData.Add(new Tuple<string, string, string, string, string>(
                                    reader.GetInt32(0).ToString(),
                                    reader.GetString(2),
                                    reader.GetString(1),
                                    reader.GetString(3),
                                    reader.GetString(4)));
                            }
                        }
                    }
                }

                RefreshData(filteredData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
            }
        }
        private List<Tuple<string, string, string, string, string>> GetData()
        {
            List<Tuple<string, string, string, string, string>> datas = new List<Tuple<string, string, string, string, string>>();

            // Code pour se connecter à la base de données et récupérer les données
            // (Remplacez les détails de connexion par les vôtres)


            string connectionString = "server=localhost;database=iptek_db;uid=root;password=root;";
            using (MySqlConnection connexion = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM users;";
                using (MySqlCommand cmd = new MySqlCommand(query, connexion))
                {
                    connexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Ajouter les données à la liste
                            datas.Add(new Tuple<string, string, string, string, string>(reader.GetInt32(0).ToString(), reader.GetString(2), reader.GetString(1), reader.GetString(3), reader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss")));
                        }
                    }
                }
            }

            return datas;
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
            LoadData();

            foreach (CompanyGridTab tab in panel_todata.Controls.OfType<CompanyGridTab>())
            {
                if (!tab.ContainsSearchTerm(searchTerm))
                {
                    tab.Visible = false; // Masquer les contrôles qui ne correspondent pas au terme de recherche
                }
                else
                {
                    tab.Visible = true; // Afficher les contrôles qui correspondent au terme de recherche
                }
            }
            RearrangeControls();
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
              

                // Faites quelque chose avec ces données textuelles, par exemple, les afficher dans une boîte de dialogue
                //FormCompanyConsultationDatas consData = new FormCompanyConsultationDatas(id, name, siret, mail, tvanumber, companyCreatedBy, companyCreatedAt, companyUpdatedBy, companyUpdatedAt);
                //consData.ShowDialog();
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
        private void Tab_ModifyButtonClicked(object sender, EventArgs e)
        {
            if (sender is UserGridTab clickedTab)
            {
                // Obtenez l'index du contrôle cliqué dans le panel
                int index = panel_todata.Controls.IndexOf(clickedTab);
                // Vérifiez si l'index est valide et obtenez l'ID correspondant
                int userId = (index > -1 && index < GetData().Count) ? int.Parse(GetData()[index].Item1) : -1;

                // Vérifiez si une instance de Form_CompanyModify est déjà ouverte
                if (modifyForm == null || modifyForm.IsDisposed)
                {
                    // Si aucune instance n'est ouverte, créez une nouvelle instance
                    modifyForm = new UserModify(userId);
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
        }
        // Variable pour stocker l'instance de Form_CompanyModify
        private void ModifyForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            modifyForm = null;
        }
        // Méthode pour rafraîchir l'affichage des données avec les données triées
        //private void RefreshData(List<Tuple<string, string, string, string, string>> sortedData)
        //{
        //    panel_todata.Controls.Clear();
        //    int top = 0;
        //    foreach (var data in sortedData)
        //    {
        //        UserGridTab tab = new UserGridTab();
        //        tab.RemplirDonnees(data.Item1, data.Item2, data.Item3, data.Item4, data.Item5);
        //        tab.Location = new Point(0, top);
        //        tab.Dock = DockStyle.Top;
        //        panel_todata.Controls.Add(tab);
        //        top += tab.Height;

        //        tab.Clicked += CompanyGridTab_Clicked;
        //        tab.ModifyButtonClicked += Tab_ModifyButtonClicked;
        //    }
        //}
        private void RefreshData(List<Tuple<string, string, string, string, string>> sortedData)
        {
            panel_todata.Controls.Clear();
            int top = 0;
            foreach (var data in sortedData)
            {
                if (DateTime.TryParse(data.Item5, out DateTime creationDate))
                {
                    UserGridTab tab = new UserGridTab();
                    tab.RemplirDonnees(data.Item1, data.Item2, data.Item3, data.Item4, creationDate);
                    tab.Location = new Point(0, top);
                    tab.Dock = DockStyle.Top;
                    panel_todata.Controls.Add(tab);
                    top += tab.Height;

                    tab.Clicked += CompanyGridTab_Clicked;
                    tab.ModifyButtonClicked += Tab_ModifyButtonClicked;
                }
                else
                {
                    MessageBox.Show("Erreur de conversion de la date", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Méthode pour trier les données en fonction de l'ID
        private void label_id_Click(object sender, EventArgs e)
        {
            // Inversez l'état du tri
            isSortedAscending = !isSortedAscending;

            // Tri des données en fonction de l'ID dans l'ordre ascendant ou descendant
            List<Tuple<string, string, string, string, string>> sortedData;
            if (isSortedAscending)
            {
                sortedData = GetData().OrderBy(data => data.Item1).ToList();
            }
            else
            {
                sortedData = GetData().OrderByDescending(data => data.Item1).ToList();
            }

            // Rafraîchir l'affichage des données
            RefreshData(sortedData);
        }
        // Méthode pour trier les données en fonction du nom de l'entreprise
        private void label_companyname_Click(object sender, EventArgs e)
        {
            // Inversez l'état du tri
            isSortedAscending = !isSortedAscending;

            // Tri des données en fonction de l'ID dans l'ordre ascendant ou descendant
            List<Tuple<string, string, string, string, string>> sortedData;
            if (isSortedAscending)
            {
                sortedData = GetData().OrderBy(data => data.Item2).ToList();
            }
            else
            {
                sortedData = GetData().OrderByDescending(data => data.Item2).ToList();
            }

            // Rafraîchir l'affichage des données
            RefreshData(sortedData);
        }
        // Méthode pour trier les données en fonction du SIRET
        private void label_siret_Click(object sender, EventArgs e)
        {
            // Inversez l'état du tri
            isSortedAscending = !isSortedAscending;

            // Tri des données en fonction de l'ID dans l'ordre ascendant ou descendant
            List<Tuple<string, string, string, string, string>> sortedData;
            if (isSortedAscending)
            {
                sortedData = GetData().OrderBy(data => data.Item3).ToList();
            }
            else
            {
                sortedData = GetData().OrderByDescending(data => data.Item3).ToList();
            }

            // Rafraîchir l'affichage des données
            RefreshData(sortedData);
        }
        // Méthode pour trier les données en fonction du numéro de TVA
        private void label_tvanumber_Click(object sender, EventArgs e)
        {
            // Inversez l'état du tri
            isSortedAscending = !isSortedAscending;

            // Tri des données en fonction de l'ID dans l'ordre ascendant ou descendant
            List<Tuple<string, string, string, string, string>> sortedData;
            if (isSortedAscending)
            {
                sortedData = GetData().OrderBy(data => data.Item4).ToList();
            }
            else
            {
                sortedData = GetData().OrderByDescending(data => data.Item4).ToList();
            }

            // Rafraîchir l'affichage des données
            RefreshData(sortedData);
        }
        // Méthode pour trier les données en fonction de l'adresse mail
        private void label_mail_Click(object sender, EventArgs e)
        {
            // Inversez l'état du tri
            isSortedAscending = !isSortedAscending;

            // Tri des données en fonction de l'ID dans l'ordre ascendant ou descendant
            List<Tuple<string, string, string, string, string>> sortedData;
            if (isSortedAscending)
            {
                sortedData = GetData().OrderBy(data => data.Item5).ToList();
            }
            else
            {
                sortedData = GetData().OrderByDescending(data => data.Item5).ToList();
            }

            // Rafraîchir l'affichage des données
            RefreshData(sortedData);
        }
        #endregion


    }
}
