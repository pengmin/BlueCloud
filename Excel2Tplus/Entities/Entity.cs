using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 单据实体
	/// </summary>
	public abstract class Entity
	{
		public Entity()
		{

		}
		/// <summary>
		/// 价格本编码，依据单据类型，其值来源于存货编码或是供应商编码
		/// </summary>
		public abstract string PriceCode { get; set; }
		/// <summary>
		/// 存货编码
		/// </summary>
		public string InventoryCode { get; set; }
		/// <summary>
		/// 存货名称
		/// </summary>
		public string InventoryName { get; set; }
		/// <summary>
		/// 价格本价格
		/// </summary>
		public decimal BookPrice { get; set; }
		/// <summary>
		/// 单据价格
		/// </summary>
		public decimal BillPrice { get; set; }
		/// <summary>
		/// 差价
		/// </summary>
		[System.Xml.Serialization.XmlIgnore]
		public decimal Differential { get { return BookPrice - BillPrice; } }
		/// <summary>
		/// 是否使用价格本价格
		/// </summary>
		public bool UseBookPrice { get; set; }

		public string 单据日期 { get; set; }
		public string 单据编号 { get; set; }
		public string 所属公司 { get; set; }
		public string 存货编码 { get { return InventoryCode; } set { InventoryCode = value; } }
		public string 存货 { get { return InventoryName; } set { InventoryName = value; } }
		public string 正品零售价 { get; set; }
		public string 年份 { get; set; }
		public string 颜色 { get; set; }
		public string 尺码 { get; set; }
		public string 款号 { get; set; }
		public string 男女 { get; set; }
		public string 货号颜色代码 { get; set; }
		public string 品牌 { get; set; }
		public string 数量 { get; set; }
		public string 单价 { get { return BillPrice.ToString(); } set { BillPrice = decimal.Parse(value); } }
		public string 税率 { get; set; }
		public string 含税单价 { get; set; }
		public string 金额 { get; set; }
		public string 税额 { get; set; }
		public string 含税金额 { get; set; }
		public string 部门 { get; set; }
		public string 业务员 { get; set; }
		public string 项目 { get; set; }
		public string 退货日期 { get; set; }

		public static TEntity Copy<TEntity>(TEntity obj) where TEntity : Entity, new()
		{
			var pros = obj.GetType().GetProperties();
			var entity = new TEntity();

			foreach (var pro in pros)
			{
				var p = entity.GetType().GetProperty(pro.Name);
				try
				{
					p.SetValue(entity, pro.GetValue(obj, null), null);
				}
				catch { }
			}

			return entity;
		}
	}
}
