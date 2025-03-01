﻿using System;
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
        public void Refresh()
        {
            dgvDB.DataSource = sqlQueries.Refresh();
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
            FormAdd formAdd_dialog = new FormAdd();
            if (formAdd_dialog.ShowDialog() == DialogResult.OK)
            {
                dgvDB.DataSource = sqlQueries.Refresh();
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFind formFind_dialog = new FormFind();
            formFind_dialog.GetDVG(dgvDB);
            formFind_dialog.Show();
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChange formChange_dialog = new FormChange();
            formChange_dialog.ShowDialog();
        }

        private void dgvDB_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
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
                sqlQueries.Del(dgvDB.Rows[dgvDB.CurrentCellAddress.Y].Cells[0].Value.ToString());
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
    }
}
