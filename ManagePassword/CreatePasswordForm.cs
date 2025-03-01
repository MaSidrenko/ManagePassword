﻿using System;
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
    public partial class CreatePasswordForm : Form
    {
        public CreatePasswordForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string password = tbCreatePassword.Text;
            Cipher.SetPass(password);
            AdmMode.RegistrAdm(password);
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
