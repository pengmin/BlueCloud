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
				new SqlParameter("@code", string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				//new SqlParameter("@deliveryDate", "2013-06-28 00:00:00"),//预计交货日期
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
				new SqlParameter("@origAmount",obj.金额),
				new SqlParameter("@amount", obj.金额),
				new SqlParameter("@origTaxAmount", obj.含税金额),
				new SqlParameter("@taxAmount", obj.含税金额),
				new SqlParameter("@contactPhone", ""),
				new SqlParameter("@voucherdate", DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@maker", "DEMO"),
				new SqlParameter("@madedate", "2013-06-01 00:00:00"),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@makerid", new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@accountingperiod", Convert.ToInt32(0)),
				new SqlParameter("@idcustomer", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@iddepartment",  TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@accountingyear", Convert.ToInt32(0)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@idbusinesstype", new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@pubuserdefnvc1", obj.部门),
				new SqlParameter("@pubuserdefnvc2",  obj.业务员),
				new SqlParameter("@pubuserdefnvc4", ""),
				new SqlParameter("@IsAutoGenerateSaleOrderBOM", Convert.ToByte(0)),
				new SqlParameter("@IsAutoGenerateRouting", Convert.ToByte(0)),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@idsettlecustomer", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@changer", ""),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@DataSource", new Guid("3d362676-d6d5-4439-9be5-40c67514b9f5")),
				new SqlParameter("@referenceCount", Convert.ToInt32(0)),
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
				new SqlParameter("@discountPrice", obj.单价),
				new SqlParameter("@taxRate", (double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@origTaxPrice", obj.含税单价),
				new SqlParameter("@taxPrice", obj.含税单价),
				new SqlParameter("@origDiscountAmount", obj.金额),
				new SqlParameter("@discountAmount",  obj.金额),
				new SqlParameter("@origTax", obj.税额),
				new SqlParameter("@tax",obj.税额),
				new SqlParameter("@origTaxAmount", obj.含税金额),
				new SqlParameter("@taxAmount", obj.含税金额),
				//new SqlParameter("@deliveryDate", "2013-06-28 00:00:00"),
				new SqlParameter("@isPresent", Convert.ToByte(0)),
				new SqlParameter("@taxFlag", Convert.ToByte(1)),
				new SqlParameter("@referenceCount", Convert.ToInt32(0)),
				new SqlParameter("@lastmodifiedfield", ""),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@inventoryBarCode", ""),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.销售单位)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.销售单位)),
				new SqlParameter("@idSaleOrderDTO", pid),
				new SqlParameter("@HasMRP", Convert.ToByte(0)),
				new SqlParameter("@HasPRA", Convert.ToByte(0)),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@prarequiretimes", Convert.ToInt32(0)),
				new SqlParameter("@mrprequiretimes", Convert.ToInt32(0)),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@PriceStrategyTypeName", ""),
				new SqlParameter("@PriceStrategySchemeIds", ""),
				new SqlParameter("@PriceStrategySchemeNames", ""),
				new SqlParameter("@PromotionVoucherCodes", ""),
				new SqlParameter("@PromotionVoucherIds", ""),
				new SqlParameter("@IsMemberIntegral", Convert.ToByte(0)),
				new SqlParameter("@IsPromotionPresent", Convert.ToByte(0)),
				new SqlParameter("@PromotionSingleVoucherCode", ""),
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
			if (TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]客户不存在");
			}
			if (TplusDatabaseHelper.Instance.GetParnerTypeByName(obj.客户) != "客户")
			{
				list.Add("单据[" + obj.单据编号 + "](" + obj.客户 + ")不是客户性质的往来单位");
			}
			msgs = list;
			return !msgs.Any();
		}
	}
}
