using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 采购进货单Excel导入提供程序
	/// </summary>
	class PurchaseArrivalExcelImportProvider : IExcelImportProvider<PurchaseArrival>
	{
		public IEnumerable<PurchaseArrival> Import(string excelPath)
		{
			throw new NotImplementedException();
		}
	}
}
