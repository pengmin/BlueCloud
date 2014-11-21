using System;
using System.Collections.Generic;
using System.Data;
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
			if (_project == null)
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
	}
}
