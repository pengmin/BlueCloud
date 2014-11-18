using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 采购入库单Excel导入提供程序
	/// </summary>
	class InputWarehouseExcelImportProvider : IExcelImportProvider<InputWarehouse>
	{
		public IEnumerable<InputWarehouse> Import(string excelPath)
		{
			throw new NotImplementedException();
		}
	}
}
