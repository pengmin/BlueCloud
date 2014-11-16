using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.PriceHandle
{
	/// <summary>
	/// 价格表提供程序
	/// </summary>
	interface IPriceBookProvider
	{
		/// <summary>
		/// 获取价格表
		/// </summary>
		/// <returns></returns>
		IEnumerable<PriceBook> Get();
	}
}
