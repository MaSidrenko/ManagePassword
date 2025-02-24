using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagePassword
{
    static internal class AdmMode
    {
        static public bool isAdm = false;
        static string PASSWORD = "291305";
        static public bool beAdm(string tbAdm)
        {
            if (PASSWORD == tbAdm)
            {
                isAdm = true;
            }
            return isAdm;
        }
    }
}
