using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// Excel导入提供程序工厂
	/// </summary>
	class ExcelImportProviderFactory
	{
		/// <summary>
		/// 获取Excel导入提供程序
		/// </summary>
		/// <typeparam name="TEntity">单据类型</typeparam>
		/// <returns>提供程序</returns>
		public IExcelImportProvider<TEntity> GetProvider<TEntity>() where TEntity : Entity
		{
			var type = typeof(TEntity);
			if (type == typeof(PurchaseRequisition))
			{
				return (IExcelImportProvider<TEntity>)new PurchaseRequisitionExcelImportProvider();
			}
			if (type == typeof(PurchaseOrder))
			{
				return (IExcelImportProvider<TEntity>)new PurchaseOrderExcelImportProvider();
			}
			if (type == typeof(PurchaseArrival))
			{
				return (IExcelImportProvider<TEntity>)new PurchaseArrivalExcelImportProvider();
			}
			if (type == typeof(InputWarehouse))
			{
				return (IExcelImportProvider<TEntity>)new InputWarehouseExcelImportProvider();
			}
			if (type == typeof(SaleQuotation))
			{
				return (IExcelImportProvider<TEntity>)new SaleQuotationExcelImportProvider();
			}
			if (type == typeof(SaleOrder))
			{
				return (IExcelImportProvider<TEntity>)new SaleOrderExcelImportProvider();
			}
			if (type == typeof(OutputWarehouse))
			{
				return (IExcelImportProvider<TEntity>)new OutputWarehouseExcelImportProvider();
			}
			if (type == typeof(SaleDelivery))
			{
				return (IExcelImportProvider<TEntity>)new SaleDeliveryExcelImportProvider();
			}
			return null;
		}
	}
}
