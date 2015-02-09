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
			get { return Total / PageSize + (Total % PageSize) > 0 ? 1 : 0; }
		}

		private static string ConnStr
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["db"].ConnectionString;
			}
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
			var sql = @"SELECT COUNT(0) FROM dbo.ST_RDRecord AS a
JOIN dbo.AA_Partner AS b ON b.id=a.idpartner
JOIN dbo.AA_PartnerAddress AS c ON c.idpartner=b.id
WHERE a.rdDirectionFlag=0";
			var helper = new SqlHelper(ConnStr);
			helper.Open();
			var r = helper.Scalar(sql);
			helper.Close();
			return r is int ? (int)r : 0;
		}

		private DataTable GetData()
		{
			var sql =
				@"SELECT id,[编号],[名称],[联系人],[地址],[手机],[固话]
FROM(SELECT ROW_NUMBER() OVER( ORDER BY a.code) AS rowNum, a.id, a.code AS [编号],a.name AS [名称], c.contact AS [联系人],c.shipmentAddress AS [地址],c.mobilePhone AS [手机], c.telephoneNo AS [固话] FROM dbo.ST_RDRecord AS a
JOIN dbo.AA_Partner AS b ON b.id=a.idpartner
JOIN dbo.AA_PartnerAddress AS c ON c.idpartner=b.id
WHERE a.rdDirectionFlag=0) AS temp
WHERE rowNum>=@start AND rowNum<=@end";
			var helper = new SqlHelper(ConnStr);
			try
			{
				helper.Open();
				return helper.GetDataTable(sql, new DbParameter[]
				{
					new SqlParameter("@start", (PageIndex - 1)*PageSize - 1),
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