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
	/// 采购入库单数据库导出提供程序
	/// </summary>
	class InputWarehouseDatabaseExportProvider : BaseDatabaseExportProvider<InputWarehouse>
	{
		private readonly List<Tuple<string, IEnumerable<DbParameter>>> _otherSql =
			new List<Tuple<string, IEnumerable<DbParameter>>>();
		protected override string VoucherName
		{
			get { return "采购入库单"; }
		}
		protected override string VoucherTable
		{
			get { return "ST_RDRecord"; }
		}

		protected override Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(InputWarehouse obj, Guid id)
		{
			SetOtherSql(id);
			decimal m;
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", id),
				new SqlParameter("@code", string.IsNullOrWhiteSpace(obj.单据编号)?Prefix+(++Serialno).ToString().PadLeft(Length,'0'):obj.单据编号),
				new SqlParameter("@voucherState", TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审")),
				new SqlParameter("@printTime", Convert.ToInt32(0)),
				new SqlParameter("@amount", Convert.ToDecimal(obj.金额)),
				new SqlParameter("@rdDirectionFlag", Convert.ToByte(1)),
				new SqlParameter("@accountState", new Guid("45964261-f94e-40ca-8643-b483034feaab")),
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
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByName(obj.供应商)),
				new SqlParameter("@idbusitype", string.IsNullOrEmpty(obj.退货日期)?new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c"):new Guid("0F086323-DC1F-40C4-8084-799E062AB8B2")),
				new SqlParameter("@idvouchertype", new Guid("9a2c7c5a-a428-4669-aa40-0aa07758241b")),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@idrdstyle", new Guid("698c1667-b8b8-41bd-8df2-8deac2677046")),
				new SqlParameter("@idcurrency", new Guid("f407692f-d14e-4a7f-84a4-87df16406b5b")),
				new SqlParameter("@pubuserdefnvc1", obj.部门),
				new SqlParameter("@pubuserdefnvc2", obj.业务员),
				new SqlParameter("@pubuserdefnvc3", obj.退货日期),
				new SqlParameter("@pubuserdefnvc4", ""),
				new SqlParameter("@VoucherYear", Convert.ToInt32(2013)),
				new SqlParameter("@VoucherPeriod", Convert.ToInt32(6)),
				new SqlParameter("@exchangeRate", Convert.ToDecimal(1.00000000000000)),
				new SqlParameter("@idProject", TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@BeforeUpgrade", ""),
				new SqlParameter("@idmarketingOrgan", new Guid("4ad74463-e871-4dc1-beb0-6e6eaa0a6386")),
				new SqlParameter("@TotalOrigTaxAmount", decimal.TryParse(obj.金额,out m)?m:m),
				new SqlParameter("@TotalTaxAmount", decimal.TryParse(obj.含税金额,out m)?m:m),
				new SqlParameter("@PrintCount", Convert.ToInt32(0)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(VoucherTable, ps);
		}

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> BuildDetailInsertSql(InputWarehouse obj, Guid pid)
		{
			double tr;//税率
			decimal yf;//运费
			decimal m;
			int d;
			var ps = new DbParameter[]
			{
				new SqlParameter("@id", Guid.NewGuid()),
				new SqlParameter("@code",(Code++).ToString().PadLeft(4,'0')),
				new SqlParameter("@quantity", int.TryParse(obj.数量,out d)?d:d),
				new SqlParameter("@baseQuantity", int.TryParse(obj.数量,out d)?d:d),
				new SqlParameter("@price", decimal.TryParse(obj.单价,out m)?m:m),
				new SqlParameter("@basePrice",decimal.TryParse(obj.单价,out m)?m:m),
				new SqlParameter("@baseEstimatedPrice", decimal.TryParse(obj.单价,out m)?m:m),
				new SqlParameter("@amount", decimal.TryParse(obj.金额,out m)?m:m),
				new SqlParameter("@estimatedAmount", decimal.TryParse(obj.金额,out m)?m:m),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@taxPrice", decimal.TryParse(obj.含税单价,out m)?m:m),
				new SqlParameter("@tax", decimal.TryParse(obj.税额,out m)?m:m),
				new SqlParameter("@taxAmount", decimal.TryParse(obj.含税金额,out m)?m:m),
				new SqlParameter("@totalAmount",decimal.TryParse(obj.金额,out m)?m:m),
				new SqlParameter("@isManualCost", Convert.ToByte(0)),
				new SqlParameter("@isCostAccounted", Convert.ToByte(0)),
				new SqlParameter("@taxFlag", Convert.ToByte(0)),
				new SqlParameter("@createdtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@sequencenumber", Convert.ToInt32(0)),
				//new SqlParameter("@ts", "System.Byte[]"),
				new SqlParameter("@updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqlParameter("@updatedBy", "demo"),
				new SqlParameter("@idRDRecordDTO", pid),
				new SqlParameter("@idinventory", TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@idunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@idbaseunit", TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@idwarehouse", TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@priuserdefdecm1", decimal.TryParse(obj.每双运费,out m)?m:m),
				//new SqlParameter("@pubuserdefnvc1", "001"),
				//new SqlParameter("@pubuserdefnvc2", "002"),
				//new SqlParameter("@pubuserdefnvc3", "003"),
				//new SqlParameter("@pubuserdefnvc4", "004"),
				new SqlParameter("@idbusiTypeByMergedFlow", new Guid("d6b6deeb-88fb-4e28-bacc-a8f2bb3b449c")),
				new SqlParameter("@idProject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@origPrice", decimal.TryParse(obj.单价,out m)?m:m),
				new SqlParameter("@origAmount",decimal.TryParse(obj.金额,out m)?m:m),
				new SqlParameter("@origTaxPrice", decimal.TryParse(obj.含税单价,out m)?m:m),
				new SqlParameter("@origTax", decimal.TryParse(obj.税额,out m)?m:m),
				new SqlParameter("@origTaxAmount", decimal.TryParse(obj.含税金额,out m)?m:m),
				new SqlParameter("@IsPresent", Convert.ToByte(0)),
				new SqlParameter("@LastModifiedField", ""),
				new SqlParameter("@IsPromotionPresent", Convert.ToByte(0)),
			};
			//return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps) };
			return new[] { new Tuple<string, IEnumerable<DbParameter>>(VoucherTable + "_b", ps), BuildCurrentStockSql(obj) };
		}

		protected Tuple<string, IEnumerable<DbParameter>> BuildCurrentStockSql(InputWarehouse obj)
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

		protected override bool CanExport(InputWarehouse obj, out IEnumerable<string> msgs)
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

		protected override IEnumerable<Tuple<string, IEnumerable<DbParameter>>> OtherSql()
		{
			return _otherSql;
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
                0 ,
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
                M.ID ,
                M.Code ,
                M.AccountingYear ,
                M.AccountingPeriod ,
                M.DocId ,
                M.DocClass ,
                M.DocNo ,
                'd6b6deeb-88fb-4e28-bacc-a8f2bb3b449c' ,
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
                S.OrigTaxAmount ,
                S.TaxAmount ,
                ( CASE WHEN s.quantity = baseQuantity
                       THEN ( CASE WHEN S.CumulativePurchaseArrivalQuantity = 0
                                   THEN 0
                                   WHEN S.CumulativePurchaseArrivalQuantity = S.quantity
                                   THEN OrigTaxAmount
                                   ELSE S.origTaxPrice
                                        * ISNULL(S.CumulativePurchaseArrivalQuantity,
                                                 0)
                              END )
                       ELSE ( CASE WHEN S.CumulativePurchaseArrivalQuantity2 = 0
                                   THEN 0
                                   WHEN S.CumulativePurchaseArrivalQuantity2 = S.quantity2
                                   THEN OrigTaxAmount
                                   ELSE S.origTaxPrice2
                                        * ISNULL(S.CumulativePurchaseArrivalQuantity2,
                                                 0)
                              END )
                  END ) AS OrigSettleAmount ,
                ( CASE WHEN s.quantity = baseQuantity
                       THEN ( CASE WHEN S.CumulativePurchaseArrivalQuantity = 0
                                   THEN 0
                                   WHEN S.CumulativePurchaseArrivalQuantity = S.quantity
                                   THEN TaxAmount
                                   ELSE S.TaxPrice
                                        * ISNULL(S.CumulativePurchaseArrivalQuantity,
                                                 0)
                              END )
                       ELSE ( CASE WHEN S.CumulativePurchaseArrivalQuantity2 = 0
                                   THEN 0
                                   WHEN S.CumulativePurchaseArrivalQuantity2 = S.quantity2
                                   THEN TaxAmount
                                   ELSE S.TaxPrice2
                                        * ISNULL(S.CumulativePurchaseArrivalQuantity2,
                                                 0)
                              END )
                  END ) AS settleAmount ,
                ( CASE WHEN s.quantity = baseQuantity
                       THEN ( CASE WHEN S.CumulativePurchaseArrivalQuantity = 0
                                   THEN 0
                                   WHEN S.quantity
                                        - S.CumulativePurchaseArrivalQuantity = 0
                                   THEN 4
                                   ELSE 3
                              END )
                       ELSE ( CASE WHEN S.CumulativePurchaseArrivalQuantity2 = 0
                                   THEN 0
                                   WHEN S.quantity2
                                        - S.CumulativePurchaseArrivalQuantity2 = 0
                                   THEN 4
                                   ELSE 3
                              END )
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
