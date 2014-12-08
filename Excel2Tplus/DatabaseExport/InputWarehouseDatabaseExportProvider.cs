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

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(InputWarehouse obj, Guid id)
		{
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", id),
				new SqlParameter("@code", string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				//new SqlParameter("@sourceVoucherId", new Guid("bfd7be52-f264-4662-940c-d32e68ffbe9b")),
				//new SqlParameter("@sourceVoucherCode", "PO-2013-06-0002"),
				new SqlParameter("@voucherState", TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@printTime", Convert.ToInt32(0)),
				//new SqlParameter("@amount", Convert.ToDecimal(46782.00000000000000)),
				new SqlParameter("@rdDirectionFlag", Convert.ToByte(1)),
				new SqlParameter("@accountState", new Guid("45964261-f94e-40ca-8643-b483034feaab")),
				new SqlParameter("@isCostAccount", Convert.ToByte(0)),
				new SqlParameter("@isMergedFlow", Convert.ToByte(0)),
				new SqlParameter("@isAutoGenerate", Convert.ToByte(0)),
				new SqlParameter("@memo", ""),
				//new SqlParameter("@isNoModify", "#BusiType#Partner_Code#Partner#Partner_PartnerAbbName#"),
				new SqlParameter("@voucherdate", DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				//new SqlParameter("@maker", "DEMO"),
				//new SqlParameter("@madedate", "2013-06-01 00:00:00"),
				//new SqlParameter("@auditor", ""),
				//new SqlParameter("@makerid", new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				//new SqlParameter("@accountingperiod", Convert.ToInt32(6)),
				//new SqlParameter("@accountingyear", Convert.ToInt32(2013)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				//new SqlParameter("@updated", "2014-12-04 14:37:41"),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@idpartner", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.供应商)),
				new SqlParameter("@idbusitype", new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@idvouchertype", new Guid("9a2c7c5a-a428-4669-aa40-0aa07758241b")),
				new SqlParameter("@idsourcevouchertype", new Guid("33d9eacd-ac90-45c4-a63c-2a65a1cef7a0")),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idrdstyle", new Guid("698c1667-b8b8-41bd-8df2-8deac2677046")),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@pubuserdefnvc1", obj.部门),
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@pubuserdefnvc3", obj.退货日期),
				new SqlParameter("@pubuserdefnvc4", ""),
				//new SqlParameter("@VoucherYear", Convert.ToInt32(2013)),
				//new SqlParameter("@VoucherPeriod", Convert.ToInt32(6)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@idProject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@BeforeUpgrade", ""),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				//new SqlParameter("@TotalOrigTaxAmount", Convert.ToDecimal(54734.94000000000000)),
				//new SqlParameter("@TotalTaxAmount", Convert.ToDecimal(54734.94000000000000)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(InputWarehouse obj, Guid pid)
		{
			double tr;//税率
			decimal yf;//运费
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", Guid.NewGuid()),
				new SqlParameter("@code", (Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@arrivalQuantity", obj.数量),
				new SqlParameter("@quantity", obj.数量),
				new SqlParameter("@baseQuantity", obj.数量),
				new SqlParameter("@price", obj.单价),
				new SqlParameter("@basePrice", obj.单价),
				new SqlParameter("@baseEstimatedPrice",obj.单价),
				new SqlParameter("@amount",obj.金额),
				new SqlParameter("@estimatedAmount", obj.金额),
				new SqlParameter("@taxRate", (double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@taxPrice", obj.含税单价),
				new SqlParameter("@tax", obj.税额),
				new SqlParameter("@taxAmount",obj.含税金额),
				new SqlParameter("@totalAmount", obj.金额),
				//new SqlParameter("@sourceVoucherId", new Guid("bfd7be52-f264-4662-940c-d32e68ffbe9b")),
				//new SqlParameter("@sourceVoucherCode", "PO-2013-06-0002"),
				//new SqlParameter("@sourceVoucherDetailId", new Guid("43b41201-2820-4efb-bafb-f00443449c3c")),
				new SqlParameter("@isManualCost", Convert.ToByte(0)),
				new SqlParameter("@isCostAccounted", Convert.ToByte(0)),
				new SqlParameter("@taxFlag", Convert.ToByte(0)),
				new SqlParameter("@isNoModify",
					"#Inventory#Inventory_Code#VendorInventoryPrice#VendorInventoryName#Warehouse#Project#InvBarCode#Unit#Unit2#TaxRate#ReceiveVoucherCode#"),
				//new SqlParameter("@createdtime", "2014-12-04 14:37:41"),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				//new SqlParameter("@updated", "2014-12-04 14:37:41"),
				new SqlParameter("@idRDRecordDTO", pid),
				new SqlParameter("@idsourcevouchertype", new Guid("33d9eacd-ac90-45c4-a63c-2a65a1cef7a0")),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				//new SqlParameter("@VendorInventoryPrice", ""),
				//new SqlParameter("@InvBarCode", ""),
				//new SqlParameter("@SourceVoucherIdByMergedFlow", new Guid("bfd7be52-f264-4662-940c-d32e68ffbe9b")),
				//new SqlParameter("@SourceVoucherCodeByMergedFlow", "PO-2013-06-0002"),
				//new SqlParameter("@SourceVoucherDetailIdByMergedFlow", new Guid("43b41201-2820-4efb-bafb-f00443449c3c")),
				//new SqlParameter("@idbusiTypeByMergedFlow", new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				//new SqlParameter("@idsourceVoucherTypeByMergedFlow", new Guid("33d9eacd-ac90-45c4-a63c-2a65a1cef7a0")),
				//new SqlParameter("@SourceOrderDetailId", new Guid("43b41201-2820-4efb-bafb-f00443449c3c")),
				//new SqlParameter("@VendorInventoryName", ""),
				//new SqlParameter("@PurchaseOrderCode", "PO-2013-06-0002"),
				new SqlParameter("@idProject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@origPrice",obj.单价),
				new SqlParameter("@origAmount", obj.金额),
				new SqlParameter("@origTaxPrice", obj.含税单价),
				new SqlParameter("@origTax", obj.税额),
				new SqlParameter("@origTaxAmount", obj.含税金额),
				new SqlParameter("@IsPresent", Convert.ToByte(0)),
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
