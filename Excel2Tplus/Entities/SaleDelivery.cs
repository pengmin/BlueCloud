using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 销货单
	/// </summary>
	public class SaleDelivery : Entity
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
		public string 仓库 { get; set; }
		public string 客户 { get; set; }
		public string 抽佣比率 { get; set; }
		public string 规格型号 { get; set; }
		public string 计量单位 { get; set; }
		public string 成本价 { get; set; }
		public string 成本金额 { get; set; }
		public string 销售订单号 { get; set; }
		public string 物流名称单号 { get; set; }
		public string 发货信息 { get; set; }
		public string 客户收货信息 { get; set; }
		public string 平台单号 { get; set; }
		public string 满减活动 { get; set; }
		public string 抵用券 { get; set; }
		public string 代收运费 { get; set; }
		public string 抽佣 { get; set; }
		public string 售价 { get { return 单价; } set { 单价 = value; } }
		public string 含税售价 { get { return 含税单价; } set { 含税单价 = value; } }
		public string 销售金额 { get; set; }
		public string 含税销售金额 { get; set; }
	}
}
