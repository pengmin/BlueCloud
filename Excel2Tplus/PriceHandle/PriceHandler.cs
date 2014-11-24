using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;

namespace Excel2Tplus.PriceHandle
{
	/// <summary>
	/// 价格本价格处理程序
	/// </summary>
	class PriceHandler
	{
		/// <summary>
		/// 处理单据对象的价格本价格
		/// </summary>
		/// <typeparam name="TEntity">单据类型</typeparam>
		/// <param name="list">单据对象集合</param>
		public void Handler<TEntity>(IEnumerable<TEntity> list) where TEntity : Entity
		{
			var provider = new PriceBookProviderFactory().GetProvider(CommonHelper.GetElementType(list.GetType()));
			var pb = provider.Get();
			foreach (var item in list)
			{
				var p = pb.SingleOrDefault(_ => _.Code == item.PriceCode);
				if (p != null)
				{
					item.BookPrice = p.Price;
				}
			}
		}
	}
}
