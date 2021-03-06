﻿using System;
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
		public IDatabaseExportProvider<TEntity> GetProvider<TEntity>(Type entityType) where TEntity : Entity
		{
			//if (entityType == typeof(PurchaseRequisition))
			//{
			//	return new PurchaseRequisitionDatabaseExportProvider();
			//}
			//if (entityType == typeof(PurchaseOrder))
			//{
			//	return new PurchaseOrderDatabaseExportProvider();
			//}
			//if (entityType == typeof(PurchaseArrival))
			//{
			//	return new PurchaseArrivalDatabaseExportProvider();
			//}
			//if (entityType == typeof(SaleQuotation))
			//{
			//	return new SaleQuotationDatabaseExportProvider();
			//}
			//if (entityType == typeof(SaleOrder))
			//{
			//	return new SaleOrderDatabaseExportProvider();
			//}
			//if (entityType == typeof(SaleDelivery))
			//{
			//	return new SaleDeliveryDatabaseExportProvider();
			//}
			//if (entityType == typeof(InputWarehouse))
			//{
			//	return new InputWarehouseDatabaseExportProvider();
			//}
			//if (entityType == typeof(OutputWarehouse))
			//{
			//	return new OutputWarehouseDatabaseExportProvider();
			//}
			return null;
		}
	}
}
