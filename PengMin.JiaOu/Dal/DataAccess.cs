using System;
using System.Collections.Generic;
using System.Data;
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
		public DataTable GetPurchaseOrder()
		{
			var sql = @"SELECT  a.VoucherDate AS 单据日期 ,
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
	}
}
