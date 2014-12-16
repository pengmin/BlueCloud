using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus.PriceHandle
{
	class VendorPriceBookProvider : IPriceBookProvider
	{
		private const string Sql = @"SELECT DISTINCT c.name+d.code AS code, a.agreementPriceFormula AS price 
FROM dbo.AA_VendorInventoryPriceDetail AS a
JOIN dbo.AA_VendorInventoryPrice AS b ON a.IdVendorInventoryPrice=b.id
JOIN dbo.AA_Partner AS c ON b.idvendor=c.id
JOIN dbo.AA_Inventory AS d ON b.idinventory= d.id
WHERE a.EffectiveStartDate<=@date AND a.EffectiveEndDate>=@date";

		public IEnumerable<PriceBook> Get(int level)
		{
			var helper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			helper.Open();
			using (var rd = helper.Reader(Sql, new SqlParameter("@date", DateTime.Now.ToString("yyyy-MM-dd"))))
			{
				var list = new List<PriceBook>();
				while (rd.Read())
				{
					decimal price;
					decimal.TryParse(rd["price"].ToString(), out price);
					list.Add(new PriceBook { Code = rd["code"].ToString(), Price = price });
				}
				rd.Close();
				return list;
			}
		}
	}
}
