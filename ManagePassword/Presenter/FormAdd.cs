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
        public FormAdd()
        {
            InitializeComponent();
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
                Model.QueriesDB.Insert(tbServiceAdd.Text, tbPasswordAdd.Text);
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
