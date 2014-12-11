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
	/// 采购进货单数据库导出提供程序
	/// </summary>
	internal class PurchaseArrivalDatabaseExportProvider : BaseDatabaseExportProvider<PurchaseArrival>
	{
		private readonly List<Tuple<string, IEnumerable<DbParameter>>> _otherSql =
			new List<Tuple<string, IEnumerable<DbParameter>>>();

		protected override string VoucherName
		{
			get { return "进货单"; }
		}

		protected override string VoucherTable
		{
			get { return "PU_PurchaseArrival"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(PurchaseArrival obj, Guid id)
		{
			SetOtherSql(id);
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", id),
				new SqlParameter("@code",  string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@invoiceType", new Guid("82bf126f-10e0-4272-8098-4c4c9c1ae3bc")),
				new SqlParameter("@purchaseInvoiceNo", ""),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@acceptAddress", ""),
				new SqlParameter("@linkMan", ""),
				new SqlParameter("@linkTelphone", ""),
				new SqlParameter("@payType", new Guid("8f69ab53-409f-4cac-acbc-fd5b65b684d4")),
				new SqlParameter("@origPaymentCashAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@paymentCashAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@origTotalTaxAmount", obj.含税金额),
				new SqlParameter("@totalTaxAmount", obj.含税金额),
				new SqlParameter("@origtotalAmount", obj.金额),
				new SqlParameter("@totalAmount", obj.金额),
				new SqlParameter("@isPriceCheck", Convert.ToByte(1)),
				new SqlParameter("@isReduceArrival", Convert.ToByte(0)),
				new SqlParameter("@isAutoGenerateInvoice", Convert.ToByte(0)),
				new SqlParameter("@isAutoGenerateInStock", Convert.ToByte(0)),
				new SqlParameter("@voucherState",  TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@memo", ""),
				new SqlParameter("@inStockState", new Guid("2f7d8d02-10c4-453e-aff9-691b052d9e52")),
				new SqlParameter("@settleState", new Guid("03f167c8-4494-44ea-b6eb-ecb2539a85a0")),
				new SqlParameter("@isNoArapBookkeeping", Convert.ToByte(0)),
				new SqlParameter("@voucherdate", DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@maker", "DEMO"),
				new SqlParameter("@madedate", "2013-06-01 00:00:00"),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@makerid", new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@accountingperiod", Convert.ToInt32(0)),
				new SqlParameter("@accountingyear", Convert.ToInt32(0)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@idbusinesstype", new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.供应商)),
				new SqlParameter("@iddepartment", TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@pubuserdefnvc1",obj.部门),
				new SqlParameter("@pubuserdefnvc2", obj.业务员),
				new SqlParameter("@pubuserdefnvc3",  obj.退货日期),
				new SqlParameter("@pubuserdefnvc4", ""),
				new SqlParameter("@origPaymentAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@paymentAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@IsBeforeSystemInuse", Convert.ToByte(0)),
				new SqlParameter("@idMarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@changer", ""),
				new SqlParameter("@PrintCount", Convert.ToInt32(0)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(PurchaseArrival obj, Guid pid)
		{
			double tr;//税率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", Guid.NewGuid()),
				new SqlParameter("@code",  (Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@taxFlag", Convert.ToByte(0)),
				new SqlParameter("@compositionQuantity", ""),
				new SqlParameter("@quantity",  obj.数量),
				new SqlParameter("@origDiscountPrice",obj.单价),
				new SqlParameter("@baseQuantity",  obj.数量),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@taxRate", (double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@origTaxPrice", obj.含税单价),
				new SqlParameter("@origDiscountAmount",obj.金额),
				new SqlParameter("@origTax", obj.税额),
				new SqlParameter("@origTaxAmount", obj.含税金额),
				new SqlParameter("@discountPrice", obj.单价),
				new SqlParameter("@taxPrice", obj.含税单价),
				new SqlParameter("@discountAmount", obj.金额),
				new SqlParameter("@tax", obj.税额),
				new SqlParameter("@taxAmount", obj.含税金额),
				new SqlParameter("@isPresent", Convert.ToByte(0)),
				new SqlParameter("@saleOrderCode", ""),
				new SqlParameter("@baseDiscountPrice", obj.单价),
				new SqlParameter("@baseTaxPrice", obj.含税单价),
				new SqlParameter("@lastmodifiedfield", ""),
				new SqlParameter("@inventoryBarCode", ""),
				new SqlParameter("@partnerInventoryCode", ""),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@idPurchaseArrivalDTO", pid),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@partnerInventoryName", ""),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@islaborcost", Convert.ToByte(0)),
			};

			//return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps) };
			return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps), BuildCurrentStockSql(obj) };
		}

		protected override bool CanExport(PurchaseArrival obj, out IEnumerable<string> msgs)
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

		protected Tuple<string, IEnumerable<DbParameter>> BuildCurrentStockSql(PurchaseArrival obj)
		{
			int d;
			return new Tuple<string, IEnumerable<DbParameter>>(
				"ST_CurrentStock",
				new DbParameter[]
				{
					new SqlParameter("@id", Guid.NewGuid()),
					new SqlParameter("@purchaseForReceiveBaseQuantity", int.TryParse(obj.数量,out d)?d:d),
					new SqlParameter("@recordDate", DateTime.Now.ToString("yyyy-MM-dd")),
					new SqlParameter("@isCarriedForwardOut", Convert.ToByte(0)),
					new SqlParameter("@isCarriedForwardIn", Convert.ToByte(0)),
					new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
					//new SqlParameter("@ts", "System.Byte[]"),
					new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
					new SqlParameter("@updatedBy", "demo"),
					new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
					new SqlParameter("@idbaseunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
					new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
					new SqlParameter("@IdMarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				});
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> OtherSql()
		{
			return _otherSql;
		}

		private void SetOtherSql(Guid id)
		{
			var sql = @" INSERT INTO ARAP_Detail
        ( Id ,
          AuditBussinessFlag ,
          AuditFlag ,
          PrepayFlag ,
          BussinessFlag ,
          Flag ,
          CreatedTime ,
          Idpartner ,
          Iddepartment ,
          Idperson ,
          Idcurrency ,
          ExchangeRate ,
          IsArFlag ,
          BusinessID ,
          BusinessCode ,
          AccountingYear ,
          AccountingPeriod ,
          DocId ,
          DocClass ,
          DocNo ,
          IdbusiType ,
          IdvoucherType ,
          IdarapVoucherType ,
          VoucherID ,
          VoucherCode ,
          VoucherDetailID ,
          DetailID ,
          DetailCode ,
          DetailName ,
          Memo ,
          VoucherDate ,
          RegisterDate ,
          ArrivalDate ,
          Year ,
          Period ,
          OrigAmount ,
          OrigSettleAmount ,
          Amount ,
          SettleAmount ,
          origCashAmount ,
          cashAmount ,
          origInAllowances ,
          inAllowances ,
          SaleOrderCode ,
          SaleOrderID ,
          SourceOrderCode ,
          SourceOrderID ,
          idsourceordertype ,
          DetailType ,
          VoucherTimeStamp ,
          Idproject ,
          IdDetailProject ,
          isCarriedForwardOut ,
          isCarriedForwardIn ,
          updated ,
          IdMarketingOrgan ,
          HeadPriuserdefdecm1 ,
          HeadPriuserdefdecm2 ,
          HeadPriuserdefdecm3 ,
          HeadPriuserdefdecm4 ,
          HeadPriuserdefdecm5 ,
          HeadPriuserdefdecm6 ,
          HeadPubuserdefdecm1 ,
          HeadPubuserdefdecm2 ,
          HeadPubuserdefdecm3 ,
          HeadPubuserdefdecm4 ,
          HeadPubuserdefdecm5 ,
          HeadPubuserdefdecm6 ,
          HeadPriuserdefnvc1 ,
          HeadPriuserdefnvc2 ,
          HeadPriuserdefnvc3 ,
          HeadPriuserdefnvc4 ,
          HeadPriuserdefnvc5 ,
          HeadPriuserdefnvc6 ,
          headPubuserdefnvc1 ,
          headPubuserdefnvc2 ,
          headPubuserdefnvc3 ,
          headPubuserdefnvc4 ,
          headPubuserdefnvc5 ,
          headPubuserdefnvc6 ,
          DetailPriuserdefdecm1 ,
          DetailPriuserdefdecm2 ,
          DetailPriuserdefdecm3 ,
          DetailPriuserdefdecm4 ,
          DetailPubuserdefdecm1 ,
          DetailPubuserdefdecm2 ,
          DetailPubuserdefdecm3 ,
          DetailPubuserdefdecm4 ,
          DetailPriuserdefnvc1 ,
          DetailPriuserdefnvc2 ,
          DetailPriuserdefnvc3 ,
          DetailPriuserdefnvc4 ,
          DetailPubuserdefnvc1 ,
          DetailPubuserdefnvc2 ,
          DetailPubuserdefnvc3 ,
          DetailPubuserdefnvc4 
        )
        SELECT  NEWID() ,
                0 ,
                0 ,
                0 ,
                '0' ,
                -1 ,
                M.CreatedTime ,
                M.Idpartner ,
                M.Iddepartment ,
                M.Idclerk ,
                M.Idcurrency ,
                M.ExchangeRate ,
                0 ,
                M.ID ,
                M.Code ,
                M.AccountingYear ,
                M.AccountingPeriod ,
                M.DocId ,
                M.DocClass ,
                M.DocNo ,
                'd6b6deeb-88fb-4e28-bacc-a8f2bb3b449c' ,
                '5399d7a6-8b61-459a-ac53-5c208d1bc871' ,
                '5399d7a6-8b61-459a-ac53-5c208d1bc871' ,
                M.ID ,
                M.Code ,
                S.ID ,
                B.ID ,
                B.Code ,
                B.Name ,
                M.Memo ,
                M.VoucherDate ,
                M.VoucherDate ,
                M.PayDate ,
                2013 ,
                6 ,
                S.OrigTaxAmount ,
                0 ,
                S.TaxAmount ,
                0 ,
                0 ,
                0 ,
                0 ,
                0 ,
                S.SaleOrderCode ,
                S.SaleOrderID ,
                S.SourceVoucherCode ,
                S.SourceVoucherId ,
                S.idsourcevouchertype ,
                'inventory' ,
                CAST(M.Ts AS BIGINT) ,
                M.Idproject ,
                S.Idproject ,
                0 ,
                0 ,
                CONVERT(CHAR(19), GETDATE(), 120) ,
                M.IdMarketingOrgan ,
                M.priuserdefdecm1 ,
                M.priuserdefdecm2 ,
                M.priuserdefdecm3 ,
                M.priuserdefdecm4 ,
                M.priuserdefdecm5 ,
                M.priuserdefdecm6 ,
                M.pubuserdefdecm1 ,
                M.pubuserdefdecm2 ,
                M.pubuserdefdecm3 ,
                M.pubuserdefdecm4 ,
                M.pubuserdefdecm5 ,
                M.pubuserdefdecm6 ,
                M.priuserdefnvc1 ,
                M.priuserdefnvc2 ,
                M.priuserdefnvc3 ,
                M.priuserdefnvc4 ,
                M.priuserdefnvc5 ,
                M.priuserdefnvc6 ,
                M.pubuserdefnvc1 ,
                M.pubuserdefnvc2 ,
                M.pubuserdefnvc3 ,
                M.pubuserdefnvc4 ,
                M.pubuserdefnvc5 ,
                M.pubuserdefnvc6 ,
                S.priuserdefdecm1 ,
                S.priuserdefdecm2 ,
                S.priuserdefdecm3 ,
                S.priuserdefdecm4 ,
                S.pubuserdefdecm1 ,
                S.pubuserdefdecm2 ,
                S.pubuserdefdecm3 ,
                S.pubuserdefdecm4 ,
                S.priuserdefnvc1 ,
                S.priuserdefnvc2 ,
                S.priuserdefnvc3 ,
                S.priuserdefnvc4 ,
                S.pubuserdefnvc1 ,
                S.pubuserdefnvc2 ,
                S.pubuserdefnvc3 ,
                S.pubuserdefnvc4
        FROM    PU_PurchaseArrival M ,
                PU_PurchaseArrival_b S ,
                AA_Inventory B
        WHERE   M.ID = S.idPurchaseArrivalDTO
                AND B.id = S.idinventory
                AND M.ID = @id";

			_otherSql.Add(new Tuple<string, IEnumerable<DbParameter>>(sql, new DbParameter[] { new SqlParameter("@id", id) }));
		}
	}
}
