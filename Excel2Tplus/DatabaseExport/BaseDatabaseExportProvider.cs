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
	abstract class BaseDatabaseExportProvider<TEntity> : IDatabaseExportProvider<TEntity> where TEntity : Entity, new()
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
			var checkMsgs = CheckVoucher(list);
			if (checkMsgs.Any())
			{
				return checkMsgs;
			}
			if (CommonFunction.GetElementType(list.GetType()) != typeof(TEntity))
			{
				throw new Exception("单据类型是" + VoucherName + "类型");
			}
			var msgList = new List<string>();
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			var id = Guid.Empty;//单据主表id
			string 单据编号 = null;//单据编号
			TEntity main = null;//当前主记录
			List<TEntity> details = null;//当前子记录

			Prefix = TplusDatabaseHelper.Instance.GetVoucherCodePrefix(VoucherName, out Length);
			Serialno = TplusDatabaseHelper.Instance.GetMaxSerialno(VoucherTable, Length);
			foreach (var item in list)
			{
				if (TplusDatabaseHelper.Instance.ExistVoucher(item.单据编号, VoucherTable))
				{
					msgList.Add("单据编码：" + item.单据编号 + "已存在");
					break;
				}
				IEnumerable<string> ckMsg;//可导入验证结果信息
				if (!CanExport(item, out ckMsg))
				{
					msgList.AddRange(ckMsg);
					break;
				}

				if (单据编号 != item.单据编号)//新记录不是当前记录
				{
					if (main != null && details != null && details.Count > 0)//已有当前记录
					{
						id = Guid.NewGuid();//主记录id
						//生成子记录sql
						foreach (var detail in details)
						{
							sqlList.AddRange(BuildDetailInsertSql(detail, id).Select(i => new Tuple<string, IEnumerable<DbParameter>>(BuildSql(i), i.Item2)));
						}
						//生成主记录sql
						Collect(main, details);//统计子记录
						var sqlInfo = BuildMainInsertSql(main, id);
						sqlList.Add(new Tuple<string, IEnumerable<DbParameter>>(BuildSql(sqlInfo), sqlInfo.Item2));
					}
					main = Entity.Copy(item);
					details = new List<TEntity>();

					单据编号 = item.单据编号;
				}
				ReCalculation(item);
				ReAmount(item);
				details.Add(item);
			}
			if (main != null && details != null && details.Count > 0)//已有当前记录
			{
				id = Guid.NewGuid();//主记录id
				//生成子记录sql
				foreach (var detail in details)
				{
					sqlList.AddRange(BuildDetailInsertSql(detail, id).Select(i => new Tuple<string, IEnumerable<DbParameter>>(BuildSql(i), i.Item2)));
				}
				//生成主记录sql
				Collect(main, details);//统计子记录
				var sqlInfo = BuildMainInsertSql(main, id);
				sqlList.Add(new Tuple<string, IEnumerable<DbParameter>>(BuildSql(sqlInfo), sqlInfo.Item2));
			}
			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			var other = OtherSql();
			if (other != null)
			{
				sqlList.AddRange(other);
			}
			sh.Open();
			msgList.Add(sh.Execute(sqlList).ToString());
			sh.Close();

			return msgList;
		}
		/// <summary>
		/// 其他sql
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerable<Tuple<string, IEnumerable<DbParameter>>> OtherSql()
		{
			return null;
		}

		/// <summary>
		/// 构造请购单主表的插入sql
		/// </summary>
		/// <param name="obj">请购单对象</param>
		/// <param name="id">id</param>
		/// <returns>sql信息</returns>
		protected abstract Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(TEntity obj, Guid id);
		/// <summary>
		/// 构造请购单明细表的插入sql
		/// </summary>
		/// <param name="obj">请购单对象</param>
		/// <param name="pid">主表id</param>
		/// <returns>sql信息</returns>
		protected abstract IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(TEntity obj, Guid pid);
		/// <summary>
		/// 单据能否导入到数据库中。
		/// </summary>
		/// <param name="obj">要导入的单据</param>
		/// <param name="msgs">验证结果信息</param>
		/// <returns></returns>
		protected abstract bool CanExport(TEntity obj, out IEnumerable<string> msgs);

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
			obj.单价 = decimal.Round(单价, 2).ToString();
			obj.含税单价 = decimal.Round(含税单价, 2).ToString();
			obj.金额 = decimal.Round(金额, 2).ToString();
			obj.税额 = decimal.Round(税额, 2).ToString();
			obj.含税金额 = decimal.Round(含税金额, 2).ToString();
		}
		/// <summary>
		/// 根据退货日期重置数量、金额、含税金额的正负数
		/// </summary>
		/// <param name="obj"></param>
		private static void ReAmount(Entity obj)
		{
			int i;
			decimal m;
			var 数量 = int.TryParse(obj.数量, out i) ? i : i;
			var 金额 = decimal.TryParse(obj.金额, out m) ? m : m;
			var 含税金额 = decimal.TryParse(obj.含税金额, out m) ? m : m;
			var 税额 = decimal.TryParse(obj.税额, out m) ? m : m;
			if (string.IsNullOrEmpty(obj.退货日期))
			{
				obj.金额 = decimal.Round(金额, 2).ToString();
				obj.含税金额 = decimal.Round(含税金额, 2).ToString();
			}
			else
			{
				obj.数量 = (-数量).ToString();
				obj.金额 = decimal.Round(-金额, 2).ToString();
				obj.含税金额 = decimal.Round(-含税金额, 2).ToString();
				obj.税额 = (-税额).ToString();
			}
		}
		/// <summary>
		/// 统计主记录数据
		/// </summary>
		/// <param name="main">主记录</param>
		/// <param name="details">子记录</param>
		private static void Collect(TEntity main, IEnumerable<TEntity> details)
		{
			int i;
			decimal m;
			var query = 0;
			var price = 0m;
			var taxPrice = 0m;
			var amount = 0m;
			var taxRate = 0m;
			var taxAmount = 0m;
			foreach (var detail in details)
			{
				int.TryParse(detail.数量, out i);
				query += i;
				decimal.TryParse(detail.单价, out m);
				price += m;
				decimal.TryParse(detail.含税单价, out m);
				taxPrice += m;
				decimal.TryParse(detail.金额, out m);
				amount += m;
				decimal.TryParse(detail.税额, out m);
				taxRate += m;
				decimal.TryParse(detail.含税金额, out m);
				taxAmount += m;
			}
			main.数量 = query.ToString();
			main.单价 = (price / details.Count()).ToString();
			main.含税单价 = (taxPrice / details.Count()).ToString();
			main.金额 = amount.ToString();
			main.税额 = taxRate.ToString();
			main.含税金额 = taxAmount.ToString();
		}

		private static IEnumerable<string> CheckVoucher(IEnumerable<TEntity> list)
		{
			var msgs = new List<string>();
			string voucherCode = null;
			var hasTuihuo = false;//有退货日期，则所有明细都要有退货日期
			var gys = "";//供应商
			foreach (var entity in list)
			{
				if (voucherCode != entity.单据编号)
				{
					voucherCode = entity.单据编号;
					hasTuihuo = false;
					gys = entity.供应商;
				}
				if (!string.IsNullOrWhiteSpace(entity.退货日期))
				{
					hasTuihuo = true;
				}
				if (hasTuihuo && string.IsNullOrWhiteSpace(entity.退货日期))
				{
					msgs.Add("单据[" + entity.单据编号 + "]不是所有行都有退货日期，不允许导入");
					return msgs;
				}
				if (gys != entity.供应商)
				{
					msgs.Add("单据[" + entity.单据编号 + "]供应商不一致，不允许导入");
					return msgs;
				}
			}
			return msgs;
		}
	}
}
