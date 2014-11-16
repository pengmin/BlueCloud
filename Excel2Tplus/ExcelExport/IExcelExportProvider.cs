using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelExport
{
	interface IExcelExportProvider
	{
		void Export<TEntity>(string excelPath, IEnumerable<TEntity> list) where TEntity : Entity;
	}
}
