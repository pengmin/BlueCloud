using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 销售出库单Excel导入提供程序
	/// </summary>
	class OutputWarehouseExcelImportProvider : IExcelImportProvider<OutputWarehouse>
	{
		public IEnumerable<OutputWarehouse> Import(string excelPath)
		{
			throw new NotImplementedException();
		}
	}
}
