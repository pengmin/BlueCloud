using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus.DatabaseExport
{
	internal class PurchaseArrivalDatabaseExportProvider : IDatabaseExportProvider
	{
		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entities.Entity
		{
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			foreach (var item in list)
			{
				Guid id;
				sqlList.Add(BuildMainInsertSql(item as PurchaseArrival, out id));
				sqlList.Add(BuildDetailInsertSql(item as PurchaseArrival, id));
			}

			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			sh.Open();
			var r = sh.Execute(sqlList);
			sh.Close();

			return new[] { r.ToString() };
		}

		private Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(PurchaseArrival obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into PU_PurchaseArrival(id,voucherdate,code,idpartner,iddepartment,idproject,pubuserdefnvc1,pubuserdefnvc2,pubuserdefnvc3,idwarehouse)";
			sql += " values(@id,@voucherdate,@code,@idpartner,@iddepartment,@idproject,@pubuserdefnvc1,@pubuserdefnvc2,@pubuserdefnvc3,@idwarehouse);";
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id), 
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期)), 
				new SqlParameter("@code",obj.单据编号), 
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByCode(obj.供应商)),
 				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)), 
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)), 
				new SqlParameter("@pubuserdefnvc1",obj.部门), 
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}

		private Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(PurchaseArrival obj, Guid pid)
		{
			var sql = "insert into PU_PurchaseArrival_b(id,idPurchaseArrivalDTO,idinventory,quantity,price)";
			sql += " values(@id,@idPurchaseArrivalDTO,@idinventory,@quantity,@price);";
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@idPurchaseArrivalDTO",pid), 
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@price",obj.UseBookPrice?obj.BookPrice:obj.BillPrice)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}
	}
}
