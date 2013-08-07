<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleDeliveryPrint.aspx.cs" Inherits="PrintService.SaleDeliveryPrint" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<style>
		#top
		{
			position: fixed;
			_position: absolute;
			top: 5px;
			right: 5px;
			_bottom: auto;
			_top: expression(eval(document.documentElement.scrollTop+5));
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager runat="server"></asp:ScriptManager>
		<div id="top">

			<asp:RadioButtonList ID="type" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="type_SelectedIndexChanged">
				<asp:ListItem Value="1">无单价金额</asp:ListItem>
				<asp:ListItem Value="2">带单价金额</asp:ListItem>
			</asp:RadioButtonList>

		</div>
		<div>
			<rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="100%" Width="100%" PageCountMode="Actual" ShowExportControls="False" ShowFindControls="False">
			</rsweb:ReportViewer>
		</div>
	</form>
</body>
</html>
