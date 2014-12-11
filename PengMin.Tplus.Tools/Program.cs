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
			Console.Write("Database(s d u p):");
			var db = Console.ReadLine();
			var info = db.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var builder = new VoucherSqlBuilder(string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", info[0], info[1], info[2], info[3]));
			while (true)
			{
				Console.Write("TableName:");
				var tn = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(tn))
				{
					return;
				}
				Console.Write("Where:");
				var w = Console.ReadLine();
				var sql = builder.Build(tn, w);
				Console.WriteLine(sql);
				Console.Write("Continue>>>>>>>>>>>>>>>>>>>>>>>>>>>");
				Console.Read();
			}
		}
	}
}
