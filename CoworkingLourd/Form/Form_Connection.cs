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
    public partial class FormConnection : Form
    {
       
        public FormConnection()
        {
            InitializeComponent();
            InitializeTextBoxPassword();

        }
        private void InitializeTextBoxPassword()
        {
            // Utilisez des étoiles (*) pour masquer le texte
            textBox1.UseSystemPasswordChar = true;
            textBox1.PasswordChar = '•';
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = textBox_FirstName.Text;
            string password = textBox1.Text;

            // Vérifier les informations de connexion dans la base de données
            if (IsValidUser(username, password))
            {
                // Ouverture de la session utilisateur
                SessionManager.SetLoggedInUser(username);

                // Affichage du formulaire principal ou autre formulaire après la connexion réussie
                //Form_CompanyList mainForm = new Form_CompanyList();
                //mainForm.Show();

                // Cacher le formulaire de connexion
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect", "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidUser(string username, string password)
        {
            // Chaîne de connexion à votre base de données
            string connectionString = "server=localhost;database=iptek_db;uid=root;password=root;";
            // Requête SQL pour vérifier l'utilisateur
           string query = "SELECT COUNT(*) FROM users WHERE FirstName=@FirstName AND Password=@Password";

            // Utilisation d'un bloc using pour s'assurer que la connexion est fermée après utilisation
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Création de la commande SQL avec les paramètres
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Ajout des paramètres à la commande
                    command.Parameters.AddWithValue("@FirstName", username);
                    command.Parameters.AddWithValue("@Password", password);
                    

                    // Tentative de connexion à la base de données
                    try
                    {
                        connection.Open();

                        // Exécution de la commande SQL et récupération du résultat
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // Si count est supérieur à 0, cela signifie qu'un utilisateur correspondant a été trouvé
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la connexion à la base de données : " + ex.Message);
                        return false;
                    }
                }
            }
        }

    }

  
}
