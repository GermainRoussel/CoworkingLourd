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
    public partial class FormCompanyConsultationDatas : Form
    {
        public FormCompanyConsultationDatas()
        {
            InitializeComponent();
        }
        private string companyId;
        private string name;
        private string siret;
        private string mail;
        private string tvanumber;
        private string companyCreatedBy;
        private string companyUpdatedBy;
        private string companyCreatedAt;
        private string companyUpdatedAt;

      
        public FormCompanyConsultationDatas(string companyId, string name, string siret, string mail, string tvanumber, string companyCreatedBy, string companyCreatedAt, string companyUpdatedBy, string companyUpdatedAt)
        {
            InitializeComponent();
            this.companyId = companyId;
            this.name = name;
            this.siret = siret;
            this.mail = mail;
            this.tvanumber = tvanumber;
            this.companyCreatedBy = companyCreatedBy;
            this.companyCreatedAt = companyCreatedAt;
            this.companyUpdatedBy = companyUpdatedBy;
            this.companyUpdatedAt = companyUpdatedAt;

            

            // Affichez les données dans les zones de texte
            textBox_companyId.Text = companyId;
            textBox_name.Text = name;
            textBox_siret.Text = siret;
            textBox_mail.Text = mail;
            textBox_tvanumber.Text = tvanumber;
            textBox1.Text = companyCreatedBy;
            //textBox2.Text = companyCreatedAt;
                
            //textBox3.Text = companyUpdatedBy;   
            //textBox4.Text = companyUpdatedAt;
        }

        
        private void button_cancel_Click(object sender, EventArgs e)
        {
            // Annulez les modifications et fermez la boîte de dialogue
            this.Close();
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            // Copiez les données de l'entreprise dans le presse-papiers
            string data = $"ID : {companyId}\nName : {name}\nSiret : {siret}\nMail : {mail}\nTvaNumber : {tvanumber}";
            Clipboard.SetText(data);
                       MessageBox.Show("Données copiées", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
