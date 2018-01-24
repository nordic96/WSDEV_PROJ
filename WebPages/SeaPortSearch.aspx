<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeaPortSearch.aspx.cs" Inherits="SeaPortSearch" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logistics & Trading: Sea Port Information</title>
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
            <asp:Label ID="Label2" runat="server" Text="Search for port information based on country code, country name, port code or port name."></asp:Label>
            <table>
            <tr>
                <td>
                    Search By : <asp:DropDownList ID="ddlSearchPort" runat="server"></asp:DropDownList>
                </td>
            <td>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox></td>
            <td>
            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="ButtonSearch_Click" style="height: 26px" /></td>
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
