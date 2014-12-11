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
		/// <param name="main">主订单</param>
		/// <param name="detail">明细订单</param>
		/// <returns>转换的Sql</returns>
		public IEnumerable<Tuple<string, IEnumerable<DbParameter>>> PurchaseOrderToSaleOrder(DataTable main, DataTable detail)
		{
			var mainSql = new DbParameter[]
			{
				new SqlParameter("@id", new Guid("b237c1f3-8912-4bd6-ade0-18b92758a9ee")),
				new SqlParameter("@code", "SO-2014-12-0001"),
				new SqlParameter("@deliveryDate", "2014-12-30 00:00:00"),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@address", ""),
				new SqlParameter("@linkMan", ""),
				new SqlParameter("@reciveType", new Guid("ef370928-98f5-4312-a1c6-021587d17af1")),
				new SqlParameter("@contractCode", ""),
				new SqlParameter("@origEarnestMoney", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@earnestMoney", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@voucherState", new Guid("d6c5e975-900d-40d3-aef0-5d189d230fb1")),
				new SqlParameter("@memo", ""),
				new SqlParameter("@origAmount", Convert.ToDecimal(207589.74000000000000)),
				new SqlParameter("@amount", Convert.ToDecimal(207589.74000000000000)),
				new SqlParameter("@origTaxAmount", Convert.ToDecimal(242880.00000000000000)),
				new SqlParameter("@taxAmount", Convert.ToDecimal(242880.00000000000000)),
				new SqlParameter("@contactPhone", ""),
				new SqlParameter("@voucherdate", "2014-12-11 00:00:00"),
				new SqlParameter("@maker", "demo"),
				new SqlParameter("@madedate", "2014-12-11 00:00:00"),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@makerid", new Guid("d3111860-89a2-4a85-8e27-a38400fd2718")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@idclerk", new Guid("bfb41966-e118-45ff-b7ed-a38c00a9286c")),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@accountingperiod", Convert.ToInt32(0)),
				new SqlParameter("@idcustomer", new Guid("2b3f4764-2b5d-4efd-91c5-a39200b11d23")),
				new SqlParameter("@iddepartment", new Guid("78831598-ed71-46ff-b62a-a38400ff03c7")),
				new SqlParameter("@accountingyear", Convert.ToInt32(0)),
				new SqlParameter("@createdtime", "2014-12-11 16:47:37"),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", "2014-12-11 16:47:37"),
				new SqlParameter("@idbusinesstype", new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98")),
				new SqlParameter("@pubuserdefnvc1", "50%"),
				new SqlParameter("@IsAutoGenerateSaleOrderBOM", Convert.ToByte(0)),
				new SqlParameter("@IsAutoGenerateRouting", Convert.ToByte(0)),
				new SqlParameter("@idsettlecustomer", new Guid("d4e94210-4594-43d2-9f21-a38e00f4920d")),
				new SqlParameter("@changer", ""),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@DataSource", new Guid("3d362676-d6d5-4439-9be5-40c67514b9f5")),
				new SqlParameter("@MemberAddress", ""),
				new SqlParameter("@PrintCount", Convert.ToInt32(0)),
			};
			var detailSql = new DbParameter[]
			{
				new SqlParameter("@id", new Guid("030f041a-388a-4f74-bf34-2cf2372069a5")),
				new SqlParameter("@code", "0000"),
				new SqlParameter("@quantity", Convert.ToDecimal(66.00000000000000)),
				new SqlParameter("@baseQuantity", Convert.ToDecimal(66.00000000000000)),
				new SqlParameter("@origPrice", Convert.ToDecimal(3680.00000000000000)),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@origDiscountPrice", Convert.ToDecimal(3145.30000000000000)),
				new SqlParameter("@discountPrice", Convert.ToDecimal(3145.30000000000000)),
				new SqlParameter("@taxRate", Convert.ToDecimal(0.17000000000000)),
				new SqlParameter("@origTaxPrice", Convert.ToDecimal(3680.00000000000000)),
				new SqlParameter("@taxPrice", Convert.ToDecimal(3680.00000000000000)),
				new SqlParameter("@origDiscountAmount", Convert.ToDecimal(207589.74000000000000)),
				new SqlParameter("@discountAmount", Convert.ToDecimal(207589.74000000000000)),
				new SqlParameter("@origTax", Convert.ToDecimal(35290.26000000000000)),
				new SqlParameter("@tax", Convert.ToDecimal(35290.26000000000000)),
				new SqlParameter("@origTaxAmount", Convert.ToDecimal(242880.00000000000000)),
				new SqlParameter("@taxAmount", Convert.ToDecimal(242880.00000000000000)),
				new SqlParameter("@deliveryDate", "2014-12-30 00:00:00"),
				new SqlParameter("@isPresent", Convert.ToByte(0)),
				new SqlParameter("@origDiscount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@discount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@taxFlag", Convert.ToByte(1)),
				new SqlParameter("@referenceCount", Convert.ToInt32(0)),
				new SqlParameter("@lastmodifiedfield", ""),
				new SqlParameter("@idinventory", new Guid("910c877a-2f85-4499-a646-a3840105c90b")),
				new SqlParameter("@inventoryBarCode", ""),
				new SqlParameter("@idunit", new Guid("1e5c5d71-ff5d-477f-9b49-a38c010ff5f9")),
				new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", "2014-12-11 16:47:37"),
				new SqlParameter("@idbaseunit", new Guid("1e5c5d71-ff5d-477f-9b49-a38c010ff5f9")),
				new SqlParameter("@idSaleOrderDTO", new Guid("b237c1f3-8912-4bd6-ade0-18b92758a9ee")),
				new SqlParameter("@HasMRP", Convert.ToByte(0)),
				new SqlParameter("@HasPRA", Convert.ToByte(0)),
				new SqlParameter("@prarequiretimes", Convert.ToInt32(0)),
				new SqlParameter("@mrprequiretimes", Convert.ToInt32(0)),
			};
			return new List<Tuple<string, IEnumerable<DbParameter>>>
			{
				new Tuple<string, IEnumerable<DbParameter>>("SA_SaleOrder",mainSql),
				new Tuple<string, IEnumerable<DbParameter>>("SA_SaleOrder_b",detailSql)
			};
		}
	}
}
