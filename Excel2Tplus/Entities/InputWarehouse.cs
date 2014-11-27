using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 采购入库单
	/// </summary>
	public class InputWarehouse : Entity
	{
		public override string PriceCode
		{
			get
			{
				return 供应商 + 存货编码;
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
		public string 货号 { get; set; }
		public string 每双运费 { get; set; }
		public string 仓库 { get; set; }
		public string 采购单位 { get; set; }
	}
}
