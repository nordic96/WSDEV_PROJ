<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Freight.aspx.cs" Inherits="WebPages_Freight" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Address Search</h2>
            <p>Search for any address located in Singapore.This information is provided by OneMapDataSG. Click the logo below to visit the website.</p>
            <img src="../Content/onemap-logo.png" style="width:150px;height:150px;"/>
            <table class="gh_table" style="width:auto">
                <tr>
                    <td>Search for any</td>
                    <td>: <asp:DropDownList ID="ddlSearch" runat="server">
                        <asp:ListItem Value="logistics">Logistics</asp:ListItem>
                        <asp:ListItem Value="warehouse">Warehouse</asp:ListItem>
                        <asp:ListItem Value="trading">Trading</asp:ListItem>
                        <asp:ListItem Value="freight">Freight</asp:ListItem>
                        </asp:DropDownList>&nbsp&nbsp related addresses in Singapore.</td>
                </tr>
                <tr>
                    <td>Search Address</td>
                    <td>: <asp:TextBox ID="txtSearchAddress" runat="server" Width="96%"></asp:TextBox>
                    </td>
                    <td><asp:Button ID="btnSearch" runat="server" Text="Search" /></td>
                </tr>
            </table>
            <table style="width:100%;">
                <tr>
                    <td>
                        <asp:Label ID="lblTest" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="text-align:right"><asp:Button ID="btnPrevPage" runat="server" Text="Prev" OnClick="btnPrevPage_Click" />
                        <asp:Button ID="btnNextPage" runat="server" Text="Next" OnClick="btnNextPage_Click" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvOneAddress" runat="server" Width="100%"></asp:GridView>
            <br />
            
            <br /><br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

