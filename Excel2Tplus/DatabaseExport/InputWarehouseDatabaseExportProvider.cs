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
	/// 采购入库单数据库导出提供程序
	/// </summary>
	class InputWarehouseDatabaseExportProvider : BaseDatabaseExportProvider<InputWarehouse>
	{
		protected override string VoucherName
		{
			get { return "采购入库单"; }
		}
		protected override string VoucherTable
		{
			get { return "ST_RDRecord"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(InputWarehouse obj, out Guid id)
		{
			id = Guid.NewGuid();

			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id),
				new SqlParameter("@idrdstyle",new Guid("698c1667-b8b8-41bd-8df2-8deac2677046")),
				new SqlParameter("@iscarriedforwardin",false),
				new SqlParameter("@amount",Convert.ToInt32(0)),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@madedate",DateTime.Parse("2013-06-01 00:00:00")),
				new SqlParameter("@auditor",""),
				new SqlParameter("@makerid",new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@maker","DEMO"),
				new SqlParameter("@rdDirectionFlag",true),
				new SqlParameter("@iscarriedforwardout",false),
				new SqlParameter("@isCostAccount",false),
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				//new SqlParameter("@VoucherYear",2013),
				//new SqlParameter("@accountingperiod",6),
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@isMergedFlow",false),
				//new SqlParameter("@accountingyear",2013),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
				new SqlParameter("@idbusitype",new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@IdMarketingOrgan",new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				//new SqlParameter("@VoucherPeriod",6),
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@pubuserdefnvc1",obj.部门),
				new SqlParameter("@accountState",new Guid("45964261-f94e-40ca-8643-b483034feaab")),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@ismodifiedcode",false),
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.供应商)),
				new SqlParameter("@pubuserdefnvc4",""),
				new SqlParameter("@memo",""),
				new SqlParameter("@idcurrency",new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@isAutoGenerate",false),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@TotalOrigTaxAmount",Convert.ToInt32(0)),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-26 17:19:15")),
				new SqlParameter("@BeforeUpgrade",""),
				new SqlParameter("@idvouchertype",new Guid("9a2c7c5a-a428-4669-aa40-0aa07758241b")),
				new SqlParameter("@exchangeRate",1),
				new SqlParameter("@isNoModify",""),
				new SqlParameter("@TotalTaxAmount",Convert.ToInt32(0)),
				new SqlParameter("@reviser",""),
				new SqlParameter("@printTime",Convert.ToInt32(0)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(InputWarehouse obj, Guid pid)
		{
			double tr;//税率
			decimal yf;//运费
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()),
				new SqlParameter("@isCostAccounted",false),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-26 17:19:15")),
				new SqlParameter("@estimatedPrice",DBNull.Value),
				new SqlParameter("@origSaleAmount",DBNull.Value),
				new SqlParameter("@origPrice2",DBNull.Value),
				new SqlParameter("@origTaxAmount",obj.含税金额),
				new SqlParameter("@defectiveQuantity",DBNull.Value),
				new SqlParameter("@origTaxSalePrice2",DBNull.Value),
				new SqlParameter("@subQuantity",DBNull.Value),
				new SqlParameter("@isManualCost",false),
				new SqlParameter("@CumReturnQuantity2",DBNull.Value),
				new SqlParameter("@taxFlag",Convert.ToInt32(0)),
				new SqlParameter("@estimatedPrice2",DBNull.Value),
				new SqlParameter("@distributedQuantity",DBNull.Value),
				new SqlParameter("@cumulativeSettlementBaseQuantity",DBNull.Value),
				new SqlParameter("@receiveAdjust",DBNull.Value),
				new SqlParameter("@dispatchAdjust",DBNull.Value),
				new SqlParameter("@ManuPrice2",DBNull.Value),
				new SqlParameter("@ManuFeeDiff",DBNull.Value),
				new SqlParameter("@cumulativeSettlementQuantity2",DBNull.Value),
				new SqlParameter("@RetailNoTaxAmount",DBNull.Value),
				new SqlParameter("@taxSaleAmount",DBNull.Value),
				new SqlParameter("@origTaxPrice",obj.含税单价),
				new SqlParameter("@origTaxPrice2",DBNull.Value),
				new SqlParameter("@feeAmount",DBNull.Value),
				new SqlParameter("@taxAmount",obj.含税金额),
				new SqlParameter("@idRDRecordDTO",pid),
				new SqlParameter("@taxSalePrice2",DBNull.Value),
				new SqlParameter("@changeRate",DBNull.Value),
				new SqlParameter("@baseQuantity",obj.数量),
				new SqlParameter("@cumulativePurchaseArrivalQuantity",DBNull.Value),
				new SqlParameter("@origTaxSalePrice",DBNull.Value),
				new SqlParameter("@materialAmount",DBNull.Value),
				new SqlParameter("@tax",obj.税额),
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@TaxManuPrice",DBNull.Value),
				new SqlParameter("@cumulativeEstimateAmount",DBNull.Value),
				new SqlParameter("@arrivalQuantity",DBNull.Value),
				new SqlParameter("@cumulativeSaleDispatchQuantity2",DBNull.Value),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@defectiveQuantity2",DBNull.Value),
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@OrigTaxManuAmount",DBNull.Value),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@ManuAmount",DBNull.Value),
				new SqlParameter("@TaxManuAmount",DBNull.Value),
				new SqlParameter("@estimatedAmount",Convert.ToInt32(0)),
				new SqlParameter("@origSalePrice",DBNull.Value),
				new SqlParameter("@TaxManuPrice2",DBNull.Value),
				new SqlParameter("@taxPrice",obj.含税单价),
				new SqlParameter("@origAmount",obj.金额),
				new SqlParameter("@feeAdjust",DBNull.Value),
				new SqlParameter("@arrivalQuantity2",DBNull.Value),
				new SqlParameter("@CumReturnQuantity",DBNull.Value),
				new SqlParameter("@isPresent",false),
				new SqlParameter("@totalAmount",Convert.ToInt32(0)),
				new SqlParameter("@ManuPrice",DBNull.Value),
				new SqlParameter("@taxSalePrice",DBNull.Value),
				new SqlParameter("@idbusiTypeByMergedFlow",new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@quantity",obj.数量),
				new SqlParameter("@origTaxSaleAmount",DBNull.Value),
				new SqlParameter("@kitQuantity2",DBNull.Value),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@origSalePrice2",DBNull.Value),
				new SqlParameter("@OrigManuPrice2",DBNull.Value),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@salePrice",DBNull.Value),
				new SqlParameter("@cumulativeSaleDispatchQuantity",DBNull.Value),
				new SqlParameter("@RetailAmount",DBNull.Value),
				new SqlParameter("@distributedQuantity2",DBNull.Value),
				new SqlParameter("@idproject",new Guid("14e9f2a9-7876-45a4-961a-a39c00e4d4b0")),
				new SqlParameter("@baseEstimatedPrice",Convert.ToInt32(0)),
				new SqlParameter("@cumulativeSettlementAmount",DBNull.Value),
				new SqlParameter("@idbaseunit",new Guid("26597812-63c0-4f80-917d-a37b013acb27")),
				new SqlParameter("@LastModifiedField",""),
				new SqlParameter("@origPrice",obj.单价),
				new SqlParameter("@price2",DBNull.Value),
				new SqlParameter("@OrigTaxManuPrice",DBNull.Value),
				new SqlParameter("@origTax",Convert.ToInt32(0)),
				new SqlParameter("@cumulativePurchaseArrivalQuantity2",DBNull.Value),
				new SqlParameter("@salePrice2",DBNull.Value),
				new SqlParameter("@manHour",DBNull.Value),
				new SqlParameter("@cumulativeSettlementSubQuantity",DBNull.Value),
				new SqlParameter("@quantity2",DBNull.Value),
				new SqlParameter("@OrigTaxManuPrice2",DBNull.Value),
				new SqlParameter("@cumulativeSettlementQuantity",DBNull.Value),
				new SqlParameter("@saleAmount",DBNull.Value),
				new SqlParameter("@OrigManuPrice",DBNull.Value),
				new SqlParameter("@TaxPrice2",DBNull.Value),
				new SqlParameter("@countQuantity",DBNull.Value),
				new SqlParameter("@amount",obj.金额),
				new SqlParameter("@OrigManuAmount",DBNull.Value),
				new SqlParameter("@RetailNoTaxPrice",DBNull.Value),
				new SqlParameter("@basePrice",obj.单价),
				new SqlParameter("@kitQuantity",DBNull.Value),
				new SqlParameter("@baseManuPrice",DBNull.Value),
				new SqlParameter("@countQuantity2",DBNull.Value),
				new SqlParameter("@price",obj.单价),
				new SqlParameter("@priuserdefdecm1",decimal.TryParse(obj.每双运费,out yf)?yf:yf),
			};

			return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps), BuildCurrentStockSql(obj) };
		}

		protected Tuple<string, IEnumerable<DbParameter>> BuildCurrentStockSql(InputWarehouse obj)
		{
			return new Tuple<string, IEnumerable<DbParameter>>(
				"ST_CurrentStock",
				new[]
				{
					new SqlParameter("@id",Guid.NewGuid()),
					new SqlParameter("@purchaseArrivalBaseQuantity",obj.数量),
					new SqlParameter("@recordDate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@isCarriedForwardOut",Convert.ToInt32(0)),
					new SqlParameter("@isCarriedForwardIn",Convert.ToInt32(0)),
					new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
					new SqlParameter("@updated",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@updatedBy","DEMO"),
					new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
					new SqlParameter("@idbaseunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
					new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
					new SqlParameter("@IdMarketingOrgan",new Guid("4AD74463-E871-4DC1-BEB0-6E6EAA0A6386"))
				});
		}

		protected override bool CanExport(InputWarehouse obj, out IEnumerable<string> msgs)
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
