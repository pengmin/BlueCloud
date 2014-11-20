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
	class InputWarehouseDatabaseExportProvider : IDatabaseExportProvider
	{
		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entities.Entity
		{
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			foreach (var item in list)
			{
				Guid id;
				sqlList.Add(BuildMainInsertSql(item as InputWarehouse, out id));
				sqlList.Add(BuildDetailInsertSql(item as InputWarehouse, id));
			}

			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			sh.Open();
			var r = sh.Execute(sqlList);
			sh.Close();

			return new[] { r.ToString() };
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(InputWarehouse obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into ST_RDRecord(id,rdDirectionFlag,voucherdate,code,idpartner,iddepartment,idproject,pubuserdefnvc1,pubuserdefnvc2,pubuserdefnvc3,idwarehouse)";
			sql += " values(@id,@rdDirectionFlag,@voucherdate,@code,@idpartner,@iddepartment,@idproject,@pubuserdefnvc1,@pubuserdefnvc2,@pubuserdefnvc3,@idwarehouse);";
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",id),
				new SqlParameter("@rdDirectionFlag",1),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期)), 
				new SqlParameter("@code",obj.单据编号),
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByCode(obj.供应商)),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)), 
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@pubuserdefnvc1",obj.部门), 
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(InputWarehouse obj, Guid pid)
		{
			var sql = "insert into ST_RDRecord_b(id,idRDRecordDTO,idinventory,quantity,price)";
			sql += " values(@id,@idRDRecordDTO,@idinventory,@quantity,@price);";
			var ps = new DbParameter[]
			{
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@idRDRecordDTO",pid), 
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@price",obj.UseBookPrice?obj.BookPrice:obj.BillPrice)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}
	}
}
