using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagePassword
{
    public partial class AdminForm : Form
    {
        ManagePassword mp_dialog;
        CreatePasswordForm CPF_dialog;
        AdmDelete admDelete_dialog;
        public AdminForm(ManagePassword parent)
        {
            InitializeComponent();
            mp_dialog = parent;
            CPF_dialog = new CreatePasswordForm();
        }

        private void btnEnterToAdminMode_Click(object sender, EventArgs e)
        {
            if (AdmMode.AuthenticateAdm(tbAdmin.Text))
            {
                AdmMode.isAdm = true;
                AdmMode.GetAdmPassword(tbAdmin.Text);
                mp_dialog.Refresh();
                tbAdmin.Clear();
                this.Close();
            }
            else if(tbAdmin.Text == "")
            {
                MessageBox.Show("text box Admin Password has been not euqal null!");
            }
            else
            {
                MessageBox.Show("You don`t have a password!");
                CPF_dialog.Show();
            }            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (AdmMode.isAdm)
            {
                AdmMode.isAdm = false;
                mp_dialog.Refresh();
            }
            this.Close();
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            CPF_dialog.Show();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            admDelete_dialog = new AdmDelete();
            admDelete_dialog.Show();
        }
    }
}
