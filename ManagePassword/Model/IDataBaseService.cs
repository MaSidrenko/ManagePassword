using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagePassword.Model
{
	 internal interface IDataBaseService
	{
		void Insert(string open_text, string secret_text);
		DataTable Refresh();
		DataTable Find(string open_text);
		void Del(string del_id);
		void Change(string open_text, string secret_text, string change_id);
		DataTable Read(DataTable table);
	}
}
