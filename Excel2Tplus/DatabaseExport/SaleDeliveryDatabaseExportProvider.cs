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
	/// <summary>
	/// 销货单数据库导出提供程序
	/// </summary>
	class SaleDeliveryDatabaseExportProvider : IDatabaseExportProvider
	{
		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entities.Entity
		{
			if (CommonHelper.GetElementType(list.GetType()) != typeof(SaleDelivery))
			{
				throw new Exception("单据类型不是销货单类型");
			}

			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			Guid id = Guid.Empty;//单据主表id
			string code = null;//单据编号
			foreach (var item in list.Cast<SaleDelivery>())
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

		private static Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(SaleDelivery obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into SA_SaleDelivery(id,voucherdate,code,idwarehouse,idcustomer,iddepartment,priuserdefdecm1,pubuserdefnvc1,pubuserdefnvc2,pubuserdefnvc3)";
			sql += " values(@id,@voucherdate,@code,@idwarehouse,@idcustomer,@iddepartment,@priuserdefdecm1,@pubuserdefnvc1,@pubuserdefnvc2,@pubuserdefnvc3);";
			decimal d;
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id), 
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期)), 
				new SqlParameter("@code",obj.单据编号), 
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idcustomer",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)), 
				new SqlParameter("@priuserdefdecm1",decimal.TryParse(obj.抽佣比率,out d)?d:d), 
				new SqlParameter("@pubuserdefnvc1",obj.部门), 
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(SaleDelivery obj, Guid pid)
		{
			var sql = "insert into SA_SaleDelivery_b(id,idSaleDeliveryDTO,idinventory,idunit,quantity,discountPrice,costAmount,Retailprice,TaxPrice,DiscountAmount,origTax,TaxAmount,priuserdefnvc1,pubuserdefnvc1,pubuserdefnvc2,pubuserdefnvc3,pubuserdefnvc4,pubuserdefdecm1,pubuserdefdecm2,pubuserdefdecm3,pubuserdefdecm4)";
			sql += " values(@id,@idSaleDeliveryDTO,@idinventory,@idunit,@quantity,@discountPrice,@costAmount,@Retailprice,@TaxPrice,@DiscountAmount,@origTax,@TaxAmount,@priuserdefnvc1,@pubuserdefnvc1,@pubuserdefnvc2,@pubuserdefnvc3,@pubuserdefnvc4,@pubuserdefdecm1,@pubuserdefdecm2,@pubuserdefdecm3,@pubuserdefdecm4);";
			decimal amount;
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@idSaleDeliveryDTO",pid), 
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@discountPrice",obj.UseBookPrice?obj.BookPrice:obj.BillPrice),
				new SqlParameter("@costAmount",decimal.TryParse(obj.成本金额,out amount)?amount:0m),
				new SqlParameter("@Retailprice",decimal.TryParse(obj.售价,out amount)?amount:0m),
				new SqlParameter("@TaxPrice",decimal.TryParse(obj.含税售价,out amount)?amount:0m),
				new SqlParameter("@DiscountAmount",decimal.TryParse(obj.销售金额,out amount)?amount:0m),
				new SqlParameter("@origTax",decimal.TryParse(obj.税额,out amount)?amount:0m),
				new SqlParameter("@TaxAmount",decimal.TryParse(obj.含税销售金额,out amount)?amount:0m),
				new SqlParameter("@priuserdefnvc1",obj.销售订单号),
				new SqlParameter("@pubuserdefnvc1",obj.物流名称单号),
				new SqlParameter("@pubuserdefnvc2",obj.发货信息),
				new SqlParameter("@pubuserdefnvc3",obj.客户收货信息),
				new SqlParameter("@pubuserdefnvc4",obj.平台单号),
				new SqlParameter("@pubuserdefdecm1",decimal.TryParse(obj.满减活动,out amount)?amount:0m),
				new SqlParameter("@pubuserdefdecm2",decimal.TryParse(obj.抵用券,out amount)?amount:0m),
				new SqlParameter("@pubuserdefdecm3",decimal.TryParse(obj.代收运费,out amount)?amount:0m),
				new SqlParameter("@pubuserdefdecm4",decimal.TryParse(obj.抽佣,out amount)?amount:0m)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}
	}
}
