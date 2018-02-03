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
            <asp:GridView ID="gvOneAddress" runat="server" Width="100%" AutoGenerateColumns="true"></asp:GridView>
            <br />
            <asp:GridView ID="gvTest" runat="server" Width="100%" AutoGenerateColumns="true"></asp:GridView>
            <br /><br />
            <h2>Freight Forwarder Contacts</h2>
            <tr>
                <td>Search By: </td>
                <td><asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                    <asp:ListItem>Country</asp:ListItem>
                    <asp:ListItem>Company Name</asp:ListItem>
                    </asp:RadioButtonList></td>
                <td><asp:DropDownList ID="ddlCountryFFC" runat="server"></asp:DropDownList></td>
                <td><asp:DropDownList ID="ddlCompanyFFC" runat="server"></asp:DropDownList></td>
                <td><asp:Button ID="btnSearchFFC" runat="server" Text="Search" OnClick="btnSearchFFC_Click" /></td>
            </tr>
            <br /><br />
            <div class="horizontal_scroll">
                <asp:GridView ID="gvFreightForwarderContact" runat="server" Width="100%"></asp:GridView>
            </div>
            <br /><br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

