using ManagePassword.Model;
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
		ManagePasswordForm mp_dialog;
		public AdminForm(ManagePasswordForm parent)
		{
			InitializeComponent();
			mp_dialog = parent;
		}

		private void btnEnterToAdminMode_Click(object sender, EventArgs e)
		{
			if(tbAdmin.Text != "" && !Model.AdmMode.HaveAdm())
			{
				MessageBox.Show("You don`t have a password!");
			}
			else if (Model.AdmMode.AuthenticateAdm(tbAdmin.Text))
			{
				mp_dialog.Refresh();
				//mp_dialog.SavePassword();
				//mp_dialog.HidePassword();
				tbAdmin.Clear();
				this.Close();
			}
			else if (tbAdmin.Text == "")
			{
				MessageBox.Show("text box Admin Password has been not euqal null!");
			}
		}
		private void btnExit_Click(object sender, EventArgs e)
		{
			if (Model.AdmMode.isAdm)
			{
				Model.AdmMode.ClearMasterPassword();
				mp_dialog.Refresh();
				
				this.Close();
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnReg_Click(object sender, EventArgs e)
		{
			if (tbAdmin.Text != "")
			{
				Model.AdmMode.RegistrAdm(tbAdmin.Text);
			}
		}

		private void btnDel_Click(object sender, EventArgs e)
		{
			if (Model.AdmMode.isAdm && tbAdmin.Text != "")
			{
				Model.AdmMode.DeleteAdm(tbAdmin.Text);
				mp_dialog.Refresh();
			}
		}
	}
}
