using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;

namespace Excel2Tplus.DatabaseExport
{
	/// <summary>
	/// 数据库导出提供程序工厂
	/// </summary>
	class DatabaseExportProviderFactory
	{
		/// <summary>
		/// 获取数据库导出提供程序
		/// </summary>
		/// <param name="entityType">单据类型</param>
		/// <returns></returns>
		public IDatabaseExportProvider GetProvider(Type entityType)
		{
			if (entityType == typeof(PurchaseRequisition))
			{
				return new PurchaseRequisitionDatabaseExportProvider();
			}
			if (entityType == typeof(PurchaseOrder))
			{
				return new PurchaseOrderDatabaseExportProvider();
			}
			if (entityType == typeof(PurchaseArrival))
			{
				return new PurchaseArrivalDatabaseExportProvider();
			}
			return null;
		}
	}
}
