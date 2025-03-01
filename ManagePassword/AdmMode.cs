using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ManagePassword
{
    static internal class AdmMode
    {
        static public bool isAdm = true;
        static string PASSWORD = "";
        static SqlQueries Query = new SqlQueries();
        static public bool beAdm(string tbAdm)
        {
            if (PASSWORD == tbAdm)
            {
                isAdm = true;
            }
            return isAdm;
        }
        static public void hasPassword()
        {
            if(PASSWORD == "")
            {
                MessageBox.Show("Create the password!", "Information", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                CreatePasswordForm CPF_dialog = new CreatePasswordForm();
                CPF_dialog.ShowDialog();
            }
        }
        static public void RegistrPassword(string Password)
        {
            try
            {
                Query.Registr_admin(Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        static public bool AuthenticateAdmin(string Password)
        {
            try
            {
                object result = Query.Autheticate(Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }
        
    }
}
