<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomDuties.aspx.cs" Inherits="CustomDuties" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logistics & Trading</title>
    <link rel="stylesheet" href="CSS/main.css"/>
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
        <div class="title_bar">
            <h2>TRADE & LOGISTICS</h2>
            <img src="" />
        </div>

        <form id="form1" runat="server">
        <div class="calculationTab">
            <table>
            <tr>
                <td>
                    <asp:Label ID="lblHSCode" runat="server" Text="HS Code"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblWeight" runat="server" Text="Weight (in kg/litres)"></asp:Label>
                <td>
                    <asp:Label ID="lblManufactured" runat="server" Text="Manufactured Country"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlHRCode" runat="server" AutoPostBack="True"></asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="tbWeight" runat="server" TextMode="Number"></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddlManufactured" runat="server" AutoPostBack="True">
                        <asp:ListItem>Domestic</asp:ListItem>
                        <asp:ListItem>International</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTotPriceText" runat="server" Text="Total Price: "></asp:Label>
                    <asp:Label ID="lblTotPrice" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            </table>
            <div class="content_scroll">
            
            <div>
                <br />  
                <asp:GridView ID="gv1" runat="server"></asp:GridView>
            </div>
            </div>
            </form>
</body>
</html>