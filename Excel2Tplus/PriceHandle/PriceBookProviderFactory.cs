using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;

namespace Excel2Tplus.PriceHandle
{
	/// <summary>
	/// 价格表提供程序工厂
	/// </summary>
	class PriceBookProviderFactory
	{
		/// <summary>
		/// 获取价格表提供程序
		/// </summary>
		/// <param name="entityType">单据类型</param>
		/// <returns></returns>
		public IPriceBookProvider GetProvider(Type entityType)
		{
			if (CommonHelper.VendorType.Contains(entityType))
			{
				return new VendorPriceBookProvider();
			}
			if (CommonHelper.InventoryType.Contains(entityType))
			{
				return new InventoryPriceBookProvider();
			}
			return null;
		}
	}
}
