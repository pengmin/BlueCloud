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
		private readonly List<Tuple<string, IEnumerable<DbParameter>>> _otherSql =
			new List<Tuple<string, IEnumerable<DbParameter>>>();

		protected override string VoucherName
		{
			get { return "销售出库单"; }
		}
		protected override string VoucherTable
		{
			get { return "ST_RDRecord"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(OutputWarehouse obj, Guid id)
		{
			SetOtherSql(id);
			decimal cy, m;//抽佣比率
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", id),
				new SqlParameter("@code", string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@dispatchAddress", ""),
				new SqlParameter("@contact", ""),
				new SqlParameter("@contactPhone", ""),
				new SqlParameter("@voucherState", TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@printTime", Convert.ToInt32(0)),
				new SqlParameter("@amount", decimal.TryParse( obj.成本金额,out m)?m:m),//成本金额
				new SqlParameter("@rdDirectionFlag", Convert.ToByte(0)),
				new SqlParameter("@deliveryState", new Guid("3fa9f8c4-4d3f-4b90-be87-a18cde11ca8e")),
				new SqlParameter("@isCostAccount", Convert.ToByte(0)),
				new SqlParameter("@isMergedFlow", Convert.ToByte(0)),
				new SqlParameter("@isAutoGenerate", Convert.ToByte(0)),
				new SqlParameter("@memo", ""),
				new SqlParameter("@isNoModify", ""),
				new SqlParameter("@voucherdate", DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@maker", "DEMO"),
				new SqlParameter("@madedate", "2013-06-01 00:00:00"),
				new SqlParameter("@auditor", ""),
				new SqlParameter("@makerid", new Guid("6bd0a3b0-5701-4c70-9cb8-a37b010e56ee")),
				new SqlParameter("@reviser", ""),
				new SqlParameter("@iscarriedforwardout", Convert.ToByte(0)),
				new SqlParameter("@iscarriedforwardin", Convert.ToByte(0)),
				new SqlParameter("@ismodifiedcode", Convert.ToByte(0)),
				new SqlParameter("@accountingperiod", Convert.ToInt32(6)),
				new SqlParameter("@accountingyear", Convert.ToInt32(2013)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@iddepartment", TplusDatabaseHelper.Instance.GetCompanyIdByName(obj.所属公司)),
				new SqlParameter("@idpartner", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@idbusitype", string.IsNullOrWhiteSpace(obj.退货日期)?new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98"):new Guid("23FBABAD-1697-41AB-9FAC-8BA86B38FF01")),
				new SqlParameter("@idvouchertype", new Guid("bb007f33-c0f0-4a19-8588-1e0e314d1f56")),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idrdstyle", new Guid("2f384c02-e650-4421-81de-e870e6a8b0b3")),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@priuserdefdecm1", decimal.TryParse(obj.抽佣比率,out cy)?cy:cy),
				new SqlParameter("@pubuserdefnvc1", obj.部门),
				new SqlParameter("@pubuserdefnvc2", obj.业务员),
				new SqlParameter("@pubuserdefnvc3", obj.退货日期),
				new SqlParameter("@pubuserdefnvc4", ""),
				new SqlParameter("@VoucherYear", Convert.ToInt32(2013)),
				new SqlParameter("@VoucherPeriod", Convert.ToInt32(6)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@idProject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@idSettleCustomer", TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.客户)),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@PrintCount", Convert.ToInt32(0)),
				new SqlParameter("@Address", ""),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(OutputWarehouse obj, Guid pid)
		{
			SetOtherSql2(obj);
			decimal m;//金额
			double tr;//税率
			int d;//整数
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",  Guid.NewGuid()),
				new SqlParameter("@code", (Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@quantity", int.TryParse(obj.数量,out d)?d:d),
				new SqlParameter("@baseQuantity", int.TryParse(obj.数量,out d)?d:d),
				new SqlParameter("@price", decimal.TryParse( obj.成本价,out m)?m:m),//成本价
				//new SqlParameter("@basePrice", Convert.ToDecimal(26.92000000000000)),
				new SqlParameter("@amount", decimal.TryParse( obj.成本金额,out m)?m:m),//成本金额
				new SqlParameter("@taxRate", (double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@tax", decimal.TryParse( obj.税额,out m)?m:m),
				new SqlParameter("@isManualCost", Convert.ToByte(0)),
				new SqlParameter("@isCostAccounted", Convert.ToByte(0)),
				new SqlParameter("@taxFlag", Convert.ToByte(1)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@idRDRecordDTO", pid),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.计量单位)),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@pubuserdefdecm4", decimal.TryParse(obj.抽佣,out m)?m:m),
				new SqlParameter("@idbusiTypeByMergedFlow", new Guid("db58a9e1-d6ad-4c7a-8d07-23fdddc51d98")),
				new SqlParameter("@idProject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@origTax", decimal.TryParse( obj.税额,out m)?m:m),
				new SqlParameter("@origSalePrice", decimal.TryParse(obj.售价,out m)?m:m),
				new SqlParameter("@origTaxSalePrice", decimal.TryParse(obj.含税售价,out m)?m:m),
				new SqlParameter("@origSaleAmount", decimal.TryParse(obj.销售金额,out m)?m:m),
				new SqlParameter("@origTaxSaleAmount", decimal.TryParse(obj.含税销售金额,out m)?m:m),
				new SqlParameter("@salePrice", decimal.TryParse(obj.售价,out m)?m:m),
				new SqlParameter("@taxSalePrice", decimal.TryParse(obj.含税售价,out m)?m:m),
				new SqlParameter("@saleAmount", decimal.TryParse(obj.销售金额,out m)?m:m),
				new SqlParameter("@taxSaleAmount", decimal.TryParse(obj.含税销售金额,out m)?m:m),
				new SqlParameter("@IsPresent", Convert.ToByte(0)),
				new SqlParameter("@LastModifiedField", ""),
				new SqlParameter("@DiscountRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@PriceStrategyTypeName", ""),
				new SqlParameter("@PriceStrategySchemeIds", ""),
				new SqlParameter("@PriceStrategySchemeNames", ""),
				new SqlParameter("@PromotionVoucherCodes", ""),
				new SqlParameter("@PromotionVoucherIds", ""),
				new SqlParameter("@IsPromotionPresent", Convert.ToByte(0)),
				new SqlParameter("@PromotionSingleVoucherCode", ""),
				new SqlParameter("@priuserdefnvc1",obj.销售订单号),
				new SqlParameter("@pubuserdefnvc1", obj.物流名称单号),
				new SqlParameter("@pubuserdefnvc2", obj.发货信息),
				new SqlParameter("@pubuserdefnvc3", obj.客户收货信息),
				new SqlParameter("@pubuserdefnvc4", obj.平台单号),
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

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> OtherSql()
		{
			return _otherSql;
		}

		private void SetOtherSql2(OutputWarehouse obj)
		{
			var sql = @"
UPDATE dbo.ST_CurrentStock SET forSaleDispatchBaseQuantity=isnull(forSaleDispatchBaseQuantity,0)+@quantity
WHERE idinventory=@inventory AND idwarehouse=@warehouse";
			_otherSql.Add(new Tuple<string, IEnumerable<DbParameter>>(sql, new DbParameter[]
			{
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@inventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@warehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)), 
			}));
		}
		private void SetOtherSql(Guid id)
		{
			var sql = @" INSERT INTO ARAP_DetailSecond
        ( Id ,
          IsArFlag ,
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
          Amount ,
          OrigSettleAmount ,
          SettleAmount ,
          ExtendFlag ,
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
                1 ,
                0 ,
                0 ,
                0 ,
                '0' ,
                -1 ,
                M.CreatedTime ,
                M.IdSettleCustomer ,
                M.Idpartner ,
                M.Iddepartment ,
                M.Idclerk ,
                M.Idcurrency ,
                M.ExchangeRate ,
                M.ID ,
                M.Code ,
                M.AccountingYear ,
                M.AccountingPeriod ,
                M.DocId ,
                M.DocClass ,
                M.DocNo ,
                'db58a9e1-d6ad-4c7a-8d07-23fdddc51d98' ,
                M.idvouchertype ,
                M.idvouchertype ,
                M.ID ,
                M.Code ,
                S.ID ,
                B.ID ,
                B.Code ,
                B.Name ,
                M.Memo ,
                M.VoucherDate ,
                M.VoucherDate ,
                M.MaturityDate ,
                2013 ,
                6 ,
                S.OrigTaxSaleAmount ,
                S.TaxSaleAmount ,
                ( CASE WHEN S.CumulativeSaleDispatchQuantity = 0 THEN 0
                       WHEN S.CumulativeSaleDispatchQuantity = S.quantity
                       THEN S.OrigTaxSaleAmount
                       ELSE S.OrigTaxSalePrice
                            * ISNULL(S.CumulativeSaleDispatchQuantity, 0)
                  END ) AS OrigSettleAmount ,
                ( CASE WHEN S.CumulativeSaleDispatchQuantity = 0 THEN 0
                       WHEN S.CumulativeSaleDispatchQuantity = S.quantity
                       THEN S.TaxSaleAmount
                       ELSE S.TaxSalePrice
                            * ISNULL(S.CumulativeSaleDispatchQuantity, 0)
                  END ) AS settleAmount ,
                ( CASE WHEN S.CumulativeSaleDispatchQuantity = 0 THEN 0
                       WHEN S.quantity - S.CumulativeSaleDispatchQuantity = 0
                       THEN 4
                       ELSE 3
                  END ) AS ExtendFlag ,
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
        FROM    ST_RDRecord M ,
                ST_RDRecord_b S ,
                AA_Inventory B
        WHERE   M.ID = S.idRDRecordDTO
                AND B.id = S.idinventory
                AND M.ID = @id";

			_otherSql.Add(new Tuple<string, IEnumerable<DbParameter>>(sql, new DbParameter[] { new SqlParameter("@id", id) }));
		}
	}
}
