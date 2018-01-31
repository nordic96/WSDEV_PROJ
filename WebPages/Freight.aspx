<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Freight.aspx.cs" Inherits="WebPages_Freight" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Local Address Search</h2>
            <p>Search for any address located in Singapore.This information is provided by OneMapDataSG. Click the logo below to visit the website.</p>
            <a href="https://docs.onemap.sg/"><img src="../Content/onemap-logo.png" style="width:150px;height:150px;"/></a>
            <table class="gh_table">
                <tr>
                    <td>Search Address</td>
                    <td>: <asp:TextBox ID="txtSearchAddress" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
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
            <asp:GridView ID="gvTest" runat="server"></asp:GridView>
            <br /><br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

