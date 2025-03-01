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
        SqlQueries sql;
        public FormAdd()
        {
            InitializeComponent();
            sql = new SqlQueries();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(tbServiceAdd.Text != "" && tbPasswordAdd.Text != "")
            {
            sql.Insert(tbServiceAdd.Text, tbPasswordAdd.Text);
            } else if(tbServiceAdd.Text == "" && tbPasswordAdd.Text == "")
            {
                MessageBox.Show("text boxes Service and/or Password never equal null!", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
