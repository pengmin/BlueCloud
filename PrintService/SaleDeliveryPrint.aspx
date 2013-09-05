<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleDeliveryPrint.aspx.cs" Inherits="PrintService.WebForm1" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" EnableParameterPrompt="False" HasCrystalLogo="False" HasDrilldownTabs="False" HasRefreshButton="True" HasSearchButton="False" HasToggleGroupTreeButton="False" HasToggleParameterPanelButton="False" HasZoomFactorList="False" Height="50px" PrintMode="ActiveX" ToolPanelView="None" Width="350px" />
		</div>
	</form>
</body>
</html>
