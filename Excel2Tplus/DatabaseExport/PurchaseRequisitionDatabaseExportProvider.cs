using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Excel2Tplus.Common;
using Excel2Tplus.Entities;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus.DatabaseExport
{
	/// <summary>
	/// 请购单数据库导出提供程序
	/// </summary>
	class PurchaseRequisitionDatabaseExportProvider : IDatabaseExportProvider
	{
		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entity
		{
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			foreach (var item in list)
			{
				Guid id;
				sqlList.Add(BuildPu_PurchaseRequisitionInsertSql(item as PurchaseRequisition, out id));
				sqlList.Add(BuildPu_PurchaseRequisition_bInsertSql(item as PurchaseRequisition, id));
			}

			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			sh.Open();
			var r = sh.Execute(sqlList);
			sh.Close();

			return new[] { r.ToString() };
		}
		/// <summary>
		/// 构造请购单主表的插入sql
		/// </summary>
		/// <param name="obj">请购单对象</param>
		/// <param name="id">id</param>
		/// <returns>sql信息</returns>
		private static Tuple<string, IEnumerable<DbParameter>> BuildPu_PurchaseRequisitionInsertSql(Entity obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into Pu_PurchaseRequisition(id,requireDate,code,iddepartment)";
			sql += " values(@id,@requireDate,@code,@iddepartment);";
			var dbParams = new List<DbParameter>
			{
				new SqlParameter("@id",id),
				new SqlParameter("@requireDate", obj.单据日期),
				new SqlParameter("@code",obj.单据编号),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司))
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, dbParams);
		}
		/// <summary>
		/// 构造请购单明细表的插入sql
		/// </summary>
		/// <param name="obj">请购单对象</param>
		/// <param name="pid">主表id</param>
		/// <returns>sql信息</returns>
		private static Tuple<string, IEnumerable<DbParameter>> BuildPu_PurchaseRequisition_bInsertSql(Entity obj, Guid pid)
		{
			var sql = "insert into Pu_PurchaseRequisition_b(id,idPurchaseRequisitionDTO,idinventory,quantity,price)";
			sql += " values(NEWID(),@idPurchaseRequisitionDTO,@idinventory,@quantity,@price)";
			var dbParams = new List<DbParameter>
			{
				new SqlParameter("@idPurchaseRequisitionDTO",pid),
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)),
				new SqlParameter("@quantity",obj.数量),
				new SqlParameter("@price",obj.UseBookPrice?obj.BookPrice:obj.BillPrice)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, dbParams);
		}
	}
}
