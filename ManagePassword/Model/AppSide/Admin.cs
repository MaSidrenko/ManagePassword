using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagePassword.Model.AppSide
{
	public class Admin
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string name = "Admin";
		public byte[] password_hash { get; set; }
		public byte[] salt { get; set; }
		public byte[] aes_iv { get; set; }
	}
}
