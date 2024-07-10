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
    public partial class UserGridTab : UserControl
    {
        #region variables
        public event EventHandler Clicked;
        public event EventHandler ModifyButtonClicked;
        public event EventHandler DeleteButtonClicked;
        public string Label1Text => label1.Text;
        public string Label2Text => label2.Text;
        public string Label3Text => label3.Text;
        public string Label4Text => label4.Text;
        //public string Label5Text => label5.Text;
        #endregion
        public UserGridTab()
        {
            InitializeComponent();
            button_delete.Click += button_delete_Click;
            button_modify.Click += button_modify_Click;

        }

        #region gestion des données
        public void RemplirDonnees(string id, string name, string lastname, string mail, DateTime creationdate)
        {
            label1.Text = id;
            label2.Text = name;
            label3.Text = lastname;
            label4.Text = mail;
            label5.Text = creationdate.ToString();

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
                label5.Text.Contains(searchTerm))
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
        private void button_delete_Click(object sender, EventArgs e)
        {
            // Obtenez l'identifiant de l'entreprise à partir de l'étiquette ou d'une autre source fiable
            string UserId = label1.Text; // Utilisez une autre source si nécessaire

            // Appelez une méthode pour supprimer l'entreprise de la base de données en utilisant l'identifiant
            DialogResult result = MessageBox.Show("Voulez-vous vraiment supprimer cet utilisateur ?", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (DeleteCompanyFromDatabase(UserId))
                {
                    Parent.Controls.Remove(this);

                }
                else
                {
                    MessageBox.Show("La suppression de l'utilisateur a échoué.");
                }
            }
        }
        private bool DeleteCompanyFromDatabase(string userId)
        {
            try
            {
                // Code pour supprimer l'entreprise de la base de données
                string connectionString = "server=localhost;database=iptek_db;uid=root;password=root;";
                using (MySqlConnection connexion = new MySqlConnection(connectionString))
                {
                    string query = "DELETE FROM users WHERE UserId = @UserId;";
                    using (MySqlCommand cmd = new MySqlCommand(query, connexion))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        connexion.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Retourne true si au moins une ligne a été supprimée
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite lors de la suppression de l'entreprise : " + ex.Message);
                return false;
            }
        }
        #endregion

    }
}
