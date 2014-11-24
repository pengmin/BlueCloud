using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
			_excel = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
			if (_filePath.IndexOf(".xlsx") > 0) // 2007版本
				_workbook = new XSSFWorkbook(_excel);
			else if (_filePath.IndexOf(".xls") > 0) // 2003版本
				_workbook = new HSSFWorkbook(_excel);
			try
			{
				var data = new DataTable();
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
			finally
			{
				_excel.Close();
			}

		}
		/// <summary>
		/// 写入DataTable中的数据到Excel
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="sheetName">工作表</param>
		/// <returns>写入的数据数</returns>
		public int Write(DataTable data, string sheetName = "Sheet1")
		{
			_excel = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			if (_filePath.IndexOf(".xlsx") > 0) // 2007版本
				_workbook = new XSSFWorkbook();
			else if (_filePath.IndexOf(".xls") > 0) // 2003版本
				_workbook = new HSSFWorkbook();

			try
			{
				var count = 0;
				ISheet sheet;
				if (_workbook != null)
				{
					sheet = _workbook.CreateSheet(sheetName);
				}
				else
				{
					return -1;
				}

				if (_firstRowIsColumn) //写入DataTable的列名
				{
					var row = sheet.CreateRow(0);
					for (var i = 0; i < data.Columns.Count; i++)
					{
						row.CreateCell(i).SetCellValue(data.Columns[i].ColumnName);
					}
					count = 1;
				}

				for (var i = 0; i < data.Rows.Count; i++)
				{
					var row = sheet.CreateRow(count);
					for (var j = 0; j < data.Columns.Count; j++)
					{
						row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
					}
					++count;
				}
				_workbook.Write(_excel); //写入到excel

				return count;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				return -1;
			}
			finally
			{
				_excel.Close();
			}
		}

		public void Dispose()
		{
			if (_excel != null)
			{
				_excel.Close();
			}
		}
	}
}
