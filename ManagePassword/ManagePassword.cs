using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ManagePassword
{
    public partial class ManagePassword : Form
    {
        public DataGridView Dgv_DB { get => dgvDB; }
        SqlQueries sqlQueries = null;
        FormInfo formInfo_dialog = null;
        AdminMode adminMode_dialog = null;
        public ManagePassword()
        {
            InitializeComponent();
            adminMode_dialog = new AdminMode(this);
            this.CenterToScreen();
            sqlQueries = new SqlQueries(this);
            sqlQueries.circle_query("SELECT id, open_string FROM \"Passwords\""); //Initialize Table
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbAddopen.Text != "" && tbAddsecret.Text != "")
            {
                sqlQueries.single_query($"CALL insertPasswords('{tbAddopen.Text}', '{tbAddsecret.Text}')"); //INSERT
                tbAddopen.Clear();
                tbAddsecret.Clear();
                Refresh();
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (adminMode_dialog.isAdmin)
                sqlQueries.circle_query($"SELECT * FROM \"Passwords\" WHERE \"open_string\" = '{tbopenFind.Text}' OR \"secret_string\" = '{tbsecretFind.Text}'"); //FIND
            else if (!adminMode_dialog.isAdmin)
            {
                sqlQueries.circle_query($"SELECT id, open_string FROM \"Passwords\" WHERE \"open_string\" = '{tbopenFind.Text}'");
                if (tbsecretFind.Text != "")
                {
                    MessageBox.Show("You are not in Admin mode and you don't have access in column 'secret_string'");
                }
            }
            tbopenFind.Clear();
            tbsecretFind.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            sqlQueries.single_query($"CALL deletepasswords('{tbDelid.Text}')"); //DELETE
            tbDelid.Clear();
            Refresh();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        public void Refresh()
        {
            if (adminMode_dialog.isAdmin)
                sqlQueries.circle_query("SELECT * FROM \"Passwords\""); //Refresh
            else if (!adminMode_dialog.isAdmin)
                sqlQueries.circle_query("SELECT id, open_string FROM \"Passwords\"");
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (adminMode_dialog.isAdmin)
            {
                sqlQueries.single_query($"CALL UpdatePasswords('{tbChangeOpen.Text}', '{tbChangeSecret.Text}', '{tbChangeid.Text}')"); //UPDATE
                sqlQueries.single_query($"CALL UpdatePasswords('{tbChangeOpen.Text}', '','{tbChangeid.Text}')");
                tbChangeOpen.Clear();
                tbChangeSecret.Clear();
                tbChangeid.Clear();
                sqlQueries.circle_query("SELECT * FROM \"Passwords\""); //Refresh 
            }
            else if (!adminMode_dialog.isAdmin)
            {
                MessageBox.Show("You are not in Admin mode and you don't have access to function 'Select or Change'");
            }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            formInfo_dialog = new FormInfo();
            formInfo_dialog.StartPosition = FormStartPosition.Manual;
            formInfo_dialog.Location = new Point
                (
                this.Location.X,
                this.Location.Y
                );
            formInfo_dialog.Show();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (adminMode_dialog.isAdmin)
            {
                sqlQueries.circle_query($"SELECT * FROM \"Passwords\" WHERE \"id\" = '{tbChangeid.Text}'");
                if (tbChangeOpen.Text == "" || tbChangeSecret.Text == "")
                {
                    dgvDB.Columns[0].Selected = true;
                    string open_str = tbChangeOpen.Text == "" ? dgvDB.Rows[0].Cells[1].Value.ToString() : tbChangeOpen.Text;
                    string secret_str = tbChangeSecret.Text == "" ? dgvDB.Rows[0].Cells[2].Value.ToString() : tbChangeSecret.Text;
                    tbChangeSecret.Text = secret_str;
                    tbChangeOpen.Text = open_str;
                }
            }
            else if (!adminMode_dialog.isAdmin)
            {
                MessageBox.Show("You are not in Admin mode and you don't have access to function 'Select or Change'");
            }
        }
        private void btnAdmMode_Click(object sender, EventArgs e)
        {
            adminMode_dialog = new AdminMode(this);
            adminMode_dialog.StartPosition = FormStartPosition.Manual;
            adminMode_dialog.Location = new Point
                (
                    this.Location.X,
                    this.Location.Y
                );
            adminMode_dialog.Show();
            Refresh();  
        }
    }
}
