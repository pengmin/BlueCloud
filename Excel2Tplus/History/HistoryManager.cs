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
		public IEnumerable<DateTime> GetHead()
		{
			const string sql = "select datetime from Excel2TplusHistory";
			var helper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			helper.Open();
			using (var rd = helper.Reader(sql))
			{
				var list = new List<DateTime>();
				while (rd.Read())
				{
					DateTime dt;
					DateTime.TryParse(rd[0] as string, out dt);
					list.Add(dt);
				}
				rd.Close();
				return list;
			}
		}
		public void Set<TEntity>(IEnumerable<TEntity> list) where TEntity : Entity
		{
			const string sql = "insert into Excel2TplusHistory values(@dt,@xml)";
			string xml;
			var elType = CommonHelper.GetElementType(list.GetType());
			if (elType != null && elType != typeof(TEntity))
			{
				var listType = typeof(List<>);
				var tempType = listType.MakeGenericType(elType);
				var tempList = Activator.CreateInstance(tempType);
				var tempListType = tempList.GetType();
				tempListType.GetMethod("AddRange").Invoke(tempList, new object[] { list });
				xml = CommonHelper.XmlSerializer(tempListType.GetMethod("ToArray").Invoke(tempList, null), tempListType).ToString();
			}
			else
			{
				xml = CommonHelper.XmlSerializer(list.ToArray()).ToString();
			}
			var helper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			helper.Open();
			helper.Execute(sql, new[]
			{
				new SqlParameter("@dt",DateTime.Now),
				new SqlParameter("@xml",xml)
			});
			helper.Close();
		}

		public IEnumerable<TEntity> Get<TEntity>(DateTime dt) where TEntity : Entity
		{
			const string sql = "select * from Excel2TplusHistory where datetime=@dt";
			var helper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			helper.Open();
			using (var rd = helper.Reader(sql, new SqlParameter("@dt", dt)))
			{
				IEnumerable<TEntity> list = null;
				if (rd.Read())
				{
					list = CommonHelper.XmlDeserialize<IEnumerable<TEntity>>(new System.IO.StringReader(rd["xml"] as string));
				}
				rd.Close();
				return list;
			}
		}
	}
}
