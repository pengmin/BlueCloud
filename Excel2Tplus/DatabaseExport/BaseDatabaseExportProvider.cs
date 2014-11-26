using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;
using System.Data.Common;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus.DatabaseExport
{
	abstract class BaseDatabaseExportProvider<TEntity> : IDatabaseExportProvider<TEntity> where TEntity : Entity
	{
		protected string Prefix;//单据编码前缀
		protected int Serialno = 0;//单据编码起始编号
		protected int Length;//单据编号长度
		protected int Code;//明细的编号

		/// <summary>
		/// 单据中文名称
		/// </summary>
		protected abstract string VoucherName { get; }
		/// <summary>
		/// 单据表名称
		/// </summary>
		protected abstract string VoucherTable { get; }

		protected BaseDatabaseExportProvider()
		{
			Prefix = string.Empty;
			Serialno = 0;
			Length = 0;
			Code = 0;
		}

		public IEnumerable<string> Export(IEnumerable<TEntity> list)
		{
			if (CommonHelper.GetElementType(list.GetType()) != typeof(TEntity))
			{
				throw new Exception("单据类型是" + VoucherName + "类型");
			}
			var msgList = new List<string>();
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			var id = Guid.Empty;//单据主表id
			string 单据编号 = null;//单据编号

			Prefix = TplusDatabaseHelper.Instance.GetVoucherCodePrefix(VoucherName, out Length);
			Serialno = TplusDatabaseHelper.Instance.GetMaxSerialno(VoucherTable, Length);
			foreach (var item in list)
			{
				if (TplusDatabaseHelper.Instance.ExistVoucher(item.单据编号, VoucherTable))
				{
					msgList.Add("单据编码：" + item.单据编号 + "已存在");
					continue;
				}

				Tuple<string, IEnumerable<DbParameter>> sqlInfo;
				if (单据编号 != item.单据编号)
				{
					单据编号 = item.单据编号;
					sqlInfo = BuildMainInsertSql(item, out id);
					sqlList.Add(new Tuple<string, IEnumerable<DbParameter>>(BuildSql(sqlInfo), sqlInfo.Item2));
				}
				ReCalculation(item);
				sqlInfo = BuildDetailInsertSql(item, id);
				sqlList.Add(new Tuple<string, IEnumerable<DbParameter>>(BuildSql(sqlInfo), sqlInfo.Item2));
			}

			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			sh.Open();
			msgList.Add(sh.Execute(sqlList).ToString());
			sh.Close();

			return msgList;
		}

		/// <summary>
		/// 构造请购单主表的插入sql
		/// </summary>
		/// <param name="obj">请购单对象</param>
		/// <param name="id">id</param>
		/// <returns>sql信息</returns>
		protected abstract Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(TEntity obj, out Guid id);
		/// <summary>
		/// 构造请购单明细表的插入sql
		/// </summary>
		/// <param name="obj">请购单对象</param>
		/// <param name="pid">主表id</param>
		/// <returns>sql信息</returns>
		protected abstract Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(TEntity obj, Guid pid);

		private static string BuildSql(Tuple<string, IEnumerable<DbParameter>> sqlInfo)
		{
			var sql1 = "INSERT INTO " + sqlInfo.Item1 + "(";
			var sql2 = " VALUES(";
			foreach (var param in sqlInfo.Item2)
			{
				sql1 += param.ParameterName.TrimStart('@') + ",";
				sql2 += param.ParameterName + ",";
			}
			sql1 = sql1.TrimEnd(',') + ")";
			sql2 = sql2.TrimEnd(',') + ")";
			return sql1 + sql2 + ";";
		}
		private static void ReCalculation(Entity obj)
		{
			if (!obj.UseBookPrice) return;
			int i;
			decimal m;
			var 数量 = int.TryParse(obj.数量, out i) ? i : i;
			var 税率 = decimal.TryParse(obj.税率, out m) ? m : m;
			var 含税单价 = obj.BookPrice;
			var 单价 = 含税单价 / ((100 + 税率) / 100);
			var 金额 = 单价 * 数量;
			var 含税金额 = 数量 * 含税单价;
			var 税额 = 含税金额 - 金额;
			obj.单价 = 单价.ToString();
			obj.含税单价 = 含税单价.ToString();
			obj.金额 = 金额.ToString();
			obj.税额 = 税额.ToString();
			obj.含税金额 = 含税金额.ToString();
		}
	}
}
