using System;
using System.Collections.Generic;
using System.Data;
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
	/// 请购单数据库导出提供程序
	/// </summary>
	class PurchaseRequisitionDatabaseExportProvider : BaseDatabaseExportProvider<PurchaseRequisition>
	{
		protected override string VoucherName
		{
			get { return "请购单"; }
		}
		protected override string VoucherTable
		{
			get { return "Pu_PurchaseRequisition"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(PurchaseRequisition obj, out Guid id)
		{
			id = Guid.NewGuid();

			var dbParams = new List<DbParameter>
			{
				new SqlParameter("@id",id),
				new SqlParameter("@makerid",new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@OrigTotalAmount",DBNull.Value),
				new SqlParameter("@iscarriedforwardin",false),
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@madedate",DBNull.Value),
				new SqlParameter("@auditor",""),
				new SqlParameter("@maker","DEMO"),
				new SqlParameter("@OrigTotalTaxAmount",DBNull.Value),
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@PrintCount",Convert.ToInt32(0)),
				new SqlParameter("@accountingperiod",Convert.ToInt32(0)),
				new SqlParameter("@accountingyear",Convert.ToInt32(0)),
				new SqlParameter("@idrequisitionperson",TplusDatabaseHelper.Instance.GetPensonIdByDepartmentName(obj.所属公司)),
				//new SqlParameter("@TotalAmount",396.00m),
				new SqlParameter("@idmarketingOrgan",new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@requireDate",DBNull.Value),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@ismodifiedcode",false),
				new SqlParameter("@priuserdefnvc2",obj.仓库),
				new SqlParameter("@pubuserdefnvc4",""),
				new SqlParameter("@memo",""),
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@updated",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@acceptAddress",""),
				new SqlParameter("@priuserdefnvc1",obj.供应商),
				new SqlParameter("@iscarriedforwardout",false),
				new SqlParameter("@pubuserdefnvc1",obj.部门),
				//new SqlParameter("@TotalTaxAmount",463.32m),
				new SqlParameter("@reviser",""),
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目))
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, dbParams);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(PurchaseRequisition obj, Guid pid)
		{
			double tr;//税率
			var dbParams = new List<DbParameter>
			{
				new SqlParameter("@id",Guid.NewGuid()),
				new SqlParameter("@inventoryBarCode",""),
				new SqlParameter("@unitExchangeRate",DBNull.Value),
				new SqlParameter("@subQuantity",DBNull.Value),
				new SqlParameter("@discountRate",1),
				new SqlParameter("@tax",obj.税额),
				new SqlParameter("@quantity2",DBNull.Value),
				new SqlParameter("@cumExecuteQuantity",DBNull.Value),
				//new SqlParameter("@baseQuantity",6.00),
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@praRequireTimes",Convert.ToInt32(0)),
				new SqlParameter("@subCumExecuteQuantity",DBNull.Value),
				new SqlParameter("@compositionQuantity",""),
				new SqlParameter("@price",DBNull.Value),
				new SqlParameter("@saleOrderCode",""),
				new SqlParameter("@availableQuantity",DBNull.Value),
				new SqlParameter("@requireDate",DBNull.Value),
				new SqlParameter("@cumExecuteQuantity2",DBNull.Value),
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@discount",DBNull.Value),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@taxFlag",Convert.ToInt32(0)),
				new SqlParameter("@idbaseunit",new Guid("3c390f6c-e76a-439d-8d14-a37b01447494")),
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@sequencenumber",Convert.ToInt32(0)),
				new SqlParameter("@taxPrice",obj.含税单价),
				new SqlParameter("@islaborcost",false),
				new SqlParameter("@quantity",obj.数量),
				new SqlParameter("@baseCumExecuteQuantity",DBNull.Value),
				new SqlParameter("@discountAmount",obj.金额),
				new SqlParameter("@existingQuantity",DBNull.Value),
				new SqlParameter("@discountPrice",obj.单价),
				new SqlParameter("@idPurchaseRequisitionDTO",pid),
				new SqlParameter("@taxAmount",obj.含税金额),
				new SqlParameter("@updated",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@isPraRequire",false),
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
			};

			return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", dbParams) };
		}

		protected override bool CanExport(PurchaseRequisition obj, out IEnumerable<string> msgs)
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
