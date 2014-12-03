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

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(SaleOrder obj, out Guid id)
		{
			id = Guid.NewGuid();

			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id),
				new SqlParameter("@makerid",new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reciveType",new Guid("ef370928-98f5-4312-a1c6-021587d17af1")),
				new SqlParameter("@iscarriedforwardin",false),
				//new SqlParameter("@amount",6515.38),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@madedate",DateTime.Parse("2013-06-01 00:00:00")),
				new SqlParameter("@auditor",""),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@MemberAddress",""),
				new SqlParameter("@PrintCount",Convert.ToInt32(0)),
				new SqlParameter("@accountingperiod",Convert.ToInt32(0)),
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@DataSource",new Guid("3d362676-d6d5-4439-9be5-40c67514b9f5")),
				new SqlParameter("@accountingyear",Convert.ToInt32("0")),
				new SqlParameter("@origEarnestMoney",Convert.ToInt32(0)),
				new SqlParameter("@idmarketingOrgan",new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@ismodifiedcode",false),
				//new SqlParameter("@origAmount",6515.38),
				new SqlParameter("@idcustomer",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@earnestMoney",Convert.ToInt32(0)),
				new SqlParameter("@idbusitype",!string.IsNullOrWhiteSpace(obj.退货日期)?new Guid("2F62ED3A-2D43-4B1C-AB69-F838DAEC7F5B"):new Guid("9FC6F8D2-352E-4A97-B03E-0D33AA0BA593")),
				new SqlParameter("@Mobilephone",DBNull.Value),
				new SqlParameter("@pubuserdefnvc4",""),
				new SqlParameter("@memo",""),
				new SqlParameter("@idcurrency",new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@origDiscountAmount",DBNull.Value),
				new SqlParameter("@changer",""),
				new SqlParameter("@isautogeneratesaleorderbom",false),
				new SqlParameter("@idsettlecustomer",new Guid("c8412a2e-d9f9-46d0-8fd4-a39a012e5099")),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-26 21:56:03")),
				new SqlParameter("@contractCode",""),
				new SqlParameter("@isautogeneraterouting",false),
				new SqlParameter("@address",""),
				new SqlParameter("@contactPhone",""),
				new SqlParameter("@linkMan",""),
				new SqlParameter("@discountAmount",DBNull.Value),
				new SqlParameter("@iscarriedforwardout",Convert.ToInt32(0)),
				//new SqlParameter("@taxAmount",7623.00),
				new SqlParameter("@exchangeRate",1),
				new SqlParameter("@reviser",""),
				new SqlParameter("@maker","DEMO"),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@pubuserdefnvc1",obj.部门),
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(SaleOrder obj, Guid pid)
		{
			double tr;//税率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()),
				new SqlParameter("@basePrice",DBNull.Value),
				new SqlParameter("@inventoryBarCode",""),
				new SqlParameter("@unitExchangeRate",DBNull.Value),
				new SqlParameter("@executedQuantity",DBNull.Value),
				new SqlParameter("@manufactureQuantity",DBNull.Value),
				new SqlParameter("@subQuantity",DBNull.Value),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@prarequiretimes",Convert.ToInt32(0)),
				new SqlParameter("@origTaxAmount",obj.含税金额),
				new SqlParameter("@manufactureQuantity2",DBNull.Value),
				new SqlParameter("@deliveryQuantity",DBNull.Value),
				new SqlParameter("@quantity2",DBNull.Value),
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.销售单位)),
				//new SqlParameter("@baseQuantity",77.00),
				new SqlParameter("@lastmodifiedfield",""),
				new SqlParameter("@purchaseQuantity2",DBNull.Value),
				new SqlParameter("@taxAmount",obj.含税金额),
				new SqlParameter("@price",DBNull.Value),
				new SqlParameter("@origDiscountPrice",obj.单价),
				new SqlParameter("@idSaleOrderDTO",pid),
				new SqlParameter("@taxPrice",obj.含税单价),
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@saleOutQuantity2",DBNull.Value),
				new SqlParameter("@discount",DBNull.Value),
				new SqlParameter("@origDiscount",DBNull.Value),
				new SqlParameter("@baseDiscountPrice",DBNull.Value),
				new SqlParameter("@executedQuantity2",DBNull.Value),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@origTax", obj.税额),
				new SqlParameter("@discountPrice",obj.单价),
				new SqlParameter("@idbaseunit",new Guid("3c390f6c-e76a-439d-8d14-a37b01447494")),
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@deliveryQuantity2",DBNull.Value),
				new SqlParameter("@purchaseQuantity",DBNull.Value),
				new SqlParameter("@origTaxPrice",obj.含税单价),
				new SqlParameter("@origDiscountAmount",obj.金额),
				new SqlParameter("@isPresent",false),
				new SqlParameter("@origPrice",DBNull.Value),
				new SqlParameter("@quantity",obj.数量),
				new SqlParameter("@tax",obj.税额),
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@hasmrp",Convert.ToInt32(0)),
				new SqlParameter("@mrprequiretimes",Convert.ToInt32(0)),
				new SqlParameter("@haspra",Convert.ToInt32(0)),
				new SqlParameter("@baseTaxPrice",DBNull.Value),
				new SqlParameter("@saleOutQuantity",DBNull.Value),
				new SqlParameter("@discountAmount", obj.金额),
				new SqlParameter("@taxFlag",1),
				//new SqlParameter("@updated",DateTime.Parse("2014-11-26 21:56:03")),
				new SqlParameter("@referenceCount",Convert.ToInt32(0)),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),

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
