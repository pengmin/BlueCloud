using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// Excel导入提供程序
	/// </summary>
	/// <typeparam name="TEntity">单据类型</typeparam>
	interface IExcelImportProvider<out TEntity> where TEntity : Entity
	{
		IEnumerable<TEntity> Import(string excelPath);
	}
}
