﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus.PriceHandle
{
	/// <summary>
	/// 存货价格表提供程序
	/// </summary>
	class InventoryPriceBookProvider : IPriceBookProvider
	{
		private const string Sql = @"SELECT a.code,b.invSCost1 FROM dbo.AA_Inventory AS a
JOIN AA_InventoryPrice AS b ON b.idinventory=a.id";

		public IEnumerable<PriceBook> Get()
		{
			var helper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			helper.Open();
			using (var rd = helper.Reader(Sql))
			{
				var list = new List<PriceBook>();
				while (rd.Read())
				{
					decimal price;
					decimal.TryParse(rd["invSCost1"].ToString(), out price);
					list.Add(new PriceBook { Code = rd["code"].ToString(), Price = price });
				}
				rd.Close();
				return list;
			}
		}
	}
}
