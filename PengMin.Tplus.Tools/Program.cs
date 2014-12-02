using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PengMin.Tplus.Tools
{
	class Program
	{
		static void Main(string[] args)
		{
			var builder = new VoucherSqlBuilder("Data Source=.;Initial Catalog=YYTPRODemoDB998;User ID=sa;Password=LY123456");
			Console.Write("TableName:");
			var tn = Console.ReadLine();
			Console.Write("Where:");
			var w = Console.ReadLine();
			var sql = builder.Build(tn, w);
			Console.Write(sql);
			Console.Read();
		}
	}
}
