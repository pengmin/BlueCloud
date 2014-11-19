using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// Excel导入管理器
	/// </summary>
	class ExcelImportManager
	{
		/// <summary>
		/// 导入Excel
		/// </summary>
		/// <typeparam name="TEntity">单据类型</typeparam>
		/// <param name="excelPath">Excel文件路径</param>
		/// <returns>单据对象集合</returns>
		public IEnumerable<TEntity> Import<TEntity>(string excelPath) where TEntity : Entity, new()
		{
			return new ExcelImportProviderFactory().GetProvider<TEntity>().Import(excelPath);
		}
	}
}
