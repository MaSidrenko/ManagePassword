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
    public partial class FormFind : Form
    {
        SqlQueries sql;
        DataGridView dgv;
        public FormFind()
        {
            InitializeComponent();
            sql = new SqlQueries();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            dgv.DataSource = sql.Find(tbFindService.Text, tbFindPassword.Text);
            this.Close();
        }
        public object GetDVG(DataGridView temp_dgv)
        {
            dgv = temp_dgv;
            return dgv.DataSource;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
