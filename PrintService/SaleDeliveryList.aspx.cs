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
@"select a.code,b.name,a.address/*b.shipmentaddress*/,c.name,convert(int,sum(quantity)) as quantity,convert(int,sum(d.taxprice)) as price, maker
from SA_SaleDelivery as a 
left join AA_Partner as b on a.idsettleCustomer=b.id
left join AA_WareHouse as c on a.idwarehouse=c.id
left join SA_SaleDelivery_b as d on a.id=d.idsaledeliverydto
where 1=1";
			if (!string.IsNullOrEmpty(this.code.Text))
			{
				sql += " and a.code like '%" + this.code.Text + "%'";
			}
			sql += " group by a.code,b.name,a.address/*b.shipmentaddress*/,c.name,maker ORDER BY a.code";

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