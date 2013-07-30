using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrintService
{
	public partial class SaleDeliveryList : System.Web.UI.Page
	{
		private string GetSql()
		{
			var sql =
@"SELECT code,name,address,name1,SUM(quantity) AS quantity, SUM(price) AS price,maker
FROM(
	SELECT a.code,c.name,a.address,d.name AS name1,CONVERT(INT,b.quantity) AS quantity,CONVERT(DECIMAL(18,2),b.quantity*b.taxPrice) AS price,a.maker, CONVERT(VARCHAR(10),a.createdtime) AS createdtime
	FROM dbo.SA_SaleDelivery AS a
	LEFT JOIN dbo.SA_SaleDelivery_b AS b ON a.id=b.idSaleDeliveryDTO
	LEFT JOIN dbo.AA_Partner AS c ON a.idsettleCustomer=c.id
	LEFT JOIN dbo.AA_Warehouse AS d ON a.idwarehouse=d.id
) AS temp
WHERE 1=1";
			if (!string.IsNullOrEmpty(this.code.Text))
			{
				sql += " and a.code like '%" + this.code.Text + "%'";
			}
			sql += " GROUP BY temp.code,temp.name,temp.address,name1,temp.maker,temp.createdtime ORDER BY temp.createdtime DESC,temp.code";

			return sql;
		}
		protected void Page_Load(object sender, EventArgs e)
		{
			//if (!this.IsPostBack)
			//{
				this.SqlDataSource1.ConnectionString = ConfigHelper.GetInstance(this.Server.MapPath("~/Config.xml")).SqlConnectionString();
				this.SqlDataSource1.SelectCommand = this.GetSql();
			//}
		}

		protected void search_Click(object sender, EventArgs e)
		{
			this.SqlDataSource1.ConnectionString = ConfigHelper.GetInstance(this.Server.MapPath("~/Config.xml")).SqlConnectionString();
			this.SqlDataSource1.SelectCommand = this.GetSql();
			this.GridView1.DataBind();
		}
	}
}