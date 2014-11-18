using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Excel2Tplus.Common
{
	/// <summary>
	/// Excel文件读写帮助器
	/// </summary>
	class ExcelHelper : IDisposable
	{
		/// <summary>
		/// Excel文件位置
		/// </summary>
		private readonly string _filePath;
		/// <summary>
		/// 第一行是否是列名称
		/// </summary>
		private readonly bool _firstRowIsColumn;
		/// <summary>
		/// 工作薄
		/// </summary>
		private IWorkbook _workbook;
		private FileStream _excel;

		/// <summary>
		/// 实例化一个ExcelHelper
		/// </summary>
		/// <param name="filePath">Excel文件位置</param>
		/// <param name="firstRowIsColumn">第一列是否是列名称</param>
		public ExcelHelper(string filePath, bool firstRowIsColumn = false)
		{
			_filePath = filePath;
			_firstRowIsColumn = firstRowIsColumn;
		}

		/// <summary>
		/// 读取Excel数据到DataTable
		/// </summary>
		/// <param name="sheetName">工作表名称</param>
		/// <returns>保存Excel数据的DataTable对象</returns>
		public DataTable Read(string sheetName = null)
		{
			var data = new DataTable();
			try
			{
				var sheet = sheetName != null ? _workbook.GetSheet(sheetName) : _workbook.GetSheetAt(0);
				if (sheet == null) return null;

				var firstRow = sheet.GetRow(0);
				int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

				int startRow;
				if (_firstRowIsColumn)
				{
					for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
					{
						var column = new DataColumn(firstRow.GetCell(i).StringCellValue);
						data.Columns.Add(column);
					}
					startRow = sheet.FirstRowNum + 1;
				}
				else
				{
					for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
					{
						var column = new DataColumn();
						data.Columns.Add(column);
					}
					startRow = sheet.FirstRowNum;
				}

				//最后一列的标号
				var rowCount = sheet.LastRowNum;
				for (var i = startRow; i <= rowCount; ++i)
				{
					var row = sheet.GetRow(i);
					if (row == null) continue; //没有数据的行默认是null　　　　　　　

					var dataRow = data.NewRow();
					for (int j = row.FirstCellNum; j < cellCount; ++j)
					{
						if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
							dataRow[j] = row.GetCell(j).ToString();
					}
					data.Rows.Add(dataRow);
				}

				return data;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				return null;
			}

		}
		/// <summary>
		/// 打开Excel
		/// </summary>
		public void Open()
		{
			_excel = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
			if (_filePath.IndexOf(".xlsx") > 0) // 2007版本
				_workbook = new XSSFWorkbook(_excel);
			else if (_filePath.IndexOf(".xls") > 0) // 2003版本
				_workbook = new HSSFWorkbook(_excel);
		}
		/// <summary>
		/// 关闭Excel
		/// </summary>
		public void Close()
		{
			_excel.Close();
		}

		public void Dispose()
		{
			Close();
		}
	}
}
