using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;

namespace Excel2Tplus.DatabaseExport
{
	/// <summary>
	/// 数据库导出管理器
	/// </summary>
	class DatabaseExportManager
	{
		/// <summary>
		/// 导出
		/// </summary>
		/// <typeparam name="TEntity">单据类型</typeparam>
		/// <param name="list">单据对象集合</param>
		/// <returns>导出结果</returns>
		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entity
		{
			var elType = CommonHelper.GetElementType(list.GetType());
			if (elType == typeof(PurchaseRequisition))
			{
				return new PurchaseRequisitionDatabaseExportProvider().Export(list as IEnumerable<PurchaseRequisition>);
			}
			if (elType == typeof(PurchaseOrder))
			{
				return new PurchaseOrderDatabaseExportProvider().Export(list as IEnumerable<PurchaseOrder>);
			}
			if (elType == typeof(PurchaseArrival))
			{
				return new PurchaseArrivalDatabaseExportProvider().Export(list as IEnumerable<PurchaseArrival>);
			}
			if (elType == typeof(InputWarehouse))
			{
				return new InputWarehouseDatabaseExportProvider().Export(list as IEnumerable<InputWarehouse>);
			}
			if (elType == typeof(SaleQuotation))
			{
				return new SaleQuotationDatabaseExportProvider().Export(list as IEnumerable<SaleQuotation>);
			}
			if (elType == typeof(SaleOrder))
			{
				return new SaleOrderDatabaseExportProvider().Export(list as IEnumerable<SaleOrder>);
			}
			if (elType == typeof(OutputWarehouse))
			{
				return new OutputWarehouseDatabaseExportProvider().Export(list as IEnumerable<OutputWarehouse>);
			}
			return new[] { "-1" };
		}
	}
}
