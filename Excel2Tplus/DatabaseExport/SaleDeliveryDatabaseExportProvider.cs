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
	class SaleDeliveryDatabaseExportProvider : BaseDatabaseExportProvider<SaleDelivery>
	{
		protected override string VoucherName
		{
			get { return "销货单"; }
		}
		protected override string VoucherTable
		{
			get { return "SA_SaleDelivery"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(SaleDelivery obj, out Guid id)
		{
			id = Guid.NewGuid();

			decimal d;
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id), 
				new SqlParameter("@isBeforeSystemInuse",false),
				new SqlParameter("@iscarriedforwardin",false),
				//new SqlParameter("@amount",44),
				new SqlParameter("@prereceiveamount",Convert.ToInt32(0)),
				//new SqlParameter("@madedate",DateTime.Parse("2013-06-01 00:00:00")),
				new SqlParameter("@isCounteractDelivery",false),
				new SqlParameter("@auditor",""),
				new SqlParameter("@makerid",new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 
				//new SqlParameter("@origTaxAmount",51.48),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
				new SqlParameter("@ismodifiedcode",false),
				new SqlParameter("@MemberAddress",""),
				new SqlParameter("@origCancelAmount",DBNull.Value),
				//new SqlParameter("@origAmount",44),
				new SqlParameter("@PrintCount",Convert.ToInt32(0)),
				new SqlParameter("@idrdstyle",new Guid("2f384c02-e650-4421-81de-e870e6a8b0b3")),
				new SqlParameter("@changer",""),
				new SqlParameter("@accountingperiod",Convert.ToInt32(0)),
				new SqlParameter("@DataSource",new Guid("3d362676-d6d5-4439-9be5-40c67514b9f5")),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@accountingyear",Convert.ToInt32(0)),
				new SqlParameter("@isAutoGenerateSaleOut",false),
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@idmarketingOrgan",new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)), 
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")), 
				new SqlParameter("@origprereceiveamount",Convert.ToInt32(0)),
				new SqlParameter("@idsettlecustomer",new Guid("c8412a2e-d9f9-46d0-8fd4-a39a012e5099")),
				new SqlParameter("@idcustomer",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@isAutoGenerateInvoice",false),
				new SqlParameter("@idbusinesstype",new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98")),
				new SqlParameter("@Mobilephone",DBNull.Value),
				new SqlParameter("@discountsAndAllowances",DBNull.Value),
				new SqlParameter("@pubuserdefnvc4",""),
				new SqlParameter("@memo",""),
				new SqlParameter("@idcurrency",new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@OrigInvoiceTaxAmount",DBNull.Value),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号), 
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@saleInvoiceNo",""),
				new SqlParameter("@isPriceTrace",true),
				new SqlParameter("@origSettleAmount",Convert.ToInt32(0)),
				new SqlParameter("@settleAmount",Convert.ToInt32(0)),
				//new SqlParameter("@taxAmount",51.48),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-27 08:51:42")),
				new SqlParameter("@isNoArapBookkeeping",false),
				new SqlParameter("@reciveType",new Guid("ef370928-98f5-4312-a1c6-021587d17af1")),
				new SqlParameter("@address",""),
				new SqlParameter("@isCancel",new Guid("03f167c8-4494-44ea-b6eb-ecb2539a85a0")),
				new SqlParameter("@cancelAmount",DBNull.Value),
				new SqlParameter("@idproject",new Guid("666374a4-6341-4b3a-96fc-a3930150f584")),
				new SqlParameter("@contactPhone",""),
				new SqlParameter("@isSaleOut",new Guid("34b60e63-7949-4209-b6d8-a2cadf00cfd7")),
				new SqlParameter("@linkMan",""),
				new SqlParameter("@invoiceType",new Guid("4806d0be-c026-4a96-a271-97514e6a9082")),
				new SqlParameter("@discountAmount",DBNull.Value),
				new SqlParameter("@iscarriedforwardout",false),
				new SqlParameter("@pubuserdefnvc1",obj.部门), 
				new SqlParameter("@exchangeRate",1),
				new SqlParameter("@reviser",""),
				//new SqlParameter("@maker","DEMO"),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@priuserdefdecm1",decimal.TryParse(obj.抽佣比率,out d)?d:d), 
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(SaleDelivery obj, Guid pid)
		{
			double tr;//税率
			decimal amount;
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@OrigInvoiceTaxAmount",DBNull.Value),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-27 08:51:42")),
				new SqlParameter("@counteractQuntity2",DBNull.Value),
				new SqlParameter("@origTaxAmount",decimal.TryParse(obj.含税销售金额,out amount)?amount:0m),
				new SqlParameter("@subQuantity",DBNull.Value),
				new SqlParameter("@isManualCost",false),
				new SqlParameter("@virtualBaseQuantity",DBNull.Value),
				new SqlParameter("@taxFlag",Convert.ToInt32(0)),
				new SqlParameter("@invoiceQuantity2",DBNull.Value),
				new SqlParameter("@baseTaxPrice",DBNull.Value),
				new SqlParameter("@idSaleDeliveryDTO",pid), 
				new SqlParameter("@settleAmount",DBNull.Value),
				new SqlParameter("@origSettleAmount",DBNull.Value),
				new SqlParameter("@adjustAmount",DBNull.Value),
				new SqlParameter("@origTaxPrice",decimal.TryParse(obj.含税售价,out amount)?amount:0m),
				new SqlParameter("@sentBaseQuantityAssociated",DBNull.Value),
				new SqlParameter("@TaxAmount",decimal.TryParse(obj.含税销售金额,out amount)?amount:0m),
				new SqlParameter("@discountPrice",obj.单价),
				new SqlParameter("@discount",DBNull.Value),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@tax",decimal.TryParse(obj.税额,out amount)?amount:0m),
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@unitExchangeRate",DBNull.Value),
				new SqlParameter("@costAmount",decimal.TryParse(obj.成本金额,out amount)?amount:0m),
				new SqlParameter("@returnQuntity2",DBNull.Value),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@origExpenseAmount",DBNull.Value),
				new SqlParameter("@baseQuantity",1.00),
				new SqlParameter("@baseDiscountPrice",DBNull.Value),
				new SqlParameter("@saleoutquantity2",DBNull.Value),
				new SqlParameter("@counteractQuntity",DBNull.Value),
				new SqlParameter("@TaxPrice",decimal.TryParse(obj.含税售价,out amount)?amount:0m),
				new SqlParameter("@saleOutQuantity",DBNull.Value),
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@expenseAmount",DBNull.Value),
				new SqlParameter("@isPresent",false),
				new SqlParameter("@invoiceQuantity",DBNull.Value),
				new SqlParameter("@origDiscountPrice",obj.单价),
				new SqlParameter("@returnQuntity",DBNull.Value),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@costPrice",DBNull.Value),
				new SqlParameter("@associatedDocIDs",DBNull.Value),
				new SqlParameter("@idproject",new Guid("666374a4-6341-4b3a-96fc-a3930150f584")),
				new SqlParameter("@idbaseunit",new Guid("3c390f6c-e76a-439d-8d14-a37b01447494")),
				new SqlParameter("@inventoryBarCode",""),
				new SqlParameter("@origPrice",DBNull.Value),
				new SqlParameter("@origTax",decimal.TryParse(obj.税额,out amount)?amount:0m),
				new SqlParameter("@backSentAmount",DBNull.Value),
				new SqlParameter("@quantity2",DBNull.Value),
				new SqlParameter("@origDiscount",DBNull.Value),
				new SqlParameter("@sentBaseAmountAssociated",DBNull.Value),
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@origDiscountAmount",decimal.TryParse(obj.销售金额,out amount)?amount:0m),
				new SqlParameter("@virtualBaseAmount",DBNull.Value),
				new SqlParameter("@lastmodifiedfield",""),
				new SqlParameter("@basePrice",DBNull.Value),
				new SqlParameter("@DiscountAmount",decimal.TryParse(obj.销售金额,out amount)?amount:0m),
				new SqlParameter("@backSentQuantity",DBNull.Value),
				new SqlParameter("@price",DBNull.Value),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 
				new SqlParameter("@Retailprice",decimal.TryParse(obj.售价,out amount)?amount:0m),
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

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps);
		}
	}
}
