<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="MainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logistics & Trading</title>
    <link rel="stylesheet" href="../CSS/main.css"/>
</head>
<body>
            <div class="menu_bar">
            <table class="menu_bar">
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
        </div>
        <div class="title_bar1">
            <img src="../Image/Billy.png" width="70px"/>
        </div>
        <div class="title_bar2">
            <h2>TRADE & LOGISTICS</h2>
        </div>

        <form id="form1" runat="server">
        <div class="content">
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br /><br />
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            <table>
            <tr>
            <td>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
            <td>
            <asp:Button ID="Button1" runat="server" Text="Search" OnClick="Button1_Click" style="height: 26px" /></td>
            </tr>
            </table>
        </div>
        <div class="content_scroll">
            
            <div>
                <br />  
                <asp:GridView ID="gv1" runat="server"></asp:GridView>
            </div>
            
        </div>
            </form>
</body>
</html>
