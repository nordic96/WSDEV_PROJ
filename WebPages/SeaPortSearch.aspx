<%@ Page Title="Sea Port Search" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SeaPortSearch.aspx.cs" Inherits="WebPages_SeaPortSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>Search Sea Port Information</h2>
    <p>This information is provided by PORT SERVICES CORPORATION (S.A.O.G), PORT SULTAN QABOOS. Visit this website for more information.</p>
    <a href="http://www.pscoman.com/" ><img src="../Content/SAOG-logo.PNG" style="width:497px;height:96px;"/></a>  
    <br />
    <h5>Select the drop down list to search specific port information.</h5>
    <table>
        <tr>
            <td>Search By : <asp:DropDownList ID="ddlSearchPort" runat="server" OnSelectedIndexChanged="ddlSearchPort_SelectedIndexChanged"></asp:DropDownList></td>
            <td><asp:TextBox ID="txtSearchPort" runat="server"></asp:TextBox></td>
            <td><asp:Button ID="btnSearchPort" runat="server" Text="Search" OnClick="btnSearchPort_Click" /></td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <asp:GridView ID="gvSeaPort" runat="server" Width="900px" PageSize="5"></asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

