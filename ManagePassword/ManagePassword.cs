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
        SqlQueries sqlQueries = null;
        FormInfo formInfo_dialog = null;
        AdminMode adminMode_dialog = null;
        public ManagePassword()
        {
            InitializeComponent();
            adminMode_dialog = new AdminMode(this);
            this.CenterToScreen();
            sqlQueries = new SqlQueries();
            Refresh();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbAddopen.Text != "" && tbAddsecret.Text != "")
            {
                sqlQueries.Insert(tbAddopen.Text, tbAddsecret.Text);
                tbAddopen.Clear();
                tbAddsecret.Clear();
                Refresh();
            }
            else 
            {
                MessageBox.Show("text boxes Service and/or Password is never equal null!", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if(tbopenFind.Text != "" || tbsecretFind.Text != "")
            {
            dgvDB.DataSource = sqlQueries.FindAdm(tbopenFind.Text, tbsecretFind.Text);
            tbopenFind.Clear();
            tbsecretFind.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            sqlQueries.Del(tbDelid.Text);
            tbDelid.Clear();
            Refresh();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        public void Refresh()
        { 
          dgvDB.DataSource = sqlQueries.Refresh();  
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            
                if (tbChangeid.Text == "")
                {
                    MessageBox.Show("Select the column", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (tbChangeOpen.Text == "" || tbChangeSecret.Text == "")
                    {
                        CheckSelect();
                    }
                    sqlQueries.Change(tbChangeOpen.Text, tbChangeSecret.Text, tbChangeid.Text); //UPDATE
                    tbChangeOpen.Clear();
                    tbChangeSecret.Clear();
                    tbChangeid.Clear();
                    Refresh();
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

                if (tbChangeid.Text == "")
                {
                    MessageBox.Show("text box 'id' is never equal null", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (tbChangeOpen.Text == "" || tbChangeSecret.Text == "")
                {
                    dgvDB.DataSource = sqlQueries.Select(tbChangeid.Text);
                    CheckSelect();
                }
        }
        public void CheckSelect()
        {
            dgvDB.Columns[0].Selected = true;
            string open_str = tbChangeOpen.Text == "" ? dgvDB.Rows[0].Cells[1].Value.ToString() : tbChangeOpen.Text;
            string secret_str = tbChangeSecret.Text == "" ? dgvDB.Rows[0].Cells[2].Value.ToString() : tbChangeSecret.Text;
            tbChangeSecret.Text = secret_str;
            tbChangeOpen.Text = open_str;
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
