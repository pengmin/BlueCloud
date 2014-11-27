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
	class PurchaseOrderDatabaseExportProvider : BaseDatabaseExportProvider<PurchaseOrder>
	{
		protected override string VoucherName
		{
			get { return "采购订单"; }
		}
		protected override string VoucherTable
		{
			get { return "PU_PurchaseOrder"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(PurchaseOrder obj, out Guid id)
		{
			id = Guid.NewGuid();

			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id),
				//new SqlParameter("@origTotalAmount",462.00),
				new SqlParameter("@iscarriedforwardin",false),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")),
				new SqlParameter("@exchangeRate",1),
				new SqlParameter("@auditor",""),
				new SqlParameter("@makerid",new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				//new SqlParameter("@origTotalTaxAmount",540.54),
				new SqlParameter("@maker","DEMO"),
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@PrintCount",Convert.ToInt32(0)),
				new SqlParameter("@accountingperiod",Convert.ToInt32(0)),
				new SqlParameter("@ReferenceCount",Convert.ToInt32(0)),
				new SqlParameter("@accountingyear",Convert.ToInt32(0)),
				//new SqlParameter("@totalAmount",462.00),
				new SqlParameter("@reviser",""),
				new SqlParameter("@idmarketingOrgan",new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@ismodifiedcode",false),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@earnestMoney",Convert.ToInt32(0)),
				new SqlParameter("@idbusinesstype",new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.供应商)),
				//new SqlParameter("@madedate",DateTime.Parse("2013-06-01 00:00:00")),
				new SqlParameter("@pubuserdefnvc4",""),
				new SqlParameter("@memo",""),
				new SqlParameter("@idcurrency",new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@origEarnestMoney",Convert.ToInt32(0)),
				new SqlParameter("@changer",""),
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@acceptAddress",""),
				new SqlParameter("@linkMan",""),
				new SqlParameter("@iscarriedforwardout",false),
				new SqlParameter("@payType",TplusDatabaseHelper.Instance.GetPayTypeIdByTypeName("其它")),
				new SqlParameter("@pubuserdefnvc1",obj.部门),
				new SqlParameter("@contractId",""),
				//new SqlParameter("@totalTaxAmount",540.54),
				new SqlParameter("@linkTelphone",""),
				new SqlParameter("@updated",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(PurchaseOrder obj, Guid pid)
		{
			double tr;//税率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@basePrice",DBNull.Value),
				new SqlParameter("@inventoryBarCode",""),
				new SqlParameter("@unitExchangeRate",DBNull.Value),
				new SqlParameter("@subQuantity",DBNull.Value),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@origTaxAmount",obj.含税金额),
				new SqlParameter("@tax",obj.税额),
				new SqlParameter("@arrivalTimes",Convert.ToInt32(0)),
				new SqlParameter("@countQuantity2",DBNull.Value),
				new SqlParameter("@cumInstockShrinkageQuantity",DBNull.Value),
				new SqlParameter("@countQuantity",DBNull.Value),
				new SqlParameter("@origDiscount",DBNull.Value),
				new SqlParameter("@idPurchaseOrderDTO",pid),
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@lastmodifiedfield",""),
				new SqlParameter("@taxAmount",obj.含税金额),
				new SqlParameter("@compositionQuantity",""),
				new SqlParameter("@price",DBNull.Value),
				new SqlParameter("@stockTimes",Convert.ToInt32(0)),
				new SqlParameter("@saleOrderCode",""),
				new SqlParameter("@availableQuantity",DBNull.Value),
				new SqlParameter("@origDiscountPrice",obj.单价),
				//new SqlParameter("@baseQuantity",7.00),
				new SqlParameter("@taxPrice",obj.含税单价),
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@quantity2",DBNull.Value),
				new SqlParameter("@discount",DBNull.Value),
				//new SqlParameter("@baseDiscountPrice",66.00),
				new SqlParameter("@countArrivalQuantity2",DBNull.Value),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@origTax",obj.税额),
				new SqlParameter("@discountPrice",obj.单价),
				new SqlParameter("@idbaseunit",new Guid("3c390f6c-e76a-439d-8d14-a37b01447494")),
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@countInQuantity",DBNull.Value),
				new SqlParameter("@origTaxPrice",obj.含税单价),
				new SqlParameter("@origDiscountAmount",obj.金额),
				new SqlParameter("@isPresent",false),
				new SqlParameter("@islaborcost",false),
				new SqlParameter("@origPrice",DBNull.Value),
				new SqlParameter("@quantity",obj.数量),
				new SqlParameter("@partnerInventoryCode",""),
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@cumInstockShrinkageQuantity2",DBNull.Value),
				new SqlParameter("@existingQuantity",DBNull.Value),
				new SqlParameter("@countInQuantity2",DBNull.Value),
				new SqlParameter("@partnerInventoryName",""),
				//new SqlParameter("@baseTaxPrice",77.22),
				new SqlParameter("@countArrivalQuantity",DBNull.Value),
				new SqlParameter("@discountAmount",obj.金额),
				new SqlParameter("@taxFlag",Convert.ToInt32(0)),
				new SqlParameter("@updated",DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")), 
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps);
		}

		protected override bool CanExport(PurchaseOrder obj, out IEnumerable<string> msgs)
		{
			var list = new List<string>();
			if (TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]仓库不存在");
			}
			if (TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.供应商) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]供应商不存在");
			}
			if (TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]所属公司不存在");
			}
			if (TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]项目不存在");
			}
			if (TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.部门) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]部门不存在");
			}
			if (TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.业务员) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]业务员不存在");
			}
			if (TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]存货编码不存在");
			}
			msgs = list;
			return !msgs.Any();
		}
	}
}
