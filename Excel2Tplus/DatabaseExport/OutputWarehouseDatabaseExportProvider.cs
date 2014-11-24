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
	/// 销售出库单数据库导出提供程序
	/// </summary>
	class OutputWarehouseDatabaseExportProvider : IDatabaseExportProvider
	{
		private static string prefix;//单据编码前缀
		private static int serialno = 0;//单据编码起始编号
		private static int length;//单据编号长度

		public OutputWarehouseDatabaseExportProvider()
		{
			prefix = string.Empty;
			serialno = 0;
			length = 0;
		}

		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entities.Entity
		{
			if (CommonHelper.GetElementType(list.GetType()) != typeof(OutputWarehouse))
			{
				throw new Exception("单据类型不是销售出库单类型");
			}
			var msgList = new List<string>();
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			Guid id = Guid.Empty;//单据主表id
			string code = null;//单据编号

			prefix = TplusDatabaseHelper.Instance.GetVoucherCodePrefix("销售出库单", out length);
			serialno = TplusDatabaseHelper.Instance.GetMaxSerialno("ST_RDRecord", length);
			foreach (var item in list.Cast<OutputWarehouse>())
			{
				if (TplusDatabaseHelper.Instance.ExistVoucher(item.单据编号, "Pu_PurchaseRequisition"))
				{
					msgList.Add("单据编码：" + item.单据编号 + "已存在");
					continue;
				}
				if (code != item.单据编号)
				{
					code = item.单据编号;
					sqlList.Add(BuildMainInsertSql(item, out id));
				}
				sqlList.Add(BuildDetailInsertSql(item, id));
			}

			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			sh.Open();
			msgList.Add(sh.Execute(sqlList).ToString());
			sh.Close();

			return msgList;
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(OutputWarehouse obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into ST_RDRecord(createdtime,id,rdDirectionFlag,voucherdate,code,idwarehouse,idpartner,iddepartment,priuserdefdecm1,pubuserdefnvc1,pubuserdefnvc2,pubuserdefnvc3,voucherState)";
			sql += " values(@createdtime,@id,@rdDirectionFlag,@voucherdate,@code,@idwarehouse,@idpartner,@iddepartment,@priuserdefdecm1,@pubuserdefnvc1,@pubuserdefnvc2,@pubuserdefnvc3,@voucherState);";
			decimal cy;//抽佣比率
			var ps = new DbParameter[]
			{
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 
				new SqlParameter("@id",id),
				new SqlParameter("@rdDirectionFlag",false),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")), 
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?prefix+(++serialno).ToString().PadLeft(length,'0'):obj.单据编号),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)), 
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)),
				new SqlParameter("@priuserdefdecm1",decimal.TryParse(obj.抽佣比率,out cy)?cy:cy),
				new SqlParameter("@pubuserdefnvc1",obj.部门), 
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审"))
				
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(OutputWarehouse obj, Guid pid)
		{
			var sql = "insert into ST_RDRecord_b(createdtime,id,idRDRecordDTO,idinventory,idunit,quantity,price,amount,salePrice,taxSalePrice,saleAmount,origTax,origTaxSaleAmount,priuserdefnvc1,pubuserdefnvc1,pubuserdefnvc2,pubuserdefnvc3,pubuserdefnvc4,pubuserdefdecm1,pubuserdefdecm2,pubuserdefdecm3,pubuserdefdecm4)";
			sql += " values(@createdtime,@id,@idRDRecordDTO,@idinventory,@idunit,@quantity,@price,@amount,@salePrice,@taxSalePrice,@saleAmount,@origTax,@origTaxSaleAmount,@priuserdefnvc1,@pubuserdefnvc1,@pubuserdefnvc2,@pubuserdefnvc3,@pubuserdefnvc4,@pubuserdefdecm1,@pubuserdefdecm2,@pubuserdefdecm3,@pubuserdefdecm4);";
			decimal amount;//金额
			var ps = new DbParameter[]
			{
				new SqlParameter("@createdtime",DateTime.Now), 
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@idRDRecordDTO",pid), 
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@price",obj.UseBookPrice?obj.BookPrice:obj.BillPrice),
				new SqlParameter("@amount",decimal.TryParse(obj.成本金额,out amount)?amount:0m),
				new SqlParameter("@salePrice",decimal.TryParse(obj.售价,out amount)?amount:0m),
				new SqlParameter("@taxSalePrice",decimal.TryParse(obj.含税售价,out amount)?amount:0m),
				new SqlParameter("@saleAmount",decimal.TryParse(obj.销售金额,out amount)?amount:0m),
				new SqlParameter("@origTax",decimal.TryParse(obj.税额,out amount)?amount:0m),
				new SqlParameter("@origTaxSaleAmount",decimal.TryParse(obj.含税销售金额,out amount)?amount:0m),
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
