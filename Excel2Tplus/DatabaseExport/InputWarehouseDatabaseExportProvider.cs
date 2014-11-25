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
	/// 采购入库单数据库导出提供程序
	/// </summary>
	class InputWarehouseDatabaseExportProvider : IDatabaseExportProvider
	{
		private static string prefix;//单据编码前缀
		private static int serialno = 0;//单据编码起始编号
		private static int length;//单据编号长度

		public InputWarehouseDatabaseExportProvider()
		{
			prefix = string.Empty;
			serialno = 0;
			length = 0;
		}

		public IEnumerable<string> Export<TEntity>(IEnumerable<TEntity> list) where TEntity : Entities.Entity
		{
			if (CommonHelper.GetElementType(list.GetType()) != typeof(InputWarehouse))
			{
				throw new Exception("单据类型不是采购入库单类型");
			}
			var msgList = new List<string>();
			var sqlList = new List<Tuple<string, IEnumerable<DbParameter>>>();
			Guid id = Guid.Empty;//单据主表id
			string code = null;//单据编号

			prefix = TplusDatabaseHelper.Instance.GetVoucherCodePrefix("采购入库单", out length);
			serialno = TplusDatabaseHelper.Instance.GetMaxSerialno("ST_RDRecord", length);
			foreach (var item in list.Cast<InputWarehouse>())
			{
				if (TplusDatabaseHelper.Instance.ExistVoucher(item.单据编号, "ST_RDRecord"))
				{
					msgList.Add("单据编码：" + item.单据编号 + "已存在");
					continue;
				}
				if (code != item.单据编号)
				{
					code = item.单据编号;
					sqlList.Add(BuildMainInsertSql(item, out id));
				}
				sqlList.Add(BuildDetailInsertSql(item, id));
			}

			var sh = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
			sh.Open();
			msgList.Add(sh.Execute(sqlList).ToString());
			sh.Close();

			return msgList;
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildMainInsertSql(InputWarehouse obj, out Guid id)
		{
			id = Guid.NewGuid();

			var sql = "insert into ST_RDRecord(createdtime,id,rdDirectionFlag,voucherdate,code,idpartner,iddepartment,idproject,pubuserdefnvc1,pubuserdefnvc2,pubuserdefnvc3,idwarehouse,voucherState)";
			sql += " values(@createdtime,@id,@rdDirectionFlag,@voucherdate,@code,@idpartner,@iddepartment,@idproject,@pubuserdefnvc1,@pubuserdefnvc2,@pubuserdefnvc3,@idwarehouse,@voucherState);";
			var ps = new DbParameter[]
			{
				new SqlParameter("@createdtime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), 
				new SqlParameter("@id",id),
				new SqlParameter("@rdDirectionFlag",true),
				new SqlParameter("@voucherdate",DateTime.Parse(obj.单据日期).ToString("yyyy-MM-dd HH:mm:ss")), 
				new SqlParameter("@code",string.IsNullOrWhiteSpace(obj.单据编号)?prefix+(++serialno).ToString().PadLeft(length,'0'):obj.单据编号),
				new SqlParameter("@idpartner",TplusDatabaseHelper.Instance.GetPartnerIdByCode(obj.供应商)),
				new SqlParameter("@iddepartment",TplusDatabaseHelper.Instance.GetDepartmentIdByName(obj.所属公司)), 
				new SqlParameter("@idproject",TplusDatabaseHelper.Instance.GetProjectIdByName(obj.项目)),
				new SqlParameter("@pubuserdefnvc1",obj.部门), 
				new SqlParameter("@pubuserdefnvc2",obj.业务员),
				new SqlParameter("@pubuserdefnvc3",obj.退货日期),
				new SqlParameter("@idwarehouse",TplusDatabaseHelper.Instance.GetWarehouseIdByName(obj.仓库)),
				new SqlParameter("@voucherState",TplusDatabaseHelper.Instance.GetVoucherStateIdByStateName("未审"))
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}

		private static Tuple<string, IEnumerable<DbParameter>> BuildDetailInsertSql(InputWarehouse obj, Guid pid)
		{
			var sql = "insert into ST_RDRecord_b(createdtime,id,idRDRecordDTO,idinventory,idunit,quantity,price,taxRate,taxPrice,amount,taxAmount,priuserdefdecm1)";
			sql += " values(@createdtime,@id,@idRDRecordDTO,@idinventory,@idunit,@quantity,@price,@taxRate,@taxPrice,@amount,@taxAmount,@priuserdefdecm1);";
			double tr;//税率
			decimal yf;//运费
			var ps = new DbParameter[]
			{
				new SqlParameter("@createdtime",DateTime.Now), 
				new SqlParameter("@id",Guid.NewGuid()), 
				new SqlParameter("@idRDRecordDTO",pid), 
				new SqlParameter("@idinventory",TplusDatabaseHelper.Instance.GetInventoryIdByCode(obj.存货编码)), 
				new SqlParameter("@idunit",TplusDatabaseHelper.Instance.GetUnitIdByName(obj.采购单位)),
				new SqlParameter("@quantity",obj.数量), 
				new SqlParameter("@price",obj.UseBookPrice?obj.BookPrice:obj.BillPrice),
				new SqlParameter("@taxRate",(double.TryParse(obj.税率,out tr)?tr:tr)/100),
				new SqlParameter("@taxPrice",obj.含税单价),
				new SqlParameter("@amount",obj.金额),
				//todo:税额没找到对应的字段
				new SqlParameter("@taxAmount",obj.含税金额),
				new SqlParameter("@priuserdefdecm1",decimal.TryParse(obj.每双运费,out yf)?yf:yf)
			};

			return new Tuple<string, IEnumerable<DbParameter>>(sql, ps);
		}
	}
}
