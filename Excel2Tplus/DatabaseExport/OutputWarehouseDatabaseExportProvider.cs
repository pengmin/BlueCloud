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
	class OutputWarehouseDatabaseExportProvider : BaseDatabaseExportProvider<OutputWarehouse>
	{
		protected override string VoucherName
		{
			get { return "销售出库单"; }
		}
		protected override string VoucherTable
		{
			get { return "ST_RDRecord"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(OutputWarehouse obj, out Guid id)
		{
			id = Guid.NewGuid();

			decimal cy;//抽佣比率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id),
				new SqlParameter("@idrdstyle",new Guid("2f384c02-e650-4421-81de-e870e6a8b0b3")),
				new SqlParameter("@iscarriedforwardin",false),
				new SqlParameter("@amount",Convert.ToInt32(0)),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				//new SqlParameter("@madedate",DateTime.Parse("2013-06-01 00:00:00")),
				new SqlParameter("@auditor",""),
				new SqlParameter("@makerid",new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@maker","DEMO"),
				new SqlParameter("@rdDirectionFlag",Convert.ToInt32(0)),
				new SqlParameter("@iscarriedforwardout",false),
				new SqlParameter("@isCostAccount",false),
				new SqlParameter("@contact",""),
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				//new SqlParameter("@VoucherYear",2013),
				//new SqlParameter("@accountingperiod",6),
				new SqlParameter("@idproject",new Guid("666374a4-6341-4b3a-96fc-a3930150f584")),
				new SqlParameter("@isMergedFlow",false),
				//new SqlParameter("@accountingyear",2013),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
				new SqlParameter("@idbusitype",new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98")),
				new SqlParameter("@IdMarketingOrgan",new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)),
				//new SqlParameter("@VoucherPeriod",6),
				new SqlParameter("@deliveryState",new Guid("3fa9f8c4-4d3f-4b90-be87-a18cde11ca8e")),
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@pubuserdefnvc1",obj.部门),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@ismodifiedcode",false),
				new SqlParameter("@idsettleCustomer",new Guid("c8412a2e-d9f9-46d0-8fd4-a39a012e5099")),
				new SqlParameter("@Mobilephone",DBNull.Value),
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@pubuserdefnvc4",""),
				new SqlParameter("@memo",""),
				new SqlParameter("@idcurrency",new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@isAutoGenerate",false),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@TotalOrigTaxAmount",DBNull.Value),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-26 22:46:30")),
				new SqlParameter("@idvouchertype",new Guid("bb007f33-c0f0-4a19-8588-1e0e314d1f56")),
				new SqlParameter("@Address",""),
				new SqlParameter("@dispatchAddress",""),
				new SqlParameter("@exchangeRate",1),
				new SqlParameter("@contactPhone",""),
				new SqlParameter("@isNoModify",""),
				new SqlParameter("@TotalTaxAmount",DBNull.Value),
				new SqlParameter("@reviser",""),
				new SqlParameter("@printTime",Convert.ToInt32(0)),
				new SqlParameter("@priuserdefdecm1",decimal.TryParse(obj.抽佣比率,out cy)?cy:cy),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(OutputWarehouse obj, Guid pid)
		{
			decimal amount;//金额
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@isCostAccounted",false),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-26 22:46:30")),
				new SqlParameter("@estimatedPrice",DBNull.Value),
				//new SqlParameter("@origSaleAmount",376.07),
				new SqlParameter("@origPrice2",DBNull.Value),
				new SqlParameter("@origTaxAmount",DBNull.Value),
				new SqlParameter("@defectiveQuantity",DBNull.Value),
				new SqlParameter("@origTaxSalePrice2",DBNull.Value),
				new SqlParameter("@subQuantity",DBNull.Value),
				new SqlParameter("@isManualCost",false),
				new SqlParameter("@CumReturnQuantity2",DBNull.Value),
				new SqlParameter("@taxFlag",1),
				new SqlParameter("@estimatedPrice2",DBNull.Value),
				new SqlParameter("@distributedQuantity",DBNull.Value),
				new SqlParameter("@cumulativeSettlementBaseQuantity",DBNull.Value),
				new SqlParameter("@receiveAdjust",DBNull.Value),
				new SqlParameter("@dispatchAdjust",DBNull.Value),
				new SqlParameter("@ManuPrice2",DBNull.Value),
				new SqlParameter("@ManuFeeDiff",DBNull.Value),
				new SqlParameter("@cumulativeSettlementQuantity2",DBNull.Value),
				new SqlParameter("@RetailNoTaxAmount",DBNull.Value),
				//new SqlParameter("@taxSaleAmount",440.00),
				new SqlParameter("@origTaxPrice",DBNull.Value),
				new SqlParameter("@origTaxPrice2",DBNull.Value),
				new SqlParameter("@feeAmount",DBNull.Value),
				new SqlParameter("@taxAmount",DBNull.Value),
				new SqlParameter("@idRDRecordDTO",pid), 
				new SqlParameter("@taxSalePrice2",DBNull.Value),
				new SqlParameter("@changeRate",DBNull.Value),
				//new SqlParameter("@baseQuantity",5.00),
				new SqlParameter("@cumulativePurchaseArrivalQuantity",DBNull.Value),
				//new SqlParameter("@origTaxSalePrice", 88.00),
				new SqlParameter("@materialAmount",DBNull.Value),
				//new SqlParameter("@tax",63.93),
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@TaxManuPrice",DBNull.Value),
				new SqlParameter("@cumulativeEstimateAmount",DBNull.Value),
				new SqlParameter("@arrivalQuantity",DBNull.Value),
				new SqlParameter("@cumulativeSaleDispatchQuantity2",DBNull.Value),
				new SqlParameter("@createdtime",DateTime.Now), 
				new SqlParameter("@defectiveQuantity2",DBNull.Value),
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@OrigTaxManuAmount",DBNull.Value),
				//new SqlParameter("@taxRate",0.17),
				new SqlParameter("@ManuAmount",DBNull.Value),
				new SqlParameter("@TaxManuAmount",DBNull.Value),
				new SqlParameter("@estimatedAmount",DBNull.Value),
				//new SqlParameter("@origSalePrice",75.21),
				new SqlParameter("@TaxManuPrice2",DBNull.Value),
				new SqlParameter("@taxPrice",DBNull.Value),
				new SqlParameter("@origAmount",DBNull.Value),
				new SqlParameter("@feeAdjust",DBNull.Value),
				new SqlParameter("@arrivalQuantity2",DBNull.Value),
				new SqlParameter("@CumReturnQuantity",DBNull.Value),
				new SqlParameter("@isPresent",false),
				new SqlParameter("@totalAmount",DBNull.Value),
				new SqlParameter("@ManuPrice",DBNull.Value),
				new SqlParameter("@taxSalePrice",decimal.TryParse(obj.含税售价,out amount)?amount:0m),
				new SqlParameter("@idbusiTypeByMergedFlow",new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98")),
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@origTaxSaleAmount",decimal.TryParse(obj.含税销售金额,out amount)?amount:0m),
				new SqlParameter("@kitQuantity2",DBNull.Value),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@origSalePrice2",DBNull.Value),
				new SqlParameter("@OrigManuPrice2",DBNull.Value),
				new SqlParameter("@idwarehouse",new Guid("daa79ee8-66b1-4045-8bfe-a37b015b5c34")),
				new SqlParameter("@salePrice",decimal.TryParse(obj.售价,out amount)?amount:0m),
				new SqlParameter("@cumulativeSaleDispatchQuantity",DBNull.Value),
				new SqlParameter("@RetailAmount",DBNull.Value),
				new SqlParameter("@distributedQuantity2",DBNull.Value),
				new SqlParameter("@idproject",new Guid("666374a4-6341-4b3a-96fc-a3930150f584")),
				new SqlParameter("@baseEstimatedPrice",DBNull.Value),
				new SqlParameter("@cumulativeSettlementAmount",DBNull.Value),
				new SqlParameter("@idbaseunit",new Guid("26597812-63c0-4f80-917d-a37b013acb27")),
				new SqlParameter("@LastModifiedField",""),
				new SqlParameter("@origPrice",DBNull.Value),
				new SqlParameter("@price2",DBNull.Value),
				new SqlParameter("@OrigTaxManuPrice",DBNull.Value),
				new SqlParameter("@origTax",decimal.TryParse(obj.税额,out amount)?amount:0m),
				new SqlParameter("@cumulativePurchaseArrivalQuantity2",DBNull.Value),
				new SqlParameter("@salePrice2",DBNull.Value),
				new SqlParameter("@manHour",DBNull.Value),
				new SqlParameter("@cumulativeSettlementSubQuantity",DBNull.Value),
				new SqlParameter("@quantity2",DBNull.Value),
				new SqlParameter("@OrigTaxManuPrice2",DBNull.Value),
				new SqlParameter("@cumulativeSettlementQuantity",DBNull.Value),
				new SqlParameter("@saleAmount",decimal.TryParse(obj.销售金额,out amount)?amount:0m),
				new SqlParameter("@OrigManuPrice",DBNull.Value),
				new SqlParameter("@TaxPrice2",DBNull.Value),
				new SqlParameter("@countQuantity",DBNull.Value),
				new SqlParameter("@amount",decimal.TryParse(obj.成本金额,out amount)?amount:0m),
				new SqlParameter("@OrigManuAmount",DBNull.Value),
				new SqlParameter("@RetailNoTaxPrice",DBNull.Value),
				new SqlParameter("@basePrice",Convert.ToInt32(0)),
				new SqlParameter("@kitQuantity",DBNull.Value),
				new SqlParameter("@baseManuPrice",DBNull.Value),
				new SqlParameter("@countQuantity2",DBNull.Value),
				new SqlParameter("@pubuserdefdecm4",decimal.TryParse(obj.抽佣,out amount)?amount:0m),
				new SqlParameter("@price",obj.单价),
				new SqlParameter("@priuserdefnvc1",obj.销售订单号),
				new SqlParameter("@pubuserdefnvc1",obj.物流名称单号),
				new SqlParameter("@pubuserdefnvc2",obj.发货信息),
				new SqlParameter("@pubuserdefnvc3",obj.客户收货信息),
				new SqlParameter("@pubuserdefnvc4",obj.平台单号),
				new SqlParameter("@pubuserdefdecm1",decimal.TryParse(obj.满减活动,out amount)?amount:0m),
				new SqlParameter("@pubuserdefdecm2",decimal.TryParse(obj.抵用券,out amount)?amount:0m),
				new SqlParameter("@pubuserdefdecm3",decimal.TryParse(obj.代收运费,out amount)?amount:0m),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps);
		}
	}
}
