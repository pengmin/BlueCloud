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
	/// 销售订单数据库导出提供程序
	/// </summary>
	class SaleOrderDatabaseExportProvider : BaseDatabaseExportProvider<SaleOrder>
	{
		protected override string VoucherName
		{
			get { return "销售订单"; }
		}
		protected override string VoucherTable
		{
			get { return "SA_SaleOrder"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(SaleOrder obj, Guid id)
		{
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", id),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				//new SqlParameter("@deliveryDate", "2013-06-25 00:00:00"),//预计交货日期
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@address", ""),
				new SqlParameter("@linkMan", ""),
				new SqlParameter("@reciveType", new Guid("ef370928-98f5-4312-a1c6-021587d17af1")),
				new SqlParameter("@contractCode", ""),
				new SqlParameter("@origEarnestMoney", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@earnestMoney", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@voucherState", TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@memo", ""),
				//new SqlParameter("@origAmount", Convert.ToDecimal(46397.44000000000000)),//金额
				//new SqlParameter("@amount", Convert.ToDecimal(46397.44000000000000)),//金额
				//new SqlParameter("@origTaxAmount", Convert.ToDecimal(54285.00000000000000)),//含税金额
				//new SqlParameter("@taxAmount", Convert.ToDecimal(54285.00000000000000)),//含税金额
				new SqlParameter("@contactPhone", ""),
				new SqlParameter("@voucherdate", DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@maker", "DEMO"),
				//new SqlParameter("@madedate", "2013-06-01 00:00:00"),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@makerid", new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@accountingperiod", Convert.ToInt32(0)),
				new SqlParameter("@idcustomer", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@iddepartment", TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@accountingyear", Convert.ToInt32(0)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				//new SqlParameter("@updated", "2014-12-04 11:40:05"),
				new SqlParameter("@idbusinesstype", new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@pubuserdefnvc1", obj.部门),
				new SqlParameter("@pubuserdefnvc2", obj.业务员),
				new SqlParameter("@pubuserdefnvc4", ""),
				new SqlParameter("@IsAutoGenerateSaleOrderBOM", Convert.ToByte(0)),
				new SqlParameter("@IsAutoGenerateRouting", Convert.ToByte(0)),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@idsettlecustomer", new Guid("c8412a2e-d9f9-46d0-8fd4-a39a012e5099")),
				new SqlParameter("@changer", ""),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@DataSource", new Guid("3d362676-d6d5-4439-9be5-40c67514b9f5")),
				new SqlParameter("@MemberAddress", ""),
				new SqlParameter("@PrintCount", Convert.ToInt32(0)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(SaleOrder obj, Guid pid)
		{
			double tr;//税率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", Guid.NewGuid()),
				new SqlParameter("@code", (Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@quantity", obj.数量),
				new SqlParameter("@baseQuantity", obj.数量),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@origDiscountPrice", obj.单价),
				new SqlParameter("@discountPrice",obj.单价),
				new SqlParameter("@taxRate", (double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@origTaxPrice", obj.含税单价),
				new SqlParameter("@taxPrice", obj.含税单价),
				new SqlParameter("@origDiscountAmount", obj.金额),
				new SqlParameter("@discountAmount", obj.金额),
				new SqlParameter("@origTax",obj.税额),
				new SqlParameter("@tax", obj.税额),
				new SqlParameter("@origTaxAmount", obj.含税金额),
				new SqlParameter("@taxAmount", obj.含税金额),
				//new SqlParameter("@deliveryDate", "2013-06-25 00:00:00"),
				new SqlParameter("@isPresent", Convert.ToByte(0)),
				new SqlParameter("@taxFlag", Convert.ToByte(1)),
				new SqlParameter("@referenceCount", Convert.ToInt32(0)),
				new SqlParameter("@lastmodifiedfield", ""),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@inventoryBarCode", ""),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.销售单位)),
				//new SqlParameter("@ts", "System.Byte[]"),
				//new SqlParameter("@updated", "2014-12-04 11:40:05"),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.销售单位)),
				new SqlParameter("@idSaleOrderDTO", pid),
				//new SqlParameter("@pubuserdefnvc1", "001"),
				//new SqlParameter("@pubuserdefnvc2", "002"),
				//new SqlParameter("@pubuserdefnvc3", "003"),
				//new SqlParameter("@pubuserdefnvc4", "004"),
				new SqlParameter("@HasMRP", Convert.ToByte(0)),
				new SqlParameter("@HasPRA", Convert.ToByte(0)),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@prarequiretimes", Convert.ToInt32(0)),
				new SqlParameter("@mrprequiretimes", Convert.ToInt32(0)),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
			};

			return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps) };
		}

		protected override bool CanExport(SaleOrder obj, out IEnumerable<string> msgs)
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
			if (TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]项目不存在");
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
