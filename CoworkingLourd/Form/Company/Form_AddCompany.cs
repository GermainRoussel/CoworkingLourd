using CoworkingLourd.Class;
using System;
using System.Windows.Forms;

namespace CoworkingLourd
{
    public partial class FormAddCompany : Form
    {
        readonly string connectionString = "server=localhost;database=iptek_db;uid=root;password=root";

        public FormAddCompany()
        {
            InitializeComponent();
            ConnectedUser.CheckLoggedInUser();
            ShowUserConnected();
        }

        #region Afficher utilisateur connecté
        private void ShowUserConnected()
        {
            label_userconnected.Text = ConnectedUser.GetUserIdConnected();
        }
        #endregion
        private async void button_save_Click(object sender, EventArgs e)
        {
            // Récupérer les valeurs saisies dans les TextBox

            string siret = textBox_siret.Text;
            string nom = textBox_name.Text;
            string email = textBox_mail.Text;
            string tvanumber = textBox_tva.Text;
            string createdBy = textBox_createdby.Text;
            string updatedBy = textBox_createdby.Text;

            // Vérifier si tous les champs sont remplis
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(siret) || string.IsNullOrWhiteSpace(tvanumber) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(createdBy))
            {
                MessageBox.Show("Veuillez remplir tous les champs", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Créer une instance de la classe ApiCompany
                var apiCompany = new ApiCompany();
                

                // Appeler la méthode AddCompanyAsync pour ajouter la nouvelle entreprise
                var addedCompany = await apiCompany.AddCompanyAsync(nom, siret, email, tvanumber, createdBy, updatedBy);
                
                // Vérifier si l'ajout a réussi
                if (addedCompany != null)
                {
                    MessageBox.Show("Company ajoutée avec succès", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout de la Company", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur s'est produite : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox_mail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
