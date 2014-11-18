using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Excel2Tplus.Entities;

namespace Excel2Tplus.Common
{
	/// <summary>
	/// 公共方法
	/// </summary>
	static class CommonHelper
	{
		/// <summary>
		/// 供应商价格表类型
		/// </summary>
		public static readonly IEnumerable<Type> SupplierType = new List<Type>
		{

		};
		/// <summary>
		/// 存货价格表类型
		/// </summary>
		public static readonly IEnumerable<Type> InventoryType = new List<Type>
		{
			typeof(PurchaseRequisition)
		};
		/// <summary>
		/// 获取集合对象元素的类型
		/// </summary>
		/// <param name="t">集合对象类型</param>
		/// <returns>集合元素类型</returns>
		public static Type GetElementType(Type t)
		{
			if (t.HasElementType)
				return t.GetElementType();
			if (t.GetInterface("IEnumerable") != null)
				return t.GetMethod("GetEnumerator").ReturnType.GetProperty("Current").PropertyType;
			throw new ApplicationException("类型\"" + t + "\"不是集合");
		}
		/// <summary>
		/// Xml序列化
		/// </summary>
		/// <typeparam name="TEntity">对象类型</typeparam>
		/// <param name="obj">对象</param>
		/// <returns>序列化结果</returns>
		public static StringWriter XmlSerializer<TEntity>(TEntity obj) where TEntity : class
		{
			var sw = new StringWriter();
			new XmlSerializer(typeof(TEntity)).Serialize(sw, obj);
			return sw;
		}
		/// <summary>
		/// Xml序列化，用于TEntiy的类型与obj的实际类型不一致时。
		/// </summary>
		/// <typeparam name="TEntity">对象类型</typeparam>
		/// <param name="obj">对象</param>
		/// <param name="type">对象的实际类型</param>
		/// <returns>序列化结果</returns>
		public static StringWriter XmlSerializer<TEntity>(TEntity obj, Type type) where TEntity : class
		{
			var sw = new StringWriter();
			new XmlSerializer(obj.GetType()).Serialize(sw, obj);
			return sw;
		}
		/// <summary>
		/// Xml反序列化
		/// </summary>
		/// <typeparam name="TEntity">对象类型</typeparam>
		/// <param name="xml">序列化结果</param>
		/// <returns>对象</returns>
		public static TEntity XmlDeserialize<TEntity>(StringReader xml) where TEntity : class
		{
			return new XmlSerializer(typeof(TEntity)).Deserialize(xml) as TEntity;
		}
		/// <summary>
		/// Xml反序列化
		/// </summary>
		/// <typeparam name="TEntity">对象类型</typeparam>
		/// <param name="xml">序列化结果</param>
		/// <param name="type">对象的原始类型</param>
		/// <returns>对象</returns>
		public static TEntity XmlDeserialize<TEntity>(StringReader xml, Type type) where TEntity : class
		{
			return new XmlSerializer(type).Deserialize(xml) as TEntity;
		}
	}
}
