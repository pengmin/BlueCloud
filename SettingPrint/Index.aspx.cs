using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SettingPrint
{
	public partial class Index : Page
	{
		protected DataTable Data { get; set; }
		protected int Total { get; set; }
		protected int PageSize { get; set; }
		protected int PageIndex { get; set; }
		protected int PageCount
		{
			get { return Total / PageSize + ((Total % PageSize) > 0 ? 1 : 0); }
		}

		private static string ConnStr
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["db"].ConnectionString;
			}
		}

		private string PrintType
		{
			get { return Request["pType"] ?? "0"; }
		}

		private string Customer
		{
			get { return Request["customer"]; }
		}

		public Index()
		{
			PageIndex = 1;
			PageSize = 30;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request["page"]))
			{
				PageIndex = int.Parse(Request["page"]);
			}
			Total = GetTotal();
			Data = GetData();
		}

		private int GetTotal()
		{
			var sql =
				@"SELECT COUNT(0)
	FROM dbo.SA_SaleDelivery AS a
	JOIN dbo.AA_Partner AS b ON b.id=a.idcustomer
	JOIN dbo.AA_PartnerAddress AS c ON c.idpartner=b.id
	JOIN dbo.AA_Person AS d ON d.id=b.idsaleman
	WHERE 1=1{0}{1}";
			var helper = new SqlHelper(ConnStr);
			var ptype = string.Empty;
			var customer = string.Empty;

			switch (PrintType)
			{
				case "0":
					//sql = string.Format(sql, " AND (a.priuserdefdecm1 IS NULL OR a.priuserdefdecm1=0) ");
					ptype = " AND (a.priuserdefdecm1 IS NULL OR a.priuserdefdecm1=0) ";
					break;
				case "1":
					//sql = string.Format(sql, " AND (a.priuserdefdecm1>0) ");
					ptype = " AND (a.priuserdefdecm1>0) ";
					break;
			}
			if (!string.IsNullOrWhiteSpace(Customer))
			{
				customer = " AND c.contact LIKE '%" + Customer + "%'";
			}
			sql = string.Format(sql, ptype, customer);

			try
			{
				helper.Open();
				return (int)helper.Scalar(sql);
			}
			catch
			{
				return 0;
			}
			finally
			{
				helper.Close();
			}
		}

		private DataTable GetData()
		{
			var sql =
				@"SELECT id,[编号],[分管人员],[业务人电话],[业务人手机],[联系人],[到货地址],[手机号],[邮编]
FROM(SELECT ROW_NUMBER() OVER( ORDER BY a.code) AS rowNum, a.id, a.code AS [编号],d.name AS [分管人员],d.officePhoneNo AS [业务人电话],d.mobilePhoneNo AS [业务人手机],
	c.contact AS [联系人],c.shipmentAddress AS [到货地址],b.priuserdefnvc2 AS [手机号], b.priuserdefnvc1 AS [邮编]
	FROM dbo.SA_SaleDelivery AS a
	JOIN dbo.AA_Partner AS b ON b.id=a.idcustomer
	JOIN dbo.AA_PartnerAddress AS c ON c.idpartner=b.id
	JOIN dbo.AA_Person AS d ON d.id=b.idsaleman
	WHERE 1=1{0}{1}) AS temp
WHERE rowNum>=@start AND rowNum<=@end";
			var helper = new SqlHelper(ConnStr);
			var ptype = string.Empty;
			var customer = string.Empty;
			switch (PrintType)
			{
				case "0":
					//sql = string.Format(sql, " AND (a.priuserdefdecm1 IS NULL OR a.priuserdefdecm1=0) ");
					ptype = " AND (a.priuserdefdecm1 IS NULL OR a.priuserdefdecm1=0) ";
					break;
				case "1":
					//sql = string.Format(sql, " AND (a.priuserdefdecm1>0) ");
					ptype = " AND (a.priuserdefdecm1>0) ";
					break;
			}
			if (!string.IsNullOrWhiteSpace(Customer))
			{
				customer = " AND c.contact LIKE '%" + Customer + "%'";
			}
			sql = string.Format(sql, ptype, customer);

			try
			{
				helper.Open();
				return helper.GetDataTable(sql, new DbParameter[]
				{
					new SqlParameter("@start", (PageIndex - 1)*PageSize + 1),
					new SqlParameter("@end", (PageIndex)*PageSize),
				});
			}
			catch
			{
				return new DataTable();
			}
			finally
			{
				helper.Close();
			}
		}
	}
}