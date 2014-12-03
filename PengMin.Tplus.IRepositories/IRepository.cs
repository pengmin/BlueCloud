using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using PengMin.Tplus.Entities;

namespace PengMin.Tplus.IRepositories
{
	/// <summary>
	/// 仓储基接口
	/// </summary>
	public partial interface IRepository
	{
	}
	public partial interface IRepository<TEntity> where TEntity : Entity
	{
		TEntity Single(Expression<Func<TEntity, bool>> predicate);
		IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
		void Add(TEntity entity);
		void Update(TEntity entity);
		void Remove(TEntity entity);
	}
}
