using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PengMin.JiaOu.SysConfig
{
	public class AccountInfo
	{
		private const string ConnStr = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";

		public string Name { get; set; }
		public string Server { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Database { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public string GetConnectionString()
		{
			return string.Format(ConnStr, Server, Database, User, Password);
		}
	}
}
