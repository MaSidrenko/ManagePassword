using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ManagePassword
{
    public partial class ManagePasswordForm : Form
    {
        FormInfo formInfo_dialog = null;
        AdminForm adminMode_dialog = null;
        public ManagePasswordForm()
        {
            InitializeComponent();
            adminMode_dialog = new AdminForm(this);
            this.CenterToScreen();
            Refresh();
        }
        public void Refresh()
        {

            dgvDB.DataSource = QueriesDB.Refresh();
            foreach (DataGridViewColumn column in dgvDB.Columns.Cast<DataGridViewColumn>().ToList())
            {
                if (column.Name == "password_hash")
                {
                    dgvDB.DataSource = DecryptPasswordDB((DataTable)dgvDB.DataSource);
                    if (dgvDB.Rows.Count > 0)
                        RemoveColumns((DataTable)dgvDB.DataSource);
                }
            }
        }
        public void RemoveColumns(DataTable table)
        {
            table.Columns.Remove("password_hash");
            table.Columns.Remove("salt");
            table.Columns.Remove("aes_iv");
        }
        //Не помню зачем
        //TODO
        public DataTable DecryptPasswordDB(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                byte[] encryptedData = (byte[])row["password_hash"];
                byte[] salt = (byte[])row["salt"];
                byte[] iv = (byte[])row["aes_iv"];


                if (!table.Columns.Contains("Password"))
                {
                    table.Columns.Add("Password", typeof(string));
                }

                if (encryptedData != null && salt != null && iv != null)
                {
                    //TODO
                    byte[] key = Cipher.DeriveKey(AdmMode.AdmPassword, salt);
                    string decrypted = Cipher.DecryptAES(encryptedData, key, iv);
                    row["Password"] = decrypted;
                }
            }
            return table;
        }
        private void dgvDB_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Right))
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void cmAdd_MouseDown(object sender, MouseEventArgs e)
        {
            adminMode_dialog = new AdminForm(this);
            if (AdmMode.AdmPassword == "")
            {
                MessageBox.Show("Type Master Password!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                adminMode_dialog.Show();
                MessageBox.Show("Click second time on Add!");
            }
            else if (AdmMode.AdmPassword != "")
            {

                FormAdd formAdd_dialog = new FormAdd();
                if (formAdd_dialog.ShowDialog() == DialogResult.OK)
                {
                    Refresh();
                }
            }
        }
        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChange formChange_dialog = new FormChange();
            formChange_dialog.ShowDialog();
        }

        private void dgvDB_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //КОСТЫЛЬ ЗДЕСЬ!!!
            try
            {
                string id = dgvDB.Rows[e.RowIndex].Cells[0].Value.ToString();
                string Service = dgvDB.Rows[e.RowIndex].Cells[1].Value.ToString();
                string Password = dgvDB.Rows[e.RowIndex].Cells[2].Value.ToString();
                FormChange formChange_dialog = new FormChange();
                formChange_dialog.Select(Service, Password, id);
                if (formChange_dialog.ShowDialog() == DialogResult.OK)
                {
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("You`a not in admin mode!");
                adminModeToolStripMenuItem_Click(sender, e);

            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete:\n" + "Service: " + dgvDB.Rows[dgvDB.CurrentCellAddress.Y].Cells[1].Value.ToString(), "Delete Message", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                QueriesDB.Del(dgvDB.Rows[dgvDB.CurrentCellAddress.Y].Cells[0].Value.ToString());
                MessageBox.Show("Data has been deleted!");
                Refresh();
            }
            else
            {
                MessageBox.Show("Data not has been deleted!");
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void adminModeToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void FindBox_TextChanged(object sender, EventArgs e)
        {
            if (FindBox.TextLength >= 4 && FindBox.Text != "Search")
            {
                dgvDB.DataSource = QueriesDB.Find(FindBox.Text);
                foreach (DataGridViewColumn column in dgvDB.Columns.Cast<DataGridViewColumn>().ToList())
                {
                    if (column.Name == "password_hash")
                    {
                        dgvDB.DataSource = DecryptPasswordDB((DataTable)dgvDB.DataSource);
                        if (dgvDB.Rows.Count > 0)
                            RemoveColumns((DataTable)dgvDB.DataSource);
                    }
                }
            }
            if (FindBox.TextLength == 0 && FindBox.Text != "Search")
                Refresh();
        }

        private void FindBox_Enter(object sender, EventArgs e)
        {
            if (FindBox.Text == "Search")
            {
                FindBox.Text = "";
                FindBox.ForeColor = Color.Black;
            }


        }

        private void FindBox_Leave(object sender, EventArgs e)
        {
            if (FindBox.Text == "")
            {
                FindBox.Text = "Search";
                FindBox.ForeColor = Color.Silver;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
