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
				new SqlParameter("@id", id),
				new SqlParameter("@code", string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@dispatchAddress", ""),
				new SqlParameter("@contact", ""),
				new SqlParameter("@contactPhone", ""),
				new SqlParameter("@sourceVoucherId", new Guid("ff33ca2e-6554-42d5-a94a-24a7929cd67a")),
				//new SqlParameter("@sourceVoucherCode", "SO-2013-06-0001"),
				//new SqlParameter("@saleOrderCode", "SO-2013-06-0001"),
				new SqlParameter("@voucherState", TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@printTime", Convert.ToInt32(0)),
				new SqlParameter("@amount", Convert.ToDecimal(1112.65000000000000)),
				new SqlParameter("@rdDirectionFlag", Convert.ToByte(0)),
				new SqlParameter("@deliveryState", new Guid("3fa9f8c4-4d3f-4b90-be87-a18cde11ca8e")),
				new SqlParameter("@isCostAccount", Convert.ToByte(0)),
				new SqlParameter("@isMergedFlow", Convert.ToByte(0)),
				new SqlParameter("@isAutoGenerate", Convert.ToByte(0)),
				new SqlParameter("@memo", ""),
				new SqlParameter("@isNoModify", "#Partner#SettleCustomer#"),
				new SqlParameter("@voucherdate", DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@maker", "DEMO"),
				//new SqlParameter("@madedate", "2013-06-01 00:00:00"),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@makerid", new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@accountingperiod", Convert.ToInt32(6)),
				new SqlParameter("@accountingyear", Convert.ToInt32(2013)),
				new SqlParameter("@createdtime", "2014-12-04 13:44:20"),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				//new SqlParameter("@updated", "2014-12-04 13:44:19"),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@idpartner", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@idbusitype",new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@idvouchertype", new Guid("bb007f33-c0f0-4a19-8588-1e0e314d1f56")),
				new SqlParameter("@idsourcevouchertype", new Guid("dd4f2c12-2ba7-4a04-9e4d-4f5d42a0c193")),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idrdstyle", new Guid("2f384c02-e650-4421-81de-e870e6a8b0b3")),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@pubuserdefnvc1", obj.部门),
				new SqlParameter("@pubuserdefnvc2", obj.业务员),
				new SqlParameter("@pubuserdefnvc3", obj.退货日期),
				new SqlParameter("@pubuserdefnvc4", ""),
				//new SqlParameter("@VoucherYear", Convert.ToInt32(2013)),
				//new SqlParameter("@VoucherPeriod", Convert.ToInt32(6)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@idProject", new Guid("666374a4-6341-4b3a-96fc-a3930150f584")),
				new SqlParameter("@idSettleCustomer", new Guid("c8412a2e-d9f9-46d0-8fd4-a39a012e5099")),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@Address", ""),
				new SqlParameter("@priuserdefdecm1",decimal.TryParse(obj.抽佣比率,out cy)?cy:cy),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(OutputWarehouse obj, Guid pid)
		{
			decimal m;//金额
			double tr;//税率
			int d;//整数
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", Guid.NewGuid()),
				new SqlParameter("@code", (Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@arrivalQuantity", int.TryParse(obj.数量,out d)?d:d),
				new SqlParameter("@quantity", int.TryParse(obj.数量,out d)?d:d),
				new SqlParameter("@baseQuantity", int.TryParse(obj.数量,out d)?d:d),
				new SqlParameter("@price", decimal.TryParse(obj.单价,out m)?m:m),
				new SqlParameter("@basePrice", decimal.TryParse(obj.单价,out m)?m:m),
				new SqlParameter("@amount",decimal.TryParse(obj.成本金额,out m)?m:m),
				new SqlParameter("@taxRate", (double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@tax",decimal.TryParse( obj.税额,out m)?m:m),
				//new SqlParameter("@sourceVoucherId", new Guid("ff33ca2e-6554-42d5-a94a-24a7929cd67a")),
				//new SqlParameter("@sourceVoucherCode", "SO-2013-06-0001"),
				//new SqlParameter("@sourceVoucherDetailId", new Guid("2cbada64-a31f-4240-b386-3d452a195ea2")),
				//new SqlParameter("@saleOrderId", new Guid("ff33ca2e-6554-42d5-a94a-24a7929cd67a")),
				//new SqlParameter("@saleOrderCode", "SO-2013-06-0001"),
				//new SqlParameter("@saleOrderDetailId", new Guid("2cbada64-a31f-4240-b386-3d452a195ea2")),
				new SqlParameter("@isManualCost", Convert.ToByte(0)),
				new SqlParameter("@isCostAccounted", Convert.ToByte(0)),
				new SqlParameter("@taxFlag", Convert.ToByte(1)),
				new SqlParameter("@isNoModify",
					"#Inventory_Code#Inventory#CustomerInventoryPrice#PartnerInventoryName#Warehouse#Project#InvBarCode#Unit#Quantity2#ChangeRate#SourceVoucherCode#SourceVoucherType#SaleOrderCode#"),
				//new SqlParameter("@createdtime", "2014-12-04 13:44:20"),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				//new SqlParameter("@updated", "2014-12-04 13:44:19"),
				new SqlParameter("@idRDRecordDTO", pid),
				new SqlParameter("@idsourcevouchertype", new Guid("dd4f2c12-2ba7-4a04-9e4d-4f5d42a0c193")),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@pubuserdefnvc1", obj.物流名称单号),
				new SqlParameter("@pubuserdefnvc2", obj.发货信息),
				new SqlParameter("@pubuserdefnvc3", obj.客户收货信息),
				new SqlParameter("@InvBarCode", ""),
				new SqlParameter("@pubuserdefnvc4", obj.平台单号),
				//new SqlParameter("@SourceVoucherIdByMergedFlow", new Guid("ff33ca2e-6554-42d5-a94a-24a7929cd67a")),
				//new SqlParameter("@SourceVoucherCodeByMergedFlow", "SO-2013-06-0001"),
				//new SqlParameter("@SourceVoucherDetailIdByMergedFlow", new Guid("2cbada64-a31f-4240-b386-3d452a195ea2")),
				new SqlParameter("@idbusiTypeByMergedFlow", new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98")),
				new SqlParameter("@idsourceVoucherTypeByMergedFlow", new Guid("dd4f2c12-2ba7-4a04-9e4d-4f5d42a0c193")),
				//new SqlParameter("@SourceOrderDetailId", new Guid("2cbada64-a31f-4240-b386-3d452a195ea2")),
				new SqlParameter("@idProject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@origPrice", decimal.TryParse(obj.售价,out m)?m:m),
				new SqlParameter("@origTax", decimal.TryParse( obj.税额,out m)?m:m),
				new SqlParameter("@origSalePrice", decimal.TryParse(obj.售价,out m)?m:m),
				new SqlParameter("@origTaxSalePrice", decimal.TryParse(obj.含税售价,out m)?m:m),
				new SqlParameter("@origSaleAmount", decimal.TryParse(obj.销售金额,out m)?m:m),
				new SqlParameter("@origTaxSaleAmount", decimal.TryParse(obj.含税销售金额,out m)?m:m),
				new SqlParameter("@salePrice", decimal.TryParse(obj.售价,out m)?m:m),
				new SqlParameter("@taxSalePrice",decimal.TryParse(obj.含税售价,out m)?m:m),
				new SqlParameter("@saleAmount", decimal.TryParse(obj.销售金额,out m)?m:m),
				new SqlParameter("@taxSaleAmount", decimal.TryParse(obj.含税销售金额,out m)?m:m),
				new SqlParameter("@IsPresent", Convert.ToByte(0)),
				new SqlParameter("@pubuserdefdecm4",decimal.TryParse(obj.抽佣,out m)?m:m),
				new SqlParameter("@priuserdefnvc1",obj.销售订单号),
				new SqlParameter("@pubuserdefdecm1",decimal.TryParse(obj.满减活动,out m)?m:0m),
				new SqlParameter("@pubuserdefdecm2",decimal.TryParse(obj.抵用券,out m)?m:0m),
				new SqlParameter("@pubuserdefdecm3",decimal.TryParse(obj.代收运费,out m)?m:0m),
			};

			return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps) };
		}

		protected override bool CanExport(OutputWarehouse obj, out IEnumerable<string> msgs)
		{
			var list = new List<string>();
			if (TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]仓库不存在");
			}
			if (TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]所属公司不存在");
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
