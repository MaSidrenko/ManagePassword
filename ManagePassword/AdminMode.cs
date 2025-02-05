using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagePassword
{
    public partial class AdminMode : Form
    {
        public bool isAdmin {  get; set; }
        const string PASSWORD = "291305";
        ManagePassword mp_dialog;
        public AdminMode(ManagePassword parent)
        {
            InitializeComponent();
            mp_dialog = parent; 
        }

        private void btnEnterToAdminMode_Click(object sender, EventArgs e)
        {
            if (tbAdmin.Text == PASSWORD)
            {
                isAdmin = true;
                mp_dialog.Refresh();
                tbAdmin.Clear();
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if(isAdmin)
            {
                isAdmin = false;
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
