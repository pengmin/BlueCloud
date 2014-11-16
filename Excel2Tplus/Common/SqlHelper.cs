using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Common
{
	class SqlHelper
	{
		private readonly DbConnection _conn;

		public SqlHelper(string connectionString)
		{
			_conn = new SqlConnection(connectionString);
		}

		public void Open()
		{
			if (_conn.State != System.Data.ConnectionState.Open)
			{
				_conn.Open();
			}
		}

		public void Close()
		{
			if (_conn.State != System.Data.ConnectionState.Closed)
			{
				_conn.Close();
			}
		}

		public DbDataReader Reader(string sql, params DbParameter[] param)
		{
			var cmd = _conn.CreateCommand();
			cmd.CommandText = sql;
			if (param != null && param.Length > 0)
			{
				cmd.Parameters.AddRange(param);
			}
			return cmd.ExecuteReader();
		}

		public int Execute(string sql, params SqlParameter[] param)
		{
			var cmd = _conn.CreateCommand();
			cmd.CommandText = sql;
			if (param != null && param.Length > 0)
			{
				cmd.Parameters.AddRange(param);
			}
			return cmd.ExecuteNonQuery();
		}
	}
}
