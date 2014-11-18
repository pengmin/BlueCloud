using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 销售订单Excel导入提供程序
	/// </summary>
	class SaleOrderExcelImportProvider : IExcelImportProvider<SaleOrder>
	{
		public IEnumerable<SaleOrder> Import(string excelPath)
		{
			throw new NotImplementedException();
		}
	}
}
