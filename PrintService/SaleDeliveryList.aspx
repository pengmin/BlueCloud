<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleDeliveryList.aspx.cs" Inherits="PrintService.SaleDeliveryList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<style>
		dl {
			clear: left;
		}

			dl dt {
				width: 80px;
				text-align: right;
			}

			dl dd {
				margin: 0;
			}

			dl dt, dd {
				float: left;
			}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<dl>
				<dt>单据编号：</dt>
				<dd>
					<asp:TextBox runat="server" ID="code"></asp:TextBox></dd>
			</dl>
			<%--<dl>
				<dt>单据日期：</dt>
				<dd>
					<asp:TextBox runat="server" ID="start"></asp:TextBox>
				</dd>
				<dd>~</dd>
				<dd>
					<asp:TextBox runat="server" ID="end"></asp:TextBox></dd>
			</dl>--%>
			<div>
				<asp:Button runat="server" ID="search" Text="查询" OnClick="search_Click" />
			</div>
		</div>
		<div>
			<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" PageSize="25">
				<Columns>
					<asp:HyperLinkField DataTextField="code" HeaderText="单据编号" SortExpression="code" DataNavigateUrlFormatString="~/SaleDeliveryPrint.aspx?code={0}" DataNavigateUrlFields="code" Target="_blank" />
					<asp:BoundField DataField="name" HeaderText="公司" SortExpression="name" />
					<asp:BoundField DataField="address" HeaderText="地址" SortExpression="address" />
					<asp:BoundField DataField="name1" HeaderText="出库仓" SortExpression="name1" />
					<asp:BoundField DataField="quantity" HeaderText="数量" ReadOnly="True" SortExpression="quantity" />
					<asp:BoundField DataField="price" HeaderText="总价" ReadOnly="True" SortExpression="price" />
					<asp:BoundField DataField="maker" HeaderText="制单人" SortExpression="maker" />
				</Columns>
				<FooterStyle BackColor="White" ForeColor="#000066" />
				<HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
				<PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
				<RowStyle ForeColor="#000066" />
				<SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
				<SortedAscendingCellStyle BackColor="#F1F1F1" />
				<SortedAscendingHeaderStyle BackColor="#007DBB" />
				<SortedDescendingCellStyle BackColor="#CAC9C9" />
				<SortedDescendingHeaderStyle BackColor="#00547E" />
			</asp:GridView>
			<asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
		</div>
	</form>
</body>
</html>
