using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PengMin.Infrastructure
{
	/// <summary>
	/// sql操作帮助器
	/// </summary>
	public class SqlHelper
	{
		private readonly DbConnection _conn;

		public SqlHelper(string connectionString)
		{
			_conn = new SqlConnection(connectionString);
		}
		/// <summary>
		/// 打开连接
		/// </summary>
		public void Open()
		{
			if (_conn.State != System.Data.ConnectionState.Open)
			{
				_conn.Open();
			}
		}
		/// <summary>
		/// 关闭连接
		/// </summary>
		public void Close()
		{
			if (_conn.State != System.Data.ConnectionState.Closed)
			{
				_conn.Close();
			}
		}
		/// <summary>
		/// 以DataReader方式读取数据
		/// </summary>
		/// <param name="sql">要执行的sql</param>
		/// <param name="param">sql参数</param>
		/// <returns>结果读取器对象</returns>
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
		/// <summary>
		/// 执行sql语句
		/// </summary>
		/// <param name="sql">要执行的sql</param>
		/// <param name="param">sql参数</param>
		/// <returns>受影响的行数</returns>
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
		/// <summary>
		/// 以事务方式执行sql语句，返回受影响的行数
		/// </summary>
		/// <param name="cmds">要执行的sql</param>
		/// <returns>受影响的行数，返回-1说明有错误</returns>
		public int Execute(IEnumerable<Tuple<string, IEnumerable<DbParameter>>> cmds)
		{
			using (var tran = _conn.BeginTransaction())
			{
				var cmd = _conn.CreateCommand();
				cmd.Transaction = tran;
				try
				{
					foreach (var c in cmds)
					{
						cmd.CommandText = c.Item1;
						cmd.Parameters.Clear();
						cmd.Parameters.AddRange(c.Item2.ToArray());
						cmd.ExecuteNonQuery();
					}
					tran.Commit();
					return cmds.Count();
				}
				catch
				{
					tran.Rollback();
					return -1;
				}
			}
		}
		/// <summary>
		/// 查询数据，以DataTable对象方式返回结果
		/// </summary>
		/// <param name="sql">查询sql</param>
		/// <param name="param">查询参数</param>
		/// <returns>查询结果</returns>
		public DataTable GetDataTable(string sql, params DbParameter[] param)
		{
			var cmd = _conn.CreateCommand();
			cmd.CommandText = sql;
			cmd.Parameters.AddRange(param);

			DbDataAdapter adapter = new SqlDataAdapter();
			adapter.SelectCommand = cmd;
			var dt = new DataTable();

			adapter.Fill(dt);
			return dt;
		}
		/// <summary>
		/// 获取查询结果的第一行第一列的值
		/// </summary>
		/// <param name="sql">查询sql</param>
		/// <param name="param">查询参数</param>
		/// <returns>查询结果</returns>
		public object Scalar(string sql, params DbParameter[] param)
		{
			var cmd = _conn.CreateCommand();
			cmd.CommandText = sql;
			if (param.Length > 0)
			{
				cmd.Parameters.AddRange(param);
			}
			return cmd.ExecuteScalar();
		}
	}
}
