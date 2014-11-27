using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PengMin.JiaOu.SysConfig
{
	public class AccountInfo
	{
		public string Name { get; set; }
		public string Server { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Database { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
