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
	class SaleQuotationDatabaseExportProvider : IDatabaseExportProvider
	{
		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entities.Entity
		{
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			foreach (var item in list)
			{
				Guid id;
				sqlList.Add(BuildMainInsertSql(item as SaleQuotation, out id));
				sqlList.Add(BuildDetailInsertSql(item as SaleQuotation, id));
			}

			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			sh.Open();
			var r = sh.Execute(sqlList);
			sh.Close();

			return new[] { r.ToString() };
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(SaleQuotation obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into SA_SaleQuotation(id,voucherdate,code,idcustomer,iddepartment,idproject)";
			sql += " values(@id,@voucherdate,@code,@idcustomer,@iddepartment,@idproject);";
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id), 
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期)), 
				new SqlParameter("@code",obj.单据编号), 
				new SqlParameter("@idcustomer",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)), 
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)), 
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目))
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(SaleQuotation obj, Guid pid)
		{
			var sql = "insert into SA_SaleQuotation_b(id,idSaleQuotationDTO,idinventory,quantity,price)";
			sql += " values(@id,@idSaleQuotationDTO,@idinventory,@quantity,@price);";
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@idSaleQuotationDTO",pid), 
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@price",obj.UseBookPrice?obj.BookPrice:obj.BillPrice)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}
	}
}
