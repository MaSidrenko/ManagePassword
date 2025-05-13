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
using ManagePassword.Model;
using Npgsql;

namespace ManagePassword
{
	public partial class ManagePasswordForm : Form
	{
		AdminForm adminMode_dialog = null;
		DateTime time;
		public List<PasswordRecrods> recrods = Model.QueriesDB.Refresh();
		public ManagePasswordForm()
		{
			InitializeComponent();
			adminMode_dialog = new AdminForm(this);
			this.CenterToScreen();
			Refresh();
			time = DateTime.Now;
			dgvDB.Columns["Id"].Visible = false;
			dgvDB.Columns["Password"].Visible = false;
			sQLiteToolStripMenuItem.Checked = true;
		}
		public void Refresh()
		{
			recrods = Model.QueriesDB.Refresh();
			dgvDB.DataSource = recrods;
			if (AdmMode.isAdm)
			{
				dgvDB.Columns["Password"].Visible = true;
			}
			else if (!AdmMode.isAdm && dgvDB.Columns["Password"] != null)
			{
				dgvDB.Columns["Password"].Visible = false;
			}
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
			if (!Model.AdmMode.SetedAdmPassword)
			{
				MessageBox.Show("Type Master Password!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				adminMode_dialog.Show();
				MessageBox.Show("Click second time on Add!");
			}
			else if (Model.AdmMode.SetedAdmPassword)
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
			if (AdmMode.SetedAdmPassword)
			{
				string id = dgvDB.Rows[e.RowIndex].Cells[0].Value.ToString();
				string Service = dgvDB.Rows[e.RowIndex].Cells[1].Value.ToString();
				string Password = recrods[e.RowIndex].Password;
				FormChange formChange_dialog = new FormChange();
				formChange_dialog.Select(Service, Password, id);
				if (formChange_dialog.ShowDialog() == DialogResult.OK)
				{
					Refresh();
				}
			}
			else
			{
				MessageBox.Show("You`a not in admin mode!");
				adminModeToolStripMenuItem_Click(sender, e);
			}

		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Delete:\n" + "Service: " + dgvDB.Rows[dgvDB.CurrentCellAddress.Y].Cells[1].Value.ToString(), "Delete Message", MessageBoxButtons.OKCancel) == DialogResult.OK)
			{
				Model.QueriesDB.Del(Convert.ToInt32(dgvDB.Rows[dgvDB.CurrentCellAddress.Y].Cells[0].Value));
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

		private void FindBox_TextChanged(object sender, EventArgs e)
		{
			if (FindBox.TextLength >= 4 && FindBox.Text != "Search")
			{
				dgvDB.DataSource = Model.QueriesDB.Find(FindBox.Text);
			}
			if (FindBox.TextLength == 0 && FindBox.Text != "Search")
			{
				Refresh();
			}
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
		private void timer1_Tick(object sender, EventArgs e)
		{
			DateTime stopWatch = new DateTime();
			stopWatch = stopWatch.AddTicks(DateTime.Now.Ticks - time.Ticks);
			if (stopWatch.Minute == 10 && stopWatch.Second == 30)
			{
				if (Model.AdmMode.SetedAdmPassword)
				{
					Model.AdmMode.ClearMasterPassword();
					dgvDB.Columns["Password"].Visible = false;
					Refresh();

				}
			}
		}
		private void dgvDB_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 2)
			{
				if (e.Value != null)
				{
					e.Value = new string('*', 5);
				}
			}
		}

		private void postgreSQLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (sQLiteToolStripMenuItem.Checked)
			{
				sQLiteToolStripMenuItem.Checked = false;
			}
			Model.QueriesDB.BdMode = "Postgre";
			if(postgreSQLToolStripMenuItem.Checked)
			{
				Refresh();	
			}
		}

		private void sQLiteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (postgreSQLToolStripMenuItem.Checked)
			{
				postgreSQLToolStripMenuItem.Checked = false ;
			}
			Model.QueriesDB.BdMode = "SQLite";
			if(sQLiteToolStripMenuItem.Checked )
			{
				Refresh();
			}
		}
	}
}
