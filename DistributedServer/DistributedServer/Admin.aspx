<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="DistributedServer.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h2>Admin</h2>
        <br />
        <br />
    <table style="width:100%;">
        <tr>
            <td>&nbsp;</td>
            <td>FileName:</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style1"></td>
            <td class="auto-style1">
                <asp:TextBox ID="tbFileName" runat="server" Height="25px" Width="168px" OnTextChanged="tbFileName_TextChanged">Batch</asp:TextBox>
            </td>
            <td class="auto-style1"></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>Batch Size</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><div style="float: right" ><asp:Label ID="lblBatchSize" runat="server" Text=""></asp:Label></div></td>
            <td>
                <asp:TextBox ID="tbBatchSize" runat="server" Height="25px" Width="93px" OnTextChanged="tbBatchSize_TextChanged">1000</asp:TextBox>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <div style="float: right" ><asp:Label ID="lblFileUpload" runat="server" Text=""></asp:Label></div>
            </td>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><div style="float: right" ><asp:Label ID="lblProcess" runat="server" Text=""></asp:Label></div></td>
            <td>
                <asp:Button ID="btnProcess" runat="server" OnClick="btnProcess_Click" Text="Process" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
    </form>
    </body>
</html>
