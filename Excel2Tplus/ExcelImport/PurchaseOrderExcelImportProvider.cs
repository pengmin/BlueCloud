using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 采购单Excel导入提供程序
	/// </summary>
	class PurchaseOrderExcelImportProvider : IExcelImportProvider<PurchaseOrder>
	{
		public IEnumerable<PurchaseOrder> Import(string excelPath)
		{
			var eh = new ExcelHelper(excelPath, true);
			var dt = eh.Read();
			return null;
		}
	}
}
