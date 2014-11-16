using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 单据实体
	/// </summary>
	public class Entity
	{
		/// <summary>
		/// 价格本编码
		/// </summary>
		public string PriceCode { get; set; }
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
		public decimal Differential
		{
			get
			{
				return BookPrice - BillPrice;
			}
		}
		/// <summary>
		/// 是否使用价格本价格
		/// </summary>
		public bool UseBookPrice { get; set; }
	}
}
