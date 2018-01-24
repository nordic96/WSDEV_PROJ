<%@ Page Title="Sea Port Search" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SeaPortSearch.aspx.cs" Inherits="WebPages_SeaPortSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>Search Sea Port Information</h2>
    <p>This information is provided by PORT SERVICES CORPORATION (S.A.O.G), PORT SULTAN QABOOS. Visit this website for more information. <br />
        <a href="http://www.pscoman.com/" >http://www.pscoman.com</a></p>
    <br />
    <h5>Select the drop down list to search specific port information.</h5>
    <table>
        <tr>
            <td>Search By : <asp:DropDownList ID="ddlSearchPort" runat="server"></asp:DropDownList></td>
            <td><asp:TextBox ID="txtSearchPort" runat="server"></asp:TextBox></td>
            <td><asp:Button ID="btnSearchPort" runat="server" Text="Search" /></td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvSeaPort" runat="server" Width="900px"></asp:GridView>
</asp:Content>

