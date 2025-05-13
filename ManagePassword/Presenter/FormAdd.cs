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
    public partial class FormAdd : Form
    {
        ManagePasswordForm mp_dialog;
        public FormAdd()
        {
            InitializeComponent();
            mp_dialog = new ManagePasswordForm();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (tbServiceAdd.Text == "" || tbPasswordAdd.Text == "")
            {
                MessageBox.Show("text boxes Service and/or Password never equal null!", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrWhiteSpace(tbServiceAdd.Text) || string.IsNullOrWhiteSpace(tbPasswordAdd.Text))
            {
                MessageBox.Show("text boxes Service and/or Password never equal null!", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                mp_dialog.recrods = Model.QueriesDB.Insert(tbServiceAdd.Text, tbPasswordAdd.Text);
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
