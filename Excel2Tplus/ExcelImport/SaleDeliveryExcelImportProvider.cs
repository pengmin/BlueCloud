using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 销货单Excel导入提供程序
	/// </summary>
	class SaleDeliveryExcelImportProvider : IExcelImportProvider<SaleDelivery>
	{
		public IEnumerable<SaleDelivery> Import(string excelPath)
		{
			throw new NotImplementedException();
		}
	}
}
