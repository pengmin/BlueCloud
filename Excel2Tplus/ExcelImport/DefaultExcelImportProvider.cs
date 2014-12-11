using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 默认的Excel导入提供程序。通过匹配Excel数据中的列名称与TEntity对象的属性名称，将数据导入到对象中。
	/// </summary>
	/// <typeparam name="TEntity">单据对象类型</typeparam>
	class DefaultExcelImportProvider<TEntity> : IExcelImportProvider<TEntity> where TEntity : Entity, new()
	{
		public IEnumerable<TEntity> Import(string excelPath)
		{
			var eh = new ExcelHelper(excelPath, true);
			var dt = eh.Read();
			var list = new List<TEntity>();
			var type = typeof(TEntity);
			foreach (DataRow row in dt.Rows)
			{
				if (string.IsNullOrWhiteSpace(row[0] as string))
				{
					continue;
				}
				var obj = new TEntity();
				foreach (DataColumn cln in dt.Columns)
				{
					type.GetProperty(cln.ColumnName).SetValue(obj, (row[cln] is DBNull ? string.Empty : row[cln]), null);
				}
				list.Add(obj);
			}
			return list;
		}
	}
}
