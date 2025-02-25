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
        AdminForm adminMode_dialog = null;
        public ManagePassword()
        {
            InitializeComponent();
            adminMode_dialog = new AdminForm(this);
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
            if (tbopenFind.Text != "" || tbsecretFind.Text != "")
            {
                dgvDB.DataSource = sqlQueries.Find(tbopenFind.Text, tbsecretFind.Text);
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
            CheckSelect();
            sqlQueries.Change(tbChangeOpen.Text, tbChangeSecret.Text, dgvDB.CurrentRow.Cells[0].Value.ToString()); //UPDATE
            tbChangeOpen.Clear();
            tbChangeSecret.Clear();
            lblSelectedId.Text = "Selected id: ";
            Refresh();
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
        public void CheckSelect()
        {
            try
            {
                lblSelectedId.Text += dgvDB.CurrentRow.Cells[0].Value.ToString();
                dgvDB.Rows[dgvDB.CurrentCellAddress.Y].Selected = true;
                string open_str = tbChangeOpen.Text == "" ? dgvDB.Rows[dgvDB.CurrentCellAddress.Y].Cells[1].Value.ToString() : tbChangeOpen.Text;
                string secret_str = tbChangeSecret.Text == "" ? dgvDB.Rows[dgvDB.CurrentCellAddress.Y].Cells[2].Value.ToString() : tbChangeSecret.Text;
                tbChangeSecret.Text = secret_str;
                tbChangeOpen.Text = open_str;
            }
            catch (Exception)
            {
                MessageBox.Show("You are not in Admin mode and you don't have access in column 'Password'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnAdmMode_Click(object sender, EventArgs e)
        {
            adminMode_dialog = new AdminForm(this);
            adminMode_dialog.StartPosition = FormStartPosition.Manual;
            adminMode_dialog.Location = new Point
                (
                    this.Location.X,
                    this.Location.Y
                );
            adminMode_dialog.Show();
            Refresh();
        }

        private void dgvDB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblSelectedId.Text = "Selected id: ";
            sqlQueries.Select(dgvDB, tbChangeOpen, tbChangeSecret);
            CheckSelect();
        }
    }
}
