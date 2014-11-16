using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.PriceHandle
{
	/// <summary>
	/// 价格表
	/// </summary>
	class PriceBook
	{
		/// <summary>
		/// 价格表编码
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// 价格
		/// </summary>
		public decimal Price { get; set; }
	}
}
