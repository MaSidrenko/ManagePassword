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
    public partial class FormChange : Form
    {
        string id;
        string service;
        string password;
        ManagePasswordForm MP_dialog;
        public FormChange()
        {
            InitializeComponent();
            MP_dialog = new ManagePasswordForm();
        }
        public void Select(string Service, string Password, string ID)
        {
            service = Service;
            password = Password;
            id = ID;
            tbChangeService.Text = service;
            tbChangePassword.Text = password;
            lblID.Text += id;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if(tbChangeService.Text == "")
            {
                tbChangeService.Text = service;
            }
            if(tbChangePassword.Text == "")
            {
                tbChangePassword.Text = password;
            }
            lblID.Text = "Selected id: ";
            Model.QueriesDB.Change(tbChangeService.Text, tbChangePassword.Text, id);
            //MP_dialog.SavePassword();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

		private void showPass_CheckedChanged(object sender, EventArgs e)
		{
            if(showPass.Checked)
            {
                tbChangePassword.PasswordChar = '\0';
            }
            else if(!showPass.Checked)
            {
                tbChangePassword.PasswordChar = '*';
            }
		}

		private void showPass_Click(object sender, EventArgs e)
		{
           showPass.Checked = showPass.Checked == true ? false : true;
		}
	}
}
