<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="PrintService.Config" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>系统设置</title>
	<style>
		h4 {
			margin-bottom: 0;
		}

		dl dt {
			width: 80px;
			text-align: right;
			clear: left;
		}

		dl dt, dd {
			float: left;
		}

		dl dd {
			margin: 0;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:Button runat="server" ID="save" Text="保存" OnClick="save_Click" />
		</div>
		<div>
			<h4>Sql Server服务器设置：</h4>
			<dl>
				<dt>服务器：</dt>
				<dd>
					<asp:TextBox runat="server" ID="server"></asp:TextBox></dd>
			</dl>
			<dl>
				<dt>数据库：</dt>
				<dd>
					<asp:TextBox runat="server" ID="Database"></asp:TextBox></dd>
			</dl>
			<dl>
				<dt>账号：</dt>
				<dd>
					<asp:TextBox runat="server" ID="account"></asp:TextBox></dd>
			</dl>
			<dl>
				<dt>密码：</dt>
				<dd>
					<asp:TextBox runat="server" ID="pswd"></asp:TextBox></dd>
			</dl>
		</div>
	</form>
</body>
</html>
