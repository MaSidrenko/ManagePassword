using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagePassword.Model
{
	internal class Entry
	{
		public string Service { get; set; }
		public string Password { get; set; }
		public bool IsPasswordHiden { get; set; }
	}
}
