using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus.History
{
	/// <summary>
	/// 导出历史管理器
	/// </summary>
	class HistoryManager
	{
		/// <summary>
		/// 获取导出操作日期
		/// </summary>
		/// <returns>日期列表</returns>
		public IEnumerable<string> GetHead()
		{
			const string sql = "select name from Excel2TplusHistory";
			var helper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			helper.Open();
			using (var rd = helper.Reader(sql))
			{
				var list = new List<string>();
				while (rd.Read())
				{
					list.Add(rd[0].ToString());
				}
				rd.Close();
				return list;
			}
		}
		/// <summary>
		/// 设置导出历史
		/// </summary>
		/// <typeparam name="TEntity">单据类型</typeparam>
		/// <param name="list">单据集合</param>
		public void Set<TEntity>(IEnumerable<TEntity> list, string voucher) where TEntity : Entity
		{
			const string sql = "insert into Excel2TplusHistory values(@dt,@type,@xml,@name)";
			string xml;
			var elType = CommonFunction.GetElementType(list.GetType());
			if (elType != null && elType != typeof(TEntity))
			{
				var listType = typeof(List<>);
				var tempType = listType.MakeGenericType(elType);
				var tempList = Activator.CreateInstance(tempType);
				var tempListType = tempList.GetType();
				tempListType.GetMethod("AddRange").Invoke(tempList, new object[] { list });
				xml = CommonFunction.XmlSerializer(tempListType.GetMethod("ToArray").Invoke(tempList, null), tempListType).ToString();
			}
			else
			{
				xml = CommonFunction.XmlSerializer(list.ToArray()).ToString();
			}
			var dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
			var helper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			helper.Open();
			helper.Execute(sql, new[]
			{
				new SqlParameter("@dt",dt),
				new SqlParameter("@type",elType.FullName), 
				new SqlParameter("@xml",xml),
				new SqlParameter("@name",voucher+" "+dt), 
			});
			helper.Close();
		}
		/// <summary>
		/// 获取导出历史
		/// </summary>
		/// <typeparam name="TEntity">单据类型</typeparam>
		/// <param name="name">历史记录名称</param>
		/// <returns>单据集合</returns>
		public IEnumerable<TEntity> Get<TEntity>(string name) where TEntity : Entity
		{
			const string sql = "select * from Excel2TplusHistory where name=@name";
			var helper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			helper.Open();
			using (var rd = helper.Reader(sql, new SqlParameter("@name", name)))
			{
				IEnumerable<TEntity> list = null;
				if (rd.Read())
				{
					var typeStr = rd["type"].ToString();
					var elType = Type.GetType(typeStr);
					var collType = typeof(List<>);
					var listType = collType.MakeGenericType(elType);
					list = CommonFunction.XmlDeserialize<IEnumerable<TEntity>>(new System.IO.StringReader(rd["xml"].ToString()), listType);
				}
				rd.Close();
				return list;
			}
		}
	}
}
