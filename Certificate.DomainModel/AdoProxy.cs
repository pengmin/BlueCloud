using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Certificate.DomainModel
{
	public class AdoProxy : IDisposable
	{
		private SqlConnection _conn = null;
		private SqlCommand _cmd = null;
		private SqlDataAdapter _ada = null;

		public AdoProxy(string connStr)
		{
			this._conn = new SqlConnection(connStr);
			this._cmd = this._conn.CreateCommand();
			this._ada = new SqlDataAdapter();
			this._ada.SelectCommand = this._cmd;
		}
		public void Open()
		{
			if (this._conn != null && this._conn.State != ConnectionState.Open)
			{
				this._conn.Open();
			}
		}
		public void Close()
		{
			if (this._conn != null && this._conn.State != ConnectionState.Closed)
			{
				this._conn.Close();
			}
		}
		public void ExecuteNonQuery(string sql)
		{
			this._cmd.CommandText = sql;
			this._cmd.ExecuteNonQuery();
		}
		public object ExecuteScalar(string sql)
		{
			this._cmd.CommandText = sql;
			return this._cmd.ExecuteScalar();
		}
		public DataTable DataTableExecute(string sql)
		{
			var data = new DataTable();
			this._cmd.CommandText = sql;
			this._ada.Fill(data);
			return data;
		}
		public SqlDataReader ExecuteReader(string sql)
		{
			this._cmd.CommandText = sql;
			return this._cmd.ExecuteReader();
		}

		public void Dispose()
		{
			this.Close();
		}
	}
}
