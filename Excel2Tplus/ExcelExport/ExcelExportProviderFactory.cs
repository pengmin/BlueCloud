using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.ExcelExport
{
	class ExcelExportProviderFactory
	{
		public IExcelExportProvider GetProvider(Type entityType)
		{
			return null;
		}
	}
}
