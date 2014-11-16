using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.DatabaseExport
{
	/// <summary>
	/// 数据库导出提供程序
	/// </summary>
	interface IDatabaseExportProvider
	{
		/// <summary>
		/// 导出
		/// </summary>
		/// <typeparam name="TEntity">单据类型</typeparam>
		/// <param name="list">单据对象集合</param>
		/// <returns>导出结果</returns>
		IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entity;
	}
}
