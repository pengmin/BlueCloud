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
	/// 采购进货单数据库导出提供程序
	/// </summary>
	internal class PurchaseArrivalDatabaseExportProvider : BaseDatabaseExportProvider<PurchaseArrival>
	{
		protected override string VoucherName
		{
			get { return "进货单"; }
		}

		protected override string VoucherTable
		{
			get { return "PU_PurchaseArrival"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(PurchaseArrival obj, out Guid id)
		{
			id = Guid.NewGuid();

			var ps = new DbParameter[]
			{
				new SqlParameter("@id", id),
				new SqlParameter("@code", string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@invoiceType", new Guid("82bf126f-10e0-4272-8098-4c4c9c1ae3bc")),
				new SqlParameter("@purchaseInvoiceNo", ""),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@acceptAddress", ""),
				new SqlParameter("@linkMan", ""),
				new SqlParameter("@linkTelphone", ""),
				new SqlParameter("@payType", new Guid("8f69ab53-409f-4cac-acbc-fd5b65b684d4")),
				new SqlParameter("@origPaymentCashAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@paymentCashAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@origTotalTaxAmount", Convert.ToDecimal(1093.95000000000000)),
				new SqlParameter("@totalTaxAmount", Convert.ToDecimal(1093.95000000000000)),
				new SqlParameter("@origtotalAmount", Convert.ToDecimal(935.00000000000000)),
				new SqlParameter("@totalAmount", Convert.ToDecimal(935.00000000000000)),
				new SqlParameter("@isPriceCheck", Convert.ToByte(1)),
				new SqlParameter("@isReduceArrival", Convert.ToByte(0)),
				new SqlParameter("@isAutoGenerateInvoice", Convert.ToByte(0)),
				new SqlParameter("@isAutoGenerateInStock", Convert.ToByte(0)),
				new SqlParameter("@voucherState", TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@memo", ""),
				new SqlParameter("@inStockState", new Guid("2f7d8d02-10c4-453e-aff9-691b052d9e52")),
				new SqlParameter("@settleState", new Guid("03f167c8-4494-44ea-b6eb-ecb2539a85a0")),
				new SqlParameter("@isNoArapBookkeeping", Convert.ToByte(0)),
				new SqlParameter("@voucherdate", DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@maker", "DEMO"),
				new SqlParameter("@madedate", "2013-06-01 00:00:00"),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@makerid", new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@accountingperiod", Convert.ToInt32(0)),
				new SqlParameter("@accountingyear", Convert.ToInt32(0)),
				new SqlParameter("@createdtime", "2014-12-02 09:22:39"),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@idbusinesstype", new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@idpartner", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.供应商)),
				new SqlParameter("@iddepartment", TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@pubuserdefnvc1", obj.部门),
				new SqlParameter("@pubuserdefnvc2", obj.业务员),
				new SqlParameter("@pubuserdefnvc3", obj.退货日期),
				new SqlParameter("@pubuserdefnvc4", ""),
				new SqlParameter("@origPaymentAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@paymentAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@IsBeforeSystemInuse", Convert.ToByte(0)),
				new SqlParameter("@idMarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@changer", ""),
				new SqlParameter("@PrintCount", Convert.ToInt32(0)),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(PurchaseArrival obj, Guid pid)
		{
			double tr;//税率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", Guid.NewGuid()),
				new SqlParameter("@code", (Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@taxFlag", Convert.ToByte(0)),
				new SqlParameter("@compositionQuantity", ""),
				new SqlParameter("@quantity", obj.数量),
				new SqlParameter("@origDiscountPrice", obj.单价),
				new SqlParameter("@baseQuantity", obj.数量),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@taxRate", (double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@origTaxPrice", obj.含税单价),
				new SqlParameter("@origDiscountAmount", obj.金额),
				new SqlParameter("@origTax", obj.税额),
				new SqlParameter("@origTaxAmount", obj.含税金额),
				new SqlParameter("@discountPrice", obj.单价),
				new SqlParameter("@taxPrice", obj.含税单价),
				new SqlParameter("@discountAmount", obj.金额),
				new SqlParameter("@tax", obj.税额),
				new SqlParameter("@taxAmount", obj.含税金额),
				new SqlParameter("@isPresent", Convert.ToByte(0)),
				new SqlParameter("@saleOrderCode", ""),
				new SqlParameter("@baseDiscountPrice", obj.单价),
				new SqlParameter("@baseTaxPrice", obj.含税单价),
				new SqlParameter("@lastmodifiedfield", ""),
				new SqlParameter("@inventoryBarCode", ""),
				new SqlParameter("@partnerInventoryCode", ""),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@idPurchaseArrivalDTO", pid),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@partnerInventoryName", ""),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@islaborcost", Convert.ToByte(0)),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
			};

			return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps) };
		}

		protected override bool CanExport(PurchaseArrival obj, out IEnumerable<string> msgs)
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
			if (TplusDatabaseHelper.Instance.GetUserIdbyUserName(obj.业务员) is DBNull)
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
