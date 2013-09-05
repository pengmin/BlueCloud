using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace YiZhuReport
{
	public class DomainModel
	{
		private DatabaseHelper _helper;

		public DomainModel(DatabaseHelper helper)
		{
			this._helper = helper;
		}
		public DataTable GetMainView(params string[] filter)
		{
			//以下获取满足查询条件的主件号
			var sql =
@"SELECT a.QAA001 FROM dbo.SGMQAA AS a
LEFT JOIN dbo.TPADEA AS b ON a.QAA001=b.DEA001
WHERE b.DEA001 IN({0}) OR b.DEA002 IN({0})";
			var table = this._helper.GetDataTable(string.Format(sql, "'" + string.Join("','", filter) + "'"));
			//以下获取主件的总表信息
			var mainAndLeaf = new Dictionary<string, string>();
			foreach (DataRow row in table.Rows)
			{
				mainAndLeaf.Add(row[0].ToString(), "'" + string.Join("','", this.GetLeaf(row[0].ToString())) + "'");
			}
			return this.GetMainTable(mainAndLeaf);
		}
		/// <summary>
		/// 获取尾阶子件
		/// </summary>
		/// <param name="main">主件号</param>
		/// <returns>尾阶子键集合</returns>
		private IEnumerable<string> GetLeaf(string main)
		{
			var leaf = new List<string>();
			var childTable = this._helper.GetDataTable("SELECT QAB001,QAB003 FROM dbo.SGMQAB");
			this.FindLeaf(childTable, main, leaf);
			return leaf;
		}
		/// <summary>
		/// 查找指定主件的尾件
		/// </summary>
		/// <param name="childTable">子件表</param>
		/// <param name="main">主件</param>
		/// <param name="leaf">尾件集合</param>
		private void FindLeaf(DataTable childTable, string main, List<string> leaf)
		{
			var hasChild = false;
			foreach (DataRow row in childTable.Rows)
			{
				if (row[0].ToString() == main)
				{
					hasChild = true;
					this.FindLeaf(childTable, row[1].ToString(), leaf);
				}
			}
			if (!hasChild)
			{
				leaf.Add(main);
			}
		}
		/// <summary>
		/// 获取主件总表信息
		/// </summary>
		/// <param name="mainAndLeaf">主件及其尾阶子件</param>
		/// <returns>总表信息</returns>
		private DataTable GetMainTable(Dictionary<string, string> mainAndLeaf)
		{
			return null;
		}
	}
}
