using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus.DatabaseExport
{
	/// <summary>
	/// 采购订单数据库导出提供程序
	/// </summary>
	class PurchaseOrderDatabaseExportProvider : IDatabaseExportProvider
	{
		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entity
		{
			if (CommonHelper.GetElementType(list.GetType()) != typeof(PurchaseOrder))
			{
				throw new Exception("单据类型不是采购订单类型");
			}

			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			Guid id = Guid.Empty;//单据主表id
			string code = null;//单据编号
			foreach (var item in list.Cast<PurchaseOrder>())
			{
				if (code != item.单据编号)
				{
					code = item.单据编号;
					sqlList.Add(BuildMainInsertSql(item, out id));
				}
				sqlList.Add(BuildDetailInsertSql(item, id));
			}

			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			sh.Open();
			var r = sh.Execute(sqlList);
			sh.Close();

			return new[] { r.ToString() };
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(PurchaseOrder obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into PU_PurchaseOrder(id,voucherdate,code,idwarehouse,idpartner,iddepartment,idproject,pubuserdefnvc1,pubuserdefnvc2)";
			sql += " values(@id,@voucherdate,@code,@idwarehouse,@idpartner,@iddepartment,@idproject,@pubuserdefnvc1,@pubuserdefnvc2);";
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id), 
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期)), 
				new SqlParameter("@code",obj.单据编号), 
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
 				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByCode(obj.供应商)), 
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)), 
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)), 
				new SqlParameter("@pubuserdefnvc1",obj.部门), 
				new SqlParameter("@pubuserdefnvc2",obj.业务员)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(PurchaseOrder obj, Guid pid)
		{
			var sql = "insert into PU_PurchaseOrder_b(id,idPurchaseOrderDTO,idinventory,idunit,quantity,discountPrice,taxRate,taxPrice,discountAmount,taxAmount)";
			sql += " values(@id,@idPurchaseOrderDTO,@idinventory,@idunit,@quantity,@discountPrice,@taxRate,@taxPrice,@discountAmount,@taxAmount);";
			double tr;//税率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@idPurchaseOrderDTO",pid), 
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@discountPrice",obj.UseBookPrice?obj.BookPrice:obj.BillPrice),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@taxPrice",obj.含税单价),
				new SqlParameter("@discountAmount",obj.金额),
				//todo:税额没找到对应的字段
				new SqlParameter("@taxAmount",obj.含税金额)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}
	}
}
