using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Entities;

namespace Excel2Tplus.ExcelImport
{
	/// <summary>
	/// 请购单Excel导入提供程序
	/// </summary>
	class PurchaseRequisitionExcelImportProvider : IExcelImportProvider<PurchaseRequisition>
	{
		public IEnumerable<PurchaseRequisition> Import(string excelPath)
		{
			return new[]
			{
				new PurchaseRequisition
				{
					PriceCode="1",
					InventoryCode="123",
					InventoryName="ddd",
					BookPrice=33.3m,
					BillPrice=22.1m
				},
				new PurchaseRequisition
				{
					PriceCode="2",
					InventoryCode="123",
					InventoryName="ddd",
					BookPrice=33.3m,
					BillPrice=22.1m
				},
				new PurchaseRequisition
				{
					PriceCode="1",
					InventoryCode="123",
					InventoryName="ddd",
					BookPrice=33.3m,
					BillPrice=22.1m
				}
			};
		}
	}
}
