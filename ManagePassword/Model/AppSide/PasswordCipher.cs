using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagePassword.Model
{
	public class PasswordCipher
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Service { get; set; }
		public byte[] Password_hash { get; set; }
		public byte[] Salt { get; set; }
		public byte[] Aes_iv { get; set; }
	}
}
