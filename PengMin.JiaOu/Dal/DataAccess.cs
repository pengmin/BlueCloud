using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PengMin.Infrastructure;

namespace PengMin.JiaOu.Dal
{
	class DataAccess
	{
		protected string Prefix;//单据编码前缀
		protected int Serialno = 0;//单据编码起始编号
		protected int Length;//单据编号长度
		protected int Code;//明细的编号
		private readonly SqlHelper _sqlHelper;

		public DataAccess(SqlHelper sqlHelper)
		{
			_sqlHelper = sqlHelper;
		}
		/// <summary>
		/// 获取采购订单摘要
		/// </summary>
		/// <returns></returns>
		public DataTable GetPurchaseOrder()
		{
			var sql = @"SELECT a.id, a.VoucherDate AS 单据日期 ,
		a.code AS 单据编号 ,
		b.name AS 供应商 ,
		c.name AS 业务员 ,
		a.acceptDate AS 预计到货日期 ,
		d.Name AS 付款方式 ,
		a.origEarnestMoney AS 订金金额 ,
		a.pubuserdefnvc1 AS 预付款百分比
FROM    PU_PurchaseOrder AS a
		JOIN dbo.AA_Partner AS b ON b.id = a.idpartner
		LEFT JOIN dbo.AA_Person AS c ON c.id = a.idclerk
		LEFT JOIN dbo.eap_EnumItem AS d ON d.id = a.payType";

			_sqlHelper.Open();
			var r = _sqlHelper.GetDataTable(sql);
			_sqlHelper.Close();

			return r;
		}
		/// <summary>
		/// 获取采购订单信息
		/// </summary>
		/// <param name="id">订单id</param>
		/// <param name="main">主订单</param>
		/// <param name="detail">明细订单</param>
		public void GetPurchaseOrder(Guid id, out DataTable main, out DataTable detail)
		{
			_sqlHelper.Open();
			main = _sqlHelper.GetDataTable("select * from PU_PurchaseOrder where id=@id", new SqlParameter("@id", id));
			detail = _sqlHelper.GetDataTable("select * from PU_PurchaseOrder_b where idPurchaseOrderDTO=@pid",
				new SqlParameter("@pid", id));
			_sqlHelper.Close();
		}

		/// <summary>
		/// 将采购订单转换成销售订单
		/// </summary>
		/// <param name="data">采购订单数据</param>
		/// <param name="customer">采购订单来源客户</param>
		/// <returns>销售订单数据</returns>
		public static DataTable PurchaseOrderToSaleOrder(DataTable data, string customer)
		{
			var saleOrder = new DataTable();
			saleOrder.Columns.Add("单据日期");
			saleOrder.Columns.Add("单据编号");
			saleOrder.Columns.Add("客户");
			saleOrder.Columns.Add("结算客户");
			saleOrder.Columns.Add("业务员");
			saleOrder.Columns.Add("预计交货日期");
			saleOrder.Columns.Add("收款方式");
			saleOrder.Columns.Add("订金金额");
			saleOrder.Columns.Add("预付款百分比");
			saleOrder.Columns.Add("金额1");
			saleOrder.Columns.Add("含税金额1");
			saleOrder.Columns.Add("存货编码");
			saleOrder.Columns.Add("存货名称");
			saleOrder.Columns.Add("规格型号");
			saleOrder.Columns.Add("单位");
			saleOrder.Columns.Add("数量");
			saleOrder.Columns.Add("单价");
			saleOrder.Columns.Add("税率");
			saleOrder.Columns.Add("含税单价");
			saleOrder.Columns.Add("金额");
			saleOrder.Columns.Add("含税金额");
			saleOrder.Columns.Add("税额");
			foreach (DataRow row in data.Rows)
			{
				var newRow = saleOrder.NewRow();
				saleOrder.Rows.Add(newRow);
				foreach (DataColumn cln in data.Columns)
				{
					if (saleOrder.Columns[cln.ColumnName] != null)
					{
						newRow[cln.ColumnName] = row[cln];
					}
					else if (cln.ColumnName == "预计到货日期")
					{
						newRow["预计交货日期"] = row[cln];
					}
				}
				newRow["客户"] = newRow["结算客户"] = customer;
			}

			return saleOrder;
		}
		/// <summary>
		/// 导入采购订单
		/// </summary>
		/// <param name="ids">要导出的采购订单id</param>
		/// <returns></returns>
		public DataTable ImportPurchaseOrder(params Guid[] ids)
		{
			if (ids.Length == 0) return null;

			var sql = @"SELECT a.voucherdate AS [单据日期],
a.code AS [单据编号],
c.name AS [供应商],
d.name AS [业务员],
a.acceptDate AS [预计到货日期],
e.Name AS [付款方式],
a.origEarnestMoney AS [订金金额],
a.SaleOrderCode AS [销售订单号],
a.pubuserdefnvc1 AS [预付款百分比],
a.origTotalAmount AS [金额1],
a.origTotalTaxAmount AS [含税金额1],
f.code AS [存货编码],
f.name AS [存货名称],
f.specification AS [规格型号],
g.name AS [单位],
b.Quantity AS [数量],
b.OrigDiscountPrice AS [单价],
b.TaxRate AS [税率],
b.OrigTaxPrice AS [含税单价],
b.OrigDiscountAmount AS [金额],
b.OrigTaxAmount AS [含税金额],
b.origTax AS [税额]
FROM PU_PurchaseOrder AS a
JOIN PU_PurchaseOrder_b AS b ON b.idPurchaseOrderDTO=a.id
JOIN dbo.AA_Partner AS c ON c.id=a.idpartner
JOIN dbo.AA_Person AS d ON d.id=a.idclerk
JOIN dbo.eap_EnumItem AS e ON e.id=a.payType
JOIN AA_Inventory AS f ON f.id=b.idinventory
JOIN dbo.AA_Unit AS g ON g.id=b.idunit
WHERE a.id=@id";
			_sqlHelper.Open();
			var dt = _sqlHelper.GetDataTable(sql, new SqlParameter("@id", ids[0]));
			_sqlHelper.Close();

			return dt;
		}
		/// <summary>
		/// 导出销售订单
		/// </summary>
		/// <param name="data">销售订单数据</param>
		/// <returns>导出结果</returns>
		public IEnumerable<string> ExportSaleOrder(DataTable data)
		{
			var tplus = TplusDatabaseHelper.GetInstance(_sqlHelper);
			Prefix = tplus.GetVoucherCodePrefix("销售订单", out Length);
			Serialno = tplus.GetMaxSerialno("SA_SaleOrder", Length);
			var id = Guid.NewGuid();
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();

			var sqlInfo = BuildMainSql(data.Rows[0], id);
			sqlList.Add(new Tuple<string, IEnumerable<DbParameter>>(BuildSql(sqlInfo), sqlInfo.Item2));
			foreach (DataRow row in data.Rows)
			{
				sqlInfo = BuildDetailSql(row, id);
				sqlList.Add(new Tuple<string, IEnumerable<DbParameter>>(BuildSql(sqlInfo), sqlInfo.Item2));
				sqlInfo = BuildCurrentStockSql(row);
				sqlList.Add(new Tuple<string, IEnumerable<DbParameter>>(BuildSql(sqlInfo), sqlInfo.Item2));
			}

			_sqlHelper.Open();
			var msgs = new[] { _sqlHelper.Execute(sqlList).ToString() };
			_sqlHelper.Close();
			return msgs;
		}

		private Tuple<string, IEnumerable<DbParameter>> BuildMainSql(DataRow row, Guid id)
		{
			var tplus = TplusDatabaseHelper.GetInstance(_sqlHelper);
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", id),
				new SqlParameter("@code", Prefix+(++Serialno).ToString().PadLeft(Length,'0')),
				new SqlParameter("@deliveryDate",row["预计交货日期"]),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@address", ""),
				new SqlParameter("@linkMan", ""),
				new SqlParameter("@reciveType", new Guid("ef370928-98f5-4312-a1c6-021587d17af1")),
				new SqlParameter("@contractCode", ""),
				new SqlParameter("@origEarnestMoney", Convert.ToDecimal(row["订金金额"])),
				new SqlParameter("@earnestMoney", Convert.ToDecimal(row["订金金额"])),
				new SqlParameter("@voucherState", new Guid("d6c5e975-900d-40d3-aef0-5d189d230fb1")),
				new SqlParameter("@memo", ""),
				new SqlParameter("@origAmount", Convert.ToDecimal(row["金额1"])),
				new SqlParameter("@amount", Convert.ToDecimal(row["金额1"])),
				new SqlParameter("@origTaxAmount", Convert.ToDecimal(row["含税金额1"])),
				new SqlParameter("@taxAmount", Convert.ToDecimal(row["含税金额1"])),
				new SqlParameter("@contactPhone", ""),
				new SqlParameter("@voucherdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@maker", "demo"),
				new SqlParameter("@madedate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@makerid", new Guid("d3111860-89a2-4a85-8e27-a38400fd2718")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@idclerk", tplus.GetPensonIdByDepartmentName(row["业务员"].ToString())),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@accountingperiod", Convert.ToInt32(0)),
				new SqlParameter("@idcustomer", tplus.GetPartnerIdByName(row["客户"].ToString())),
				//new SqlParameter("@iddepartment", tplus.GetDepartmentIdByName(row["部门"].ToString())),
				new SqlParameter("@accountingyear", Convert.ToInt32(0)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@idbusinesstype", new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@pubuserdefnvc1", row["预付款百分比"]),
				new SqlParameter("@IsAutoGenerateSaleOrderBOM", Convert.ToByte(0)),
				new SqlParameter("@IsAutoGenerateRouting", Convert.ToByte(0)),
				new SqlParameter("@idsettlecustomer", new Guid("d4e94210-4594-43d2-9f21-a38e00f4920d")),
				new SqlParameter("@changer", ""),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@DataSource", new Guid("3d362676-d6d5-4439-9be5-40c67514b9f5")),
				new SqlParameter("@referenceCount", Convert.ToInt32(0)),
				new SqlParameter("@MemberAddress", ""),
				new SqlParameter("@PrintCount", Convert.ToInt32(0)),
			};
			return new Tuple<string, IEnumerable<DbParameter>>("SA_SaleOrder", ps);
		}

		private Tuple<string, IEnumerable<DbParameter>> BuildDetailSql(DataRow row, Guid pid)
		{
			var tplus = TplusDatabaseHelper.GetInstance(_sqlHelper);
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", Guid.NewGuid()),
				new SqlParameter("@code", (Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@quantity", Convert.ToDecimal(row["数量"])),
				new SqlParameter("@baseQuantity", Convert.ToDecimal(row["数量"])),
				new SqlParameter("@origPrice", Convert.ToDecimal(row["含税单价"])),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@origDiscountPrice", Convert.ToDecimal(row["单价"])),
				new SqlParameter("@discountPrice", Convert.ToDecimal(row["单价"])),
				new SqlParameter("@taxRate", Convert.ToDecimal(row["税率"])),
				new SqlParameter("@origTaxPrice", Convert.ToDecimal(row["含税单价"])),
				new SqlParameter("@taxPrice", Convert.ToDecimal(row["含税单价"])),
				new SqlParameter("@origDiscountAmount", Convert.ToDecimal(row["金额"])),
				new SqlParameter("@discountAmount", Convert.ToDecimal(row["金额"])),
				new SqlParameter("@origTax", Convert.ToDecimal(row["税额"])),
				new SqlParameter("@tax", Convert.ToDecimal(row["税额"])),
				new SqlParameter("@origTaxAmount", Convert.ToDecimal(row["含税金额"])),
				new SqlParameter("@taxAmount", Convert.ToDecimal(row["含税金额"])),
				new SqlParameter("@deliveryDate", row["预计交货日期"]),
				new SqlParameter("@isPresent", Convert.ToByte(0)),
				new SqlParameter("@origDiscount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@discount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@taxFlag", Convert.ToByte(1)),
				new SqlParameter("@referenceCount", Convert.ToInt32(0)),
				new SqlParameter("@idinventory", tplus.GetInventoryIdByCode(row["存货编码"].ToString())),
				new SqlParameter("@inventoryBarCode", ""),
				new SqlParameter("@idunit", tplus.GetUnitIdByName(row["单位"].ToString())),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@idbaseunit", tplus.GetUnitIdByName(row["单位"].ToString())),
				new SqlParameter("@idSaleOrderDTO", pid),
				new SqlParameter("@HasMRP", Convert.ToByte(0)),
				new SqlParameter("@HasPRA", Convert.ToByte(0)),
				new SqlParameter("@prarequiretimes", Convert.ToInt32(0)),
				new SqlParameter("@mrprequiretimes", Convert.ToInt32(0)),
				new SqlParameter("@PriceStrategyTypeName", "存货批发价"),
				new SqlParameter("@PriceStrategyTypeId", new Guid("e870c22a-2adb-47b0-8e87-fb642c2f08d3")),
				new SqlParameter("@PriceStrategySchemeIds", ""),
				new SqlParameter("@PriceStrategySchemeNames", ""),
				new SqlParameter("@PromotionVoucherCodes", ""),
				new SqlParameter("@PromotionVoucherIds", ""),
				new SqlParameter("@IsMemberIntegral", Convert.ToByte(0)),
				new SqlParameter("@IsPromotionPresent", Convert.ToByte(0)),
				new SqlParameter("@PromotionSingleVoucherCode", ""),
			};
			return new Tuple<string, IEnumerable<DbParameter>>("SA_SaleOrder_b", ps);
		}

		private Tuple<string, IEnumerable<DbParameter>> BuildCurrentStockSql(DataRow obj)
		{
			int d;
			return new Tuple<string, IEnumerable<DbParameter>>(
				"ST_CurrentStock",
				new DbParameter[]
				{
					new SqlParameter("@id", Guid.NewGuid()),
					new SqlParameter("@purchaseForReceiveBaseQuantity", int.TryParse(obj["数量"].ToString(),out d)?d:d),
					new SqlParameter("@recordDate", DateTime.Now.ToString("yyyy-MM-dd")),
					new SqlParameter("@isCarriedForwardOut", Convert.ToByte(0)),
					new SqlParameter("@isCarriedForwardIn", Convert.ToByte(0)),
					new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
					//new SqlParameter("@ts", "System.Byte[]"),
					new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@updatedBy", "demo"),
					//new SqlParameter("@idwarehouse", TplusDatabaseHelper.GetInstance(_sqlHelper).GetWarehouseIdByName(obj.仓库)),
					new SqlParameter("@idbaseunit",TplusDatabaseHelper.GetInstance(_sqlHelper).GetUnitIdByName(obj["单位"].ToString())),
					new SqlParameter("@idinventory", TplusDatabaseHelper.GetInstance(_sqlHelper).GetInventoryIdByCode(obj["存货编码"].ToString())),
					new SqlParameter("@IdMarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				});
		}

		private static string BuildSql(Tuple<string, IEnumerable<DbParameter>> sqlInfo)
		{
			var sql1 = "INSERT INTO " + sqlInfo.Item1 + "(";
			var sql2 = " VALUES(";
			foreach (var param in sqlInfo.Item2)
			{
				sql1 += param.ParameterName.TrimStart('@') + ",";
				sql2 += param.ParameterName + ",";
			}
			sql1 = sql1.TrimEnd(',') + ")";
			sql2 = sql2.TrimEnd(',') + ")";
			return sql1 + sql2 + ";";
		}
	}
}
