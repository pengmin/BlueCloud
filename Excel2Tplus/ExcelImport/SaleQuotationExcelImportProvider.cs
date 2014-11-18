using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 报价单Excel导入提供程序
	/// </summary>
	class SaleQuotationExcelImportProvider : IExcelImportProvider<SaleQuotation>
	{
		public IEnumerable<SaleQuotation> Import(string excelPath)
		{
			throw new NotImplementedException();
		}
	}
}
