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
        public AdminForm(ManagePassword parent)
        {
            InitializeComponent();
            mp_dialog = parent;
            CPF_dialog = new CreatePasswordForm();
        }

        private void btnEnterToAdminMode_Click(object sender, EventArgs e)
        {
            AdmMode.hasPassword();
            if (AdmMode.beAdm(tbAdmin.Text))
            {
                mp_dialog.Refresh();
                tbAdmin.Clear();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error!");
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
    }
}
