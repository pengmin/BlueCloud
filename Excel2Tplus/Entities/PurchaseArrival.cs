using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 采购进货单
	/// </summary>
	public class PurchaseArrival : Entity
	{
		public override string PriceCode
		{
			get
			{
				return 供应商;
			}
			set
			{
				供应商 = value;
			}
		}
		public string 供应商 { get; set; }
		public string 项目 { get; set; }
		public string 部门 { get; set; }
		public string 业务员 { get; set; }
		public string 退货日期 { get; set; }
		public string 规格型号 { get; set; }
		public string 仓库 { get; set; }
		public string 采购单位 { get; set; }
	}
}
