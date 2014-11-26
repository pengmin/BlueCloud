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
			return new[] { "-1" };
		}
	}
}
