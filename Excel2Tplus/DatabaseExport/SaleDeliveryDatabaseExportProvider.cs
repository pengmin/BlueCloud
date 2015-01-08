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
	/// 销货单数据库导出提供程序
	/// </summary>
	class SaleDeliveryDatabaseExportProvider : BaseDatabaseExportProvider<SaleDelivery>
	{
		private readonly List<Tuple<string, IEnumerable<DbParameter>>> _otherSql =
			new List<Tuple<string, IEnumerable<DbParameter>>>();

		protected override string VoucherName
		{
			get { return "销货单"; }
		}
		protected override string VoucherTable
		{
			get { return "SA_SaleDelivery"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(SaleDelivery obj, Guid id)
		{
			SetOtherSql(id);
			decimal d;
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", id),
				new SqlParameter("@code", string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@origAmount", obj.金额),
				new SqlParameter("@amount", obj.金额),
				new SqlParameter("@origTaxAmount", obj.含税金额),
				new SqlParameter("@taxAmount", obj.含税金额),
				new SqlParameter("@voucherState", TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@isCounteractDelivery", Convert.ToByte(0)),
				new SqlParameter("@invoiceType", new Guid("4806d0be-c026-4a96-a271-97514e6a9082")),
				new SqlParameter("@discountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@address", ""),
				new SqlParameter("@linkMan", ""),
				new SqlParameter("@reciveType", new Guid("ef370928-98f5-4312-a1c6-021587d17af1")),
				new SqlParameter("@isCancel", new Guid("03f167c8-4494-44ea-b6eb-ecb2539a85a0")),
				new SqlParameter("@isPriceTrace", Convert.ToByte(1)),
				new SqlParameter("@memo", ""),
				new SqlParameter("@saleInvoiceNo", ""),
				new SqlParameter("@isAutoGenerateInvoice", Convert.ToByte(0)),
				new SqlParameter("@isAutoGenerateSaleOut", Convert.ToByte(0)),
				new SqlParameter("@origSettleAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@settleAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@contactPhone", ""),
				new SqlParameter("@isSaleOut", new Guid("34b60e63-7949-4209-b6d8-a2cadf00cfd7")),
				new SqlParameter("@isBeforeSystemInuse", Convert.ToByte(0)),
				new SqlParameter("@voucherdate", DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@maker", "DEMO"),
				new SqlParameter("@madedate", "2013-06-01 00:00:00"),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@idcustomer", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@makerid", new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@iddepartment", TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@accountingperiod", Convert.ToInt32(0)),
				new SqlParameter("@idrdstyle", new Guid("2f384c02-e650-4421-81de-e870e6a8b0b3")),
				new SqlParameter("@accountingyear", Convert.ToInt32(0)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@idbusinesstype", string.IsNullOrWhiteSpace(obj.退货日期)?new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98"):new Guid("23FBABAD-1697-41AB-9FAC-8BA86B38FF01")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@priuserdefdecm1", decimal.TryParse(obj.抽佣比率,out d)?d:d),
				new SqlParameter("@pubuserdefnvc1", obj.部门),
				new SqlParameter("@pubuserdefnvc2", obj.业务员),
				new SqlParameter("@pubuserdefnvc3", obj.退货日期),
				new SqlParameter("@pubuserdefnvc4", ""),
				new SqlParameter("@isNoArapBookkeeping", Convert.ToByte(0)),
				new SqlParameter("@origprereceiveamount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@PreReceiveAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@idsettleCustomer", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@changer", ""),
				new SqlParameter("@DataSource", new Guid("3d362676-d6d5-4439-9be5-40c67514b9f5")),
				new SqlParameter("@OrigInvoiceTaxAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@thisBalanceIntegral", Convert.ToInt32(0)),
				new SqlParameter("@MemberAddress", ""),
				new SqlParameter("@PrintCount", Convert.ToInt32(0)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(SaleDelivery obj, Guid pid)
		{
			double tr;//税率
			decimal m;
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
				new SqlParameter("@discountAmount", obj.金额),
				new SqlParameter("@origTax", obj.税额),
				new SqlParameter("@tax", obj.税额),
				new SqlParameter("@origTaxAmount", obj.含税金额),
				new SqlParameter("@taxAmount", obj.含税金额),
				new SqlParameter("@costAmount", Convert.ToDecimal(0.00000000000000)),
				new SqlParameter("@isPresent", Convert.ToByte(0)),
				new SqlParameter("@idSaleDeliveryDTO", pid),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@taxFlag", Convert.ToByte(1)),
				new SqlParameter("@isManualCost", Convert.ToByte(0)),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@lastmodifiedfield", ""),
				new SqlParameter("@inventoryBarCode", ""),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@priuserdefnvc1", obj.销售订单号),
				new SqlParameter("@pubuserdefnvc1", obj.物流名称单号),
				new SqlParameter("@pubuserdefdecm1",decimal.TryParse(obj.满减活动,out m)?m:0m),
				new SqlParameter("@pubuserdefnvc2", obj.发货信息),
				new SqlParameter("@pubuserdefdecm2", decimal.TryParse(obj.抵用券,out m)?m:0m),
				new SqlParameter("@pubuserdefnvc3", obj.客户收货信息),
				new SqlParameter("@pubuserdefdecm3",decimal.TryParse(obj.代收运费,out m)?m:0m),
				new SqlParameter("@pubuserdefnvc4", obj.平台单号),
				new SqlParameter("@pubuserdefdecm4", decimal.TryParse(obj.抽佣,out m)?m:0m),
				new SqlParameter("@idproject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
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

		protected override bool CanExport(SaleDelivery obj, out IEnumerable<string> msgs)
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
			if (TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户) is DBNull)
			{
				list.Add("单据[" + obj.单据编号 + "]客户不存在");
			}
			if (!TplusDatabaseHelper.Instance.GetParnerTypeByName(obj.客户).Contains("客户"))
			{
				list.Add("单据[" + obj.单据编号 + "](" + obj.客户 + ")不是客户性质的往来单位");
			}
			msgs = list;
			return !msgs.Any();
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
          IdnoSettlePartner ,
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
          IdMember ,
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
          IdsourceOrderType ,
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
        SELECT  dbo.EAP_FN_NewObjectID(NEWID()) ,
                0 ,
                0 ,
                0 ,
                '0' ,
                -1 ,
                M.CreatedTime ,
                M.IdsettleCustomer ,
                M.Idcustomer ,
                M.Iddepartment ,
                M.Idclerk ,
                M.Idcurrency ,
                M.ExchangeRate ,
                1 ,
                M.ID ,
                M.Code ,
                M.AccountingYear ,
                M.AccountingPeriod ,
                M.DocId ,
                M.DocClass ,
                M.DocNo ,
                'db58a9e1-d6ad-4c7a-8d07-23fdddc51d98' ,
                '5794d3dd-7fe4-4b5c-af53-d21ddc13bf16' ,
                '5794d3dd-7fe4-4b5c-af53-d21ddc13bf16' ,
                M.ID ,
                M.Code ,
                S.ID ,
                B.ID ,
                B.Code ,
                B.Name ,
                M.Memo ,
                M.IdMember ,
                M.VoucherDate ,
                M.VoucherDate ,
                M.RecivingMaturity ,
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
        FROM    SA_SaleDelivery M ,
                SA_SaleDelivery_b S ,
                AA_Inventory B
        WHERE   M.ID = S.idSaleDeliveryDTO
                AND B.id = S.idinventory
                AND M.ID = @id";

			_otherSql.Add(new Tuple<string, IEnumerable<DbParameter>>(sql, new DbParameter[] { new SqlParameter("@id", id) }));
		}
	}
}
