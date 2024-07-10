using CoworkingLourd.Class;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoworkingLourd
{
    public partial class Form_Home : Form
    {
        #region déclaration des variables
        
        #endregion

        #region Constructeur
        public Form_Home()
        {

            InitializeComponent();
            LoadData();
            
            ConnectedUser.CheckLoggedInUser();
            ShowUserConnected();

        }
        #endregion

        #region Afficher utilisateur connecté
        // Affiche l'utilisateur connecté 
        private void ShowUserConnected()
        {
            label_userconnected.Text = ConnectedUser.GetUserIdConnected();
        }
        #endregion


        #region Gestion des données
        // Charge les données dans le formulaire
        private void LoadData()
        {

            List<Tuple<string, string, string, string, string>> datas = GetData();
            //int top = 0;
            //foreach (var data in datas)
            //{
            //    CompanyGridTab tab = new CompanyGridTab();
            //    tab.RemplirDonnees(data.Item1, data.Item2, data.Item3, data.Item4, data.Item5);
            //    tab.Location = new Point(0, top);
            //    tab.Dock = DockStyle.Top;
            //    panel_todata.Controls.Add(tab);
            //    top += tab.Height;

            //    // Associer le gestionnaire d'événements Clicked de chaque CompanyGridTab à la méthode CompanyGridTab_Clicked
            //    //tab.Clicked += CompanyGridTab_Clicked;
            //    //tab.ModifyButtonClicked += Tab_ModifyButtonClicked;
            //}
        }
        // Récupère les données de la base de données
        private List<Tuple<string, string, string, string, string>> GetData()
        {
            List<Tuple<string, string, string, string, string>> datas = new List<Tuple<string, string, string, string, string>>();

            // Code pour se connecter à la base de données et récupérer les données
            // (Remplacez les détails de connexion par les vôtres)


            string connectionString = "server=localhost;database=iptek_db;uid=root;password=root;";
            using (MySqlConnection connexion = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM company;";
                using (MySqlCommand cmd = new MySqlCommand(query, connexion))
                {
                    connexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Ajouter les données à la liste
                            datas.Add(new Tuple<string, string, string, string, string>(reader.GetInt32(0).ToString(), reader.GetString(2), reader.GetString(1), reader.GetString(3), reader.GetString(4)));
                        }
                    }
                }
            }

            return datas;
        }
        #endregion

        #region Interactions
        // Ouvre le formulaire d'ajout de l'entreprise
        private void button_addcompany_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'ajout d'une nouvelle entreprise
            FormAddCompany form = new FormAddCompany();
            form.ShowDialog();

            
        }
        // Ouvre le formulaire de liste d'entreprises
        private void button_listcompany_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'ajout d'une nouvelle entreprise
            FormCompanyList Clist = new FormCompanyList();
            Clist.ShowDialog();
           

            

            LoadData(); // Recharger les données après l'ajout
        }
        // Ouvre le formulaire de liste d'users
        private void button_listuser_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'ajout d'une nouvelle entreprise
            Form_UserList Ulist = new Form_UserList();
            Ulist.ShowDialog();




            LoadData(); // Recharger les données après l'ajout
        }
        // Ouvre le formulaire de liste des salles
        private void button_listspace_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'ajout d'une nouvelle entreprise
            Form_SpaceList Slist = new Form_SpaceList();
            Slist.ShowDialog();




            LoadData(); // Recharger les données après l'ajout
        }
        // Ouvre le formulaire de liste des pièces
        private void button_listpieces_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'ajout d'une nouvelle entreprise
            Form_PiecesList Plist = new Form_PiecesList();
            Plist.ShowDialog();




            LoadData(); // Recharger les données après l'ajout
        }
        // Efface le contenu du textBox_search lorsqu'il est cliqué  
        private void textBox_companySearch(object sender, EventArgs e)
        {
            
            textBox_companysearch.Text = "";
        }
        // Recharge les données de l'entreprise
        private void button_realoadcompany_Click(object sender, EventArgs e)
        {
            // Créez une instance de Form_CompanyList
            FormCompanyList companyListForm = new FormCompanyList();

            // Obtenez le terme de recherche à partir de textBox_companysearch
            string searchTerm = textBox_companysearch.Text;

            // Chargez les données dans Form_CompanyList avec le terme de recherche spécifié
            companyListForm.LoadData(searchTerm);

            // Affichez le formulaire
            companyListForm.Show();
        }
        private void button_addspace_Click(object sender, EventArgs e)
        {
            // Ouvrir le formulaire d'ajout d'une nouvelle entreprise
            Form_AddSpace form = new Form_AddSpace();
            form.ShowDialog();

            
        }
        #endregion
    }
}
