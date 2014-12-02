using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PengMin.Infrastructure;

namespace PengMin.Tplus.Tools
{
	class VoucherSqlBuilder
	{
		private readonly string _connStr;

		public VoucherSqlBuilder(string connStr)
		{
			_connStr = connStr;
		}
		public string Build(string tableName, string where = "1=1")
		{
			where = string.IsNullOrWhiteSpace(where) ? "1=1" : where;
			var db = new SqlHelper(_connStr);
			db.Open();
			var dt = db.GetDataTable("select * from " + tableName + " where " + where);
			db.Close();

			var sql = "new DbParameter[]{";
			if (dt.Rows.Count <= 0) return sql + "};";
			var row = dt.Rows[0];
			for (var i = 0; i < dt.Columns.Count; i++)
			{
				if (row[i] is DBNull) continue;
				var val = "";
				switch (dt.Columns[i].DataType.ToString())
				{
					case "System.Byte":
						val = "Convert.ToByte(" + row[i] + ")";
						break;
					case "System.Int32":
						val = "Convert.ToInt32(" + row[i] + ")";
						break;
					case "System.Double":
						val = "Convert.ToDouble(" + row[i] + ")";
						break;
					case "System.Decimal":
						val = "Convert.ToDecimal(" + row[i] + ")";
						break;
					case "System.DateTime":
						val = "Convert.ToDateTime(" + row[i] + ")";
						break;
					case "System.Guid":
						val = "new Guid(\"" + row[i] + "\")";
						break;
					default:
						val = "\"" + row[i] + "\"";
						break;
				}
				sql += "\r\nnew SqlParameter(\"@" + dt.Columns[i].ColumnName + "\"," + val + "),";
			}
			return sql + "};";
		}
	}
}
