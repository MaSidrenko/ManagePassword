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
        SqlQueries sql;
        string id;
        string service;
        string password;
        ManagePassword MP_dialog;
        public FormChange()
        {
            InitializeComponent();
            sql = new SqlQueries();
            MP_dialog = new ManagePassword();
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
            sql.Change(tbChangeService.Text, tbChangePassword.Text, id);
            lblID.Text = "Selected id: ";
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
