using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Excel2Tplus.SysConfig;
using NPOI.SS.Formula.Functions;

namespace Excel2Tplus.Common
{
	/// <summary>
	/// T+数据库帮助器，用于反查各种数据
	/// </summary>
	public class TplusDatabaseHelper
	{
		/// <summary>
		/// 获取单例对象
		/// </summary>
		public static TplusDatabaseHelper Instance
		{
			get { return _instance ?? (_instance = new TplusDatabaseHelper()); }
		}
		private static TplusDatabaseHelper _instance;

		/// <summary>
		/// Sql帮助器
		/// </summary>
		private readonly SqlHelper _sqlHelper;

		private TplusDatabaseHelper()
		{
			_sqlHelper = new SqlHelper(new SysConfigManager().Get().DbConfig.GetConnectionString());
		}

		private DataTable _department;
		/// <summary>
		/// 依据部门名称获取部门id
		/// </summary>
		/// <param name="name">部门名称</param>
		/// <returns>部门id</returns>
		public object GetDepartmentIdByName(string name)
		{
			if (_department == null)
			{
				_sqlHelper.Open();
				_department = _sqlHelper.GetDataTable("SELECT * FROM dbo.AA_Department");
				_sqlHelper.Close();
			}
			foreach (var row in _department.Rows.Cast<DataRow>().Where(row => String.Equals(row["name"].ToString(), name, StringComparison.CurrentCultureIgnoreCase)))
			{
				return (Guid)row["id"];
			}
			return DBNull.Value;
		}

		private DataTable _inventory;
		/// <summary>
		/// 依据存货编码获取存货id
		/// </summary>
		/// <param name="code">存货编码</param>
		/// <returns>存货id</returns>
		public object GetInventoryIdByCode(string code)
		{
			if (_inventory == null)
			{
				_sqlHelper.Open();
				_inventory = _sqlHelper.GetDataTable("SELECT * FROM AA_Inventory");
				_sqlHelper.Close();
			}
			foreach (var row in _inventory.Rows.Cast<DataRow>().Where(row => String.Equals(row["code"].ToString(), code, StringComparison.CurrentCultureIgnoreCase)))
			{
				return (Guid)row["id"];
			}
			return DBNull.Value;
		}

		private DataTable _warehouse;
		/// <summary>
		/// 依据仓库名称获取仓库id
		/// </summary>
		/// <param name="name">仓库名称</param>
		/// <returns>仓库id</returns>
		public object GetWarehouseIdByName(string name)
		{
			if (_warehouse == null)
			{
				_sqlHelper.Open();
				_warehouse = _sqlHelper.GetDataTable("SELECT * FROM AA_Warehouse");
				_sqlHelper.Close();
			}
			foreach (var row in _warehouse.Rows.Cast<DataRow>().Where(row => String.Equals(row["name"].ToString(), name, StringComparison.CurrentCultureIgnoreCase)))
			{
				return (Guid)row["id"];
			}
			return DBNull.Value;
		}

		private DataTable _partner;
		/// <summary>
		/// 依据供应商名称获取供应商id。也是往来单位。
		/// </summary>
		/// <param name="name">供应商名称</param>
		/// <returns>供应商id</returns>
		public object GetPartnerIdByName(string name)
		{
			if (_partner == null)
			{
				_sqlHelper.Open();
				_partner = _sqlHelper.GetDataTable("SELECT * FROM dbo.AA_Partner");
				_sqlHelper.Close();
			}
			foreach (var row in _partner.Rows.Cast<DataRow>().Where(row => String.Equals(row["name"].ToString(), name, StringComparison.CurrentCultureIgnoreCase)))
			{
				return (Guid)row["id"];
			}
			return DBNull.Value;
		}
		/// <summary>
		/// 依据供应商编码获取供应商id。也是往来单位。
		/// </summary>
		/// <param name="code">供应商编码</param>
		/// <returns>供应商id</returns>
		public object GetPartnerIdByCode(string code)
		{
			if (_partner == null)
			{
				_sqlHelper.Open();
				_partner = _sqlHelper.GetDataTable("SELECT * FROM dbo.AA_Partner");
				_sqlHelper.Close();
			}
			foreach (var row in _partner.Rows.Cast<DataRow>().Where(row => String.Equals(row["code"].ToString(), code, StringComparison.CurrentCultureIgnoreCase)))
			{
				return (Guid)row["id"];
			}
			return DBNull.Value;
		}

		private DataTable _project;
		/// <summary>
		/// 依据项目名称获取项目id
		/// </summary>
		/// <param name="name">项目名称</param>
		/// <returns>项目id</returns>
		public object GetProjectIdByName(string name)
		{
			if (_project == null)
			{
				_sqlHelper.Open();
				_project = _sqlHelper.GetDataTable("SELECT * FROM dbo.AA_Project");
				_sqlHelper.Close();
			}
			foreach (var row in _project.Rows.Cast<DataRow>().Where(row => String.Equals(row["name"].ToString(), name, StringComparison.CurrentCultureIgnoreCase)))
			{
				return (Guid)row["id"];
			}
			return DBNull.Value;
		}

		private DataTable _unit;
		/// <summary>
		/// 依据计量单位名称获取单位id
		/// </summary>
		/// <param name="name">计量单位名称</param>
		/// <returns>计量单位id</returns>
		public object GetUnitIdByName(string name)
		{
			if (_unit == null)
			{
				_sqlHelper.Open();
				_unit = _sqlHelper.GetDataTable("SELECT * FROM dbo.AA_Unit");
				_sqlHelper.Close();
			}
			foreach (var row in _unit.Rows.Cast<DataRow>().Where(row => String.Equals(row["name"].ToString(), name, StringComparison.CurrentCultureIgnoreCase)))
			{
				return (Guid)row["id"];
			}
			return DBNull.Value;
		}

		private DataTable _userRole;
		/// <summary>
		/// 获取订单的编码格式
		/// </summary>
		/// <param name="name">订单的中文名称</param>
		/// <param name="serialnoLength">订单序号的长度</param>
		/// <returns>订单编码的前缀</returns>
		public string GetVoucherCodePrefix(string name, out int serialnoLength)
		{
			var sql = @"SELECT a.*,b.name AS voucherName
FROM dbo.SM_UsedRule AS a
JOIN dbo.SM_VoucherType AS b ON a.idvouchertype=b.id";
			if (_userRole == null)
			{
				_sqlHelper.Open();
				_userRole = _sqlHelper.GetDataTable(sql);
				_sqlHelper.Close();
			}
			foreach (var row in _userRole.Rows.Cast<DataRow>().Where(row => (string)row["voucherName"] == name))
			{
				serialnoLength = (int)row["serialnolength"];
				return (string)row["allprefixion"];
			}
			serialnoLength = 0;
			return null;
		}
		/// <summary>
		/// 获取单据已使用的最大编号
		/// </summary>
		/// <param name="voucher">单据的表名称</param>
		/// <param name="length">单据编码长度</param>
		/// <returns>已使用的最大编号</returns>
		public int GetMaxSerialno(string voucher, int length)
		{
			var sql = "SELECT RIGHT(MAX(code),{1}) FROM {0}";
			_sqlHelper.Open();
			var r = _sqlHelper.Scalar(string.Format(sql, voucher, length));
			_sqlHelper.Close();

			int i;
			int.TryParse(r.ToString(), out i);
			return i;
		}
		/// <summary>
		/// 获取部门的员工id，只获取找到的第一个员工的id
		/// </summary>
		/// <param name="name">部门尖</param>
		/// <returns>员工id</returns>
		public object GetPensonIdByDepartmentName(string name)
		{
			var sql = "SELECT a.id FROM AA_Person AS a JOIN dbo.AA_Department AS b ON a.iddepartment=b.id WHERE b.name=@name";
			_sqlHelper.Open();
			var r = _sqlHelper.Scalar(sql, new SqlParameter("@name", name));
			_sqlHelper.Close();

			return r is Guid ? r : DBNull.Value;
		}
		/// <summary>
		/// 判断指据编码的单据是否已存在
		/// </summary>
		/// <param name="code">单据编码</param>
		/// <param name="voucher">单据表名称</param>
		/// <returns></returns>
		public bool ExistVoucher(string code, string voucher)
		{
			var sql = "select count(0) from {0} where code=@code";
			_sqlHelper.Open();
			var r = _sqlHelper.Scalar(string.Format(sql, voucher), new SqlParameter("@code", code));
			_sqlHelper.Close();
			return (int)r > 0;
		}
		/// <summary>
		/// 获取单据状态id
		/// </summary>
		/// <param name="name">状态名称</param>
		/// <returns>状态id</returns>
		public object GetVoucherStateIdByStateName(string name)
		{
			var sql = "select a.id from eap_EnumItem as a join eap_Enum as b on a.idEnum=b.id where b.Title='单据状态' and a.Name=@name";
			_sqlHelper.Open();
			var r = _sqlHelper.Scalar(sql, new SqlParameter("@name", name));
			_sqlHelper.Close();
			return r is Guid ? r : DBNull.Value;
		}
		/// <summary>
		/// 获取采购单付款方式id
		/// </summary>
		/// <param name="name">付款方式名称</param>
		/// <returns></returns>
		public object GetPayTypeIdByTypeName(string name)
		{
			var sql = "select a.id from eap_EnumItem as a join eap_Enum as b on a.idEnum=b.id where b.Title='采购付款方式' and a.Name=@name";
			_sqlHelper.Open();
			var r = _sqlHelper.Scalar(sql, new SqlParameter("@name", name));
			_sqlHelper.Close();
			return r is Guid ? r : DBNull.Value;
		}
	}
}
