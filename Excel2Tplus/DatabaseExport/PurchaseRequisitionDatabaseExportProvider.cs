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
			return new string[] { r.ToString() };
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildPu_PurchaseRequisitionInsertSql(PurchaseRequisition obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into Pu_PurchaseRequisition(id,requireDate,code,pubuserdefnvc1)";
			sql += " values(@id,@requireDate,@code,@pubuserdefnvc1);";
			var dbParams = new List<DbParameter>
			{
				new SqlParameter("@id",id),
				new SqlParameter("@requireDate", obj.单据日期),
				new SqlParameter("@code",obj.单据编号),
				new SqlParameter("@pubuserdefnvc1",obj.所属公司)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, dbParams);
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildPu_PurchaseRequisition_bInsertSql(PurchaseRequisition obj, Guid pid)
		{
			var sql = "insert into Pu_PurchaseRequisition_b(id,idPurchaseRequisitionDTO,idinventory,priuserdefdecm1,priuserdefdecm2,priuserdefnvc1,priuserdefnvc2,priuserdefnvc3,priuserdefnvc4,quantity,price)";
			sql += " values(NEWID(),@idPurchaseRequisitionDTO,@idinventory,@priuserdefdecm1,@priuserdefdecm2,@priuserdefnvc1,@priuserdefnvc2,@priuserdefnvc3,@priuserdefnvc4,@quantity,@price)";
			var dbParams = new List<DbParameter>
			{
				new SqlParameter("@idPurchaseRequisitionDTO",pid),
				new SqlParameter("@idinventory",obj.存货编码),
				new SqlParameter("@priuserdefdecm1",obj.正品零售价),
				new SqlParameter("@priuserdefdecm2",obj.年份),
				new SqlParameter("@priuserdefnvc1",obj.颜色),
				new SqlParameter("@priuserdefnvc2",obj.尺码),
				new SqlParameter("@priuserdefnvc3",obj.款号),
				new SqlParameter("@priuserdefnvc4",obj.男女),
				//new SqlParameter("@priuserdefnvc5",obj.货号颜色代码),
				new SqlParameter("@quantity",obj.数量),
				new SqlParameter("@price",obj.UseBookPrice?obj.BookPrice:obj.BillPrice)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, dbParams);
		}
	}
}
