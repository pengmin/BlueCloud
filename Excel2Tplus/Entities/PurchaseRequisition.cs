using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 请购单
	/// </summary>
	public class PurchaseRequisition : Entity
	{
		public string 单据日期 { get; set; }
		public string 单据编号 { get; set; }
		public string 所属公司 { get; set; }
		public string 存货编码 { get { return InventoryCode; } set { InventoryCode = value; } }
		public string 存货 { get { return InventoryName; } set { InventoryName = value; } }
		public string 规格型号 { get; set; }
		public string 正品零售价 { get; set; }
		public string 年份 { get; set; }
		public string 颜色 { get; set; }
		public string 尺码 { get; set; }
		public string 款号 { get; set; }
		public string 男女 { get; set; }
		public string 货号颜色代码 { get; set; }
		public string 品牌 { get; set; }
		public string 采购单位 { get; set; }
		public string 数量 { get; set; }
		public string 单价 { get { return BillPrice.ToString(); } set { BillPrice = decimal.Parse(value); } }
		public string 税率 { get; set; }
		public string 含税单价 { get; set; }
		public string 金额 { get; set; }
		public string 税额 { get; set; }
		public string 含税金额 { get; set; }
	}
}
