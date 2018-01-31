<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Duties.aspx.cs" Inherits="WebPages_Duties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Duties</h2>
            <h5>Calculate the total price payable using the calculator below.</h5>
            <table class ="gh_table">
                <tr>
                    <td>
                        <asp:Label ID="lblHSCode" runat="server" Text="HS Code"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlHRCode" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblWeight" runat="server" Text="Weight (in kg/litres)"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbWeight" runat="server" TextMode="Number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTotalProductPrice" runat="server" Text="Total Product Price"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbTotalProductPrice" runat="server" TextMode="Number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblManufactured" runat="server" Text="Manufactured Country"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlManufactured" runat="server">
                        <asp:ListItem>Domestic</asp:ListItem>
                        <asp:ListItem>International</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>          
            <br />
            <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" />
            <asp:Label ID="lblTotPrice" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <h2>Search</h2>
            <h5>Search the duties rate by either HS Code/Product Description</h5>
            <asp:Label ID="lblSearchBy" runat="server" Text="Search By: "></asp:Label>
            <asp:DropDownList ID="ddlSearchBy" runat="server" Height="16px">
                <asp:ListItem Value="List of Dutiable Goods ">HS Code</asp:ListItem>
                <asp:ListItem Value="F2">Product Description</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="tbSearchBy" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearchBy" runat="server" Text="Search" OnClick="btnSearchBy_Click" />
            <asp:GridView ID="gv1" runat="server"></asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

