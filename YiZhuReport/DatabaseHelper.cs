using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace YiZhuReport
{
	public class DatabaseHelper
	{
		private string _connStr;
		private SqlConnection _conn;
		private SqlCommand _cmd;
		private SqlDataAdapter _ada;

		public DatabaseHelper(string connectionString)
		{
			this._connStr = connectionString;
			this._conn = new SqlConnection(this._connStr);
			this._cmd = this._conn.CreateCommand();
			this._ada = new SqlDataAdapter(this._cmd);
		}

		public DataTable GetDataTable(string sql)
		{
			var table = new DataTable();
			this._ada.SelectCommand.CommandText = sql;
			this._ada.Fill(table);
			return table;
		}
	}
}
