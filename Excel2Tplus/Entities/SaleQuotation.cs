using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 报价单
	/// </summary>
	class SaleQuotation : Entity
	{
		public string 客户 { get; set; }
		public string 项目 { get; set; }
		public string 规格型号 { get; set; }
		public string 销售单位 { get; set; }
	}
}
