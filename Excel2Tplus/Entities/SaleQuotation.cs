using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 报价单
	/// </summary>
	public class SaleQuotation : Entity
	{
		public override string PriceCode
		{
			get
			{
				return InventoryCode;
			}
			set
			{
				InventoryCode = value;
			}
		}
		public string 客户 { get; set; }
		public string 规格型号 { get; set; }
		public string 销售单位 { get; set; }
		public object 仓库 { get; set; }
	}
}
