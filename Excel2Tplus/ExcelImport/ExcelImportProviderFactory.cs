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
			return null;
		}
	}
}
