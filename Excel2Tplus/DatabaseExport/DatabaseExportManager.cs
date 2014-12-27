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
		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list, out bool success, out string voucherCodes) where TEntity : Entity
		{
			var elType = CommonFunction.GetElementType(list.GetType());
			if (elType == typeof(PurchaseRequisition))
			{
				return new PurchaseRequisitionDatabaseExportProvider().Export(list as IEnumerable<PurchaseRequisition>, out success, out voucherCodes);
			}
			if (elType == typeof(PurchaseOrder))
			{
				return new PurchaseOrderDatabaseExportProvider().Export(list as IEnumerable<PurchaseOrder>, out success, out voucherCodes);
			}
			if (elType == typeof(PurchaseArrival))
			{
				return new PurchaseArrivalDatabaseExportProvider().Export(list as IEnumerable<PurchaseArrival>, out success, out voucherCodes);
			}
			if (elType == typeof(InputWarehouse))
			{
				return new InputWarehouseDatabaseExportProvider().Export(list as IEnumerable<InputWarehouse>, out success, out voucherCodes);
			}
			if (elType == typeof(SaleQuotation))
			{
				return new SaleQuotationDatabaseExportProvider().Export(list as IEnumerable<SaleQuotation>, out success, out voucherCodes);
			}
			if (elType == typeof(SaleOrder))
			{
				return new SaleOrderDatabaseExportProvider().Export(list as IEnumerable<SaleOrder>, out success, out voucherCodes);
			}
			if (elType == typeof(OutputWarehouse))
			{
				return new OutputWarehouseDatabaseExportProvider().Export(list as IEnumerable<OutputWarehouse>, out success, out voucherCodes);
			}
			if (elType == typeof(SaleDelivery))
			{
				return new SaleDeliveryDatabaseExportProvider().Export(list as IEnumerable<SaleDelivery>, out success, out voucherCodes);
			}
			success = false;
			voucherCodes = null;
			return new[] { "-1" };
		}
	}
}
