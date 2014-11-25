﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus.PriceHandle
{
	class VendorPriceBookProvider : IPriceBookProvider
	{
		private const string Sql = @"select * from AA_VendorInventoryPrice";

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
					decimal.TryParse(rd["agreementPrice"].ToString(), out price);
					list.Add(new PriceBook { Code = rd["name"].ToString(), Price = price });
				}
				rd.Close();
				return list;
			}
		}
	}
}
