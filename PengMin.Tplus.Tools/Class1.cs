using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PengMin.Tplus.Tools
{
	internal class Class1
	{
		public Class1()
		{
			var v = new DbParameter[]
			{
				new SqlParameter("@id", ""), 
				new SqlParameter("@code", ""), 
				new SqlParameter("@invoiceType", ""), 
				new SqlParameter("@purchaseInvoiceNo", ""), 
				new SqlParameter("@discountRate", "Convert.ToDecimal(1.00000000000000)"), 
				new SqlParameter("@exchangeRate", "Convert.ToDecimal(1.00000000000000)"), 
				new SqlParameter("@acceptAddress", "Convert.ToDecimal(1.00000000000000)"), 
				new SqlParameter("@linkMan", "Convert.ToDecimal(1.00000000000000)"), 
				new SqlParameter("@linkTelphone", "Convert.ToDecimal(1.00000000000000)"), 
				new SqlParameter("@payType", "Convert.ToDecimal(1.00000000000000)"), 
				new SqlParameter("@origPaymentCashAmount", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@paymentCashAmount", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@isPriceCheck", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@isReduceArrival", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@isAutoGenerateInvoice", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@isAutoGenerateInStock", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@voucherState", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@memo", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@inStockState", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@settleState", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@isNoArapBookkeeping", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@voucherdate", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@maker", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@auditor", "Convert.ToDecimal(0.00000000000000)"),
				new SqlParameter("@makerid", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@reviser", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@iscarriedforwardout", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@iscarriedforwardin", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@ismodifiedcode", "Convert.ToDecimal(0.00000000000000)"), 
				new SqlParameter("@accountingperiod", "Convert.ToInt32(0)"), 
				new SqlParameter("@accountingyear", "Convert.ToInt32(0)"), 
				new SqlParameter("@createdtime", "Convert.ToInt32(0)"), 
				new SqlParameter("@sequencenumber", "Convert.ToInt32(0)"), new SqlParameter("@ts", "Convert.ToInt32(0)"), new SqlParameter("@idbusinesstype", "Convert.ToInt32(0)"), new SqlParameter("@idcurrency", "Convert.ToInt32(0)"), new SqlParameter("@idpartner", "Convert.ToInt32(0)"), new SqlParameter("@iddepartment", "Convert.ToInt32(0)"), new SqlParameter("@idwarehouse", "Convert.ToInt32(0)"), new SqlParameter("@pubuserdefnvc1", "Convert.ToInt32(0)"), new SqlParameter("@pubuserdefnvc2", "Convert.ToInt32(0)"), new SqlParameter("@pubuserdefnvc3", "Convert.ToInt32(0)"), new SqlParameter("@pubuserdefnvc4", "Convert.ToInt32(0)"), new SqlParameter("@origPaymentAmount", "Convert.ToDecimal(0.00000000000000)"), new SqlParameter("@paymentAmount", "Convert.ToDecimal(0.00000000000000)"), new SqlParameter("@idproject", "Convert.ToDecimal(0.00000000000000)"), new SqlParameter("@IsBeforeSystemInuse", "Convert.ToDecimal(0.00000000000000)"), new SqlParameter("@idMarketingOrgan", "Convert.ToDecimal(0.00000000000000)"), new SqlParameter("@changer", "Convert.ToDecimal(0.00000000000000)"), new SqlParameter("@PrintCount", "Convert.ToInt32(0)"),
			};
		}
	}
}
