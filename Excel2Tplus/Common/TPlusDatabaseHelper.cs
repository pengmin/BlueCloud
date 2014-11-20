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
		public Guid? GetDepartmentIdByName(string name)
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
			return null;
		}

		private DataTable _inventory;
		/// <summary>
		/// 依据存货编码获取存货id
		/// </summary>
		/// <param name="code">存货编码</param>
		/// <returns>存货id</returns>
		public Guid? GetInventoryIdByCode(string code)
		{
			if (_inventory == null)
			{
				_sqlHelper.Open();
				_inventory = _sqlHelper.GetDataTable("SELECT * FROM AA_Inventory");
				_sqlHelper.Close();
			}
			foreach (var row in _inventory.Rows.Cast<DataRow>().Where(row => String.Equals(row["name"].ToString(), code, StringComparison.CurrentCultureIgnoreCase)))
			{
				return (Guid)row["id"];
			}
			return null;
		}
	}
}
