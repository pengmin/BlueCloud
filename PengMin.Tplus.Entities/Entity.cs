using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PengMin.Tplus.Entities
{
	/// <summary>
	/// 业务实体基类
	/// </summary>
	public abstract partial class Entity
	{
	}
	/// <summary>
	/// 业务实体基类
	/// </summary>
	/// <typeparam name="TId">Id类型</typeparam>
	public abstract partial class Entity<TId> : Entity
	{
		public virtual TId Id { get; set; }
	}
}
