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

namespace CoworkingLourd
{
    public partial class Form_AddSpace : Form
    {
        private int Idc;
        public Form_AddSpace()
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
            string spacecreatedAt = null;

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
    }
}
