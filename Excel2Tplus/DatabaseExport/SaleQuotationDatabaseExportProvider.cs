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
	/// 报价单数据库导出提供程序
	/// </summary>
	class SaleQuotationDatabaseExportProvider : BaseDatabaseExportProvider<SaleQuotation>
	{
		protected override string VoucherName
		{
			get { return "报价单"; }
		}

		protected override string VoucherTable
		{
			get { return "SA_SaleQuotation"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(SaleQuotation obj, out Guid id)
		{
			id = Guid.NewGuid();

			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id),
				new SqlParameter("@makerid",new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@iscarriedforwardin",false),
				//new SqlParameter("@amount",184.00),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@madedate",Convert.ToDateTime("2013-06-01 00:00:00")),
				new SqlParameter("@transactionFlag",new Guid("a7204ebf-87d9-405e-a5d0-aefa4ff2b22d")),
				new SqlParameter("@auditor",""),
				new SqlParameter("@discountRate",1),
				//new SqlParameter("@origTaxAmount",184.00),
				new SqlParameter("@pubuserdefnvc2",""),
				new SqlParameter("@MemberAddress",""),
				new SqlParameter("@PrintCount",Convert.ToInt32(0)),
				new SqlParameter("@accountingperiod",Convert.ToInt32(0)),
				new SqlParameter("@maker","DEMO"),
				new SqlParameter("@idmarketingOrgan",new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@voucherstate",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@ismodifiedcode",false),
				//new SqlParameter("@origamount",184.00),
				new SqlParameter("@Mobilephone",DBNull.Value),
				new SqlParameter("@pubuserdefnvc4",""),
				new SqlParameter("@memo",""),
				new SqlParameter("@idcurrency",new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@origdiscountamount",DBNull.Value),
				new SqlParameter("@accountingyear",Convert.ToInt32(0)),
				//new SqlParameter("@updated",Convert.ToDateTime("2014-11-26 20:54:12")),
				new SqlParameter("@discountamount",DBNull.Value),
				new SqlParameter("@iscarriedforwardout",false),
				//new SqlParameter("@taxamount",184.00),
				new SqlParameter("@pubuserdefnvc1",""),
				new SqlParameter("@exchangeRate",1),
				new SqlParameter("@reviser",""),
				new SqlParameter("@idcustomer",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)), 
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)), 
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(SaleQuotation obj, Guid pid)
		{
			double tr;//税率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@UnitExchangeRate",DBNull.Value),
				new SqlParameter("@idSaleQuotationDTO",pid),
				new SqlParameter("@DiscountRate",1),
				new SqlParameter("@origTaxAmount",obj.含税金额),
				new SqlParameter("@tax",obj.税额),
				new SqlParameter("@origDiscount",DBNull.Value),
				new SqlParameter("@lastModifiedField",""),
				new SqlParameter("@taxAmount",obj.含税金额),
				new SqlParameter("@price",DBNull.Value),
				new SqlParameter("@origDiscountPrice",obj.单价),
				new SqlParameter("@basequantity",DBNull.Value),
				new SqlParameter("@saleOrderIssueQuantity",DBNull.Value),
				new SqlParameter("@quantity2",DBNull.Value),
				new SqlParameter("@discount",DBNull.Value),
				new SqlParameter("@saleOrderIssueQuantity2",DBNull.Value),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@origTax",Convert.ToInt32(0)),
				new SqlParameter("@discountPrice",obj.单价),
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@origTaxPrice",obj.含税单价),
				new SqlParameter("@origDiscountAmount",obj.金额),
				new SqlParameter("@taxPrice",obj.含税单价),
				new SqlParameter("@origPrice",DBNull.Value),
				new SqlParameter("@quantity",obj.数量),
				new SqlParameter("@subquantity",DBNull.Value),
				new SqlParameter("@discountAmount",obj.金额),
				new SqlParameter("@taxFlag",Convert.ToInt32(0)),
				//new SqlParameter("@updated",Convert.ToDateTime("2014-11-26 21:25:01")),
				new SqlParameter("@createdtime",DateTime.Now), 
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.销售单位)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps);
		}

		protected override bool CanExport(SaleQuotation obj, out IEnumerable<string> msgs)
		{
			var list = new List<string>();
			if (TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司) is DBNull)
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
