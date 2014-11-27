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
				new SqlParameter("@id",id),
				new SqlParameter("@makerid",new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@origPaymentAmount",Convert.ToInt32(0)),
				//new SqlParameter("@origtotalAmount",594.00),
				new SqlParameter("@iscarriedforwardin",false),
				new SqlParameter("@invoiceType",new Guid("82bf126f-10e0-4272-8098-4c4c9c1ae3bc")),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@exchangeRate",1),
				new SqlParameter("@auditor",""),
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.供应商)),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@IsBeforeSystemInuse",false),
				//new SqlParameter("@origTotalTaxAmount",694.98),
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@isAutoGenerateInStock",false),
				new SqlParameter("@origPaymentCashAmount",Convert.ToInt32(0)),
				new SqlParameter("@PrintCount",Convert.ToInt32(0)),
				new SqlParameter("@accountingperiod",Convert.ToInt32(0)),
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@accountingyear",Convert.ToInt32(0)),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
				//new SqlParameter("@totalAmount",594.00),
				new SqlParameter("@idMarketingOrgan",new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@isReduceArrival",false),
				new SqlParameter("@settleState",new Guid("03f167c8-4494-44ea-b6eb-ecb2539a85a0")),
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@paymentAmount",Convert.ToInt32(0)),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@ismodifiedcode",false),
				new SqlParameter("@isAutoGenerateInvoice",false),
				new SqlParameter("@idbusinesstype",new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				//new SqlParameter("@madedate",DateTime.Parse("2013-06-01 00:00:00")),
				new SqlParameter("@paymentCashAmount",Convert.ToInt32(0)),
				new SqlParameter("@pubuserdefnvc4",""),
				new SqlParameter("@memo",""),
				new SqlParameter("@idcurrency",new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@purchaseInvoiceNo",""),
				new SqlParameter("@changer",""),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-26 15:46:49")),
				new SqlParameter("@isNoArapBookkeeping",false),
				new SqlParameter("@isPriceCheck",true),
				new SqlParameter("@acceptAddress",""),
				new SqlParameter("@linkMan",""),
				new SqlParameter("@iscarriedforwardout",false),
				new SqlParameter("@payType",new Guid("8f69ab53-409f-4cac-acbc-fd5b65b684d4")),
				new SqlParameter("@pubuserdefnvc1",obj.部门),
				new SqlParameter("@inStockState",new Guid("2f7d8d02-10c4-453e-aff9-691b052d9e52")),
				//new SqlParameter("@totalTaxAmount",694.98),
				new SqlParameter("@linkTelphone",""),
				new SqlParameter("@reviser",""),
				new SqlParameter("@maker","DEMO"),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(PurchaseArrival obj, Guid pid)
		{
			double tr;//税率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-26 15:46:49")),
				new SqlParameter("@cumInstockShrinkageQuantity",DBNull.Value),
				new SqlParameter("@origShrinkageQuantity",DBNull.Value),
				new SqlParameter("@origTaxAmount",obj.含税金额),
				new SqlParameter("@subQuantity",DBNull.Value),
				new SqlParameter("@taxFlag",Convert.ToInt32(0)),
				//new SqlParameter("@baseTaxPrice",77.22),
				new SqlParameter("@arrivalQuantity",DBNull.Value),
				new SqlParameter("@settleAmount",DBNull.Value),
				new SqlParameter("@shrinkageSubQuantity",DBNull.Value),
				new SqlParameter("@origSettleAmount",DBNull.Value),
				new SqlParameter("@warehouseAdjustAmount",DBNull.Value),
				new SqlParameter("@origTaxPrice",obj.含税单价),
				new SqlParameter("@cumReduceShrinkageQuantity2",DBNull.Value),
				new SqlParameter("@totalReturnQuantity2",DBNull.Value),
				new SqlParameter("@cumAccountAmount",DBNull.Value),
				//new SqlParameter("@taxAmount",694.98),
				new SqlParameter("@discountPrice2",DBNull.Value),
				new SqlParameter("@discount",DBNull.Value),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@totalSettleQuantity2",DBNull.Value),
				new SqlParameter("@arrivalSubQuantity",DBNull.Value),
				new SqlParameter("@totalReturnQuantity",DBNull.Value),
				//new SqlParameter("@tax",100.98),
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@unitExchangeRate",DBNull.Value),
				new SqlParameter("@arrivalBaseQuantity",DBNull.Value),
				new SqlParameter("@totalInQuantity",DBNull.Value),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				//new SqlParameter("@baseQuantity",9.00),
				//new SqlParameter("@baseDiscountPrice",66.00),
				new SqlParameter("@existingQuantity",DBNull.Value),
				new SqlParameter("@shrinkageQuantity",DBNull.Value),
				new SqlParameter("@islaborcost",false),
				new SqlParameter("@shrinkageBaseQuantity",DBNull.Value),
				new SqlParameter("@totalInQuantity2",DBNull.Value),
				new SqlParameter("@origDiscountPrice2",DBNull.Value),
				new SqlParameter("@origDiscountPrice",obj.单价),
				new SqlParameter("@inventoryBarCode",""),
				//new SqlParameter("@taxPrice",77.22),
				new SqlParameter("@cumInstockShrinkageQuantity2",DBNull.Value),
				new SqlParameter("@cumInvoiceShrinkageQuantity",DBNull.Value),
				new SqlParameter("@totalSettleSubQuantity",DBNull.Value),
				new SqlParameter("@quantity",obj.数量),
				new SqlParameter("@purchaseAmount",DBNull.Value),
				new SqlParameter("@origShrinkageQuantity2",DBNull.Value),
				new SqlParameter("@isPresent",false),
				new SqlParameter("@acceptanceQuantity",DBNull.Value),
				new SqlParameter("@cumReturnShrinkageQuantity",DBNull.Value),
				new SqlParameter("@acceptanceQuantity2",DBNull.Value),
				new SqlParameter("@totalReduceQuantity",DBNull.Value),
				new SqlParameter("@cumReduceShrinkageQuantity",DBNull.Value),
				new SqlParameter("@totalSettleTaxAmount",DBNull.Value),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@saleOrderCode",""),
				new SqlParameter("@cumReturnShrinkageQuantity2",DBNull.Value),
				new SqlParameter("@cumInvoiceShrinkageQuantity2",DBNull.Value),
				new SqlParameter("@partnerInventoryCode",""),
				new SqlParameter("@idproject",new Guid("666374a4-6341-4b3a-96fc-a3930150f584")),
				new SqlParameter("@idbaseunit",new Guid("3c390f6c-e76a-439d-8d14-a37b01447494")),
				new SqlParameter("@totalReduceQuantity2",DBNull.Value),
				new SqlParameter("@totalSettleBaseQuantity",DBNull.Value),
				new SqlParameter("@shrinkageQuantity2",DBNull.Value),
				new SqlParameter("@origPrice",DBNull.Value),
				new SqlParameter("@totalSettleQuantity",DBNull.Value),
				new SqlParameter("@RetailPrice",DBNull.Value),
				new SqlParameter("@availableQuantity",DBNull.Value),
				new SqlParameter("@partnerInventoryName",""),
				new SqlParameter("@origTax",obj.税额),
				new SqlParameter("@idPurchaseArrivalDTO",pid),
				new SqlParameter("@quantity2",DBNull.Value),
				//new SqlParameter("@discountPrice",66.00),
				new SqlParameter("@compositionQuantity",""),
				new SqlParameter("@origDiscount",DBNull.Value),
				new SqlParameter("@arrivalQuantity2",DBNull.Value),
				new SqlParameter("@totalInvoiceQuantity2",DBNull.Value),
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@origDiscountAmount",obj.金额),
				new SqlParameter("@totalInvoiceQuantity",DBNull.Value),
				new SqlParameter("@lastmodifiedfield",""),
				new SqlParameter("@basePrice",DBNull.Value),
				//new SqlParameter("@discountAmount",594.00),
				new SqlParameter("@price",DBNull.Value),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps);
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
