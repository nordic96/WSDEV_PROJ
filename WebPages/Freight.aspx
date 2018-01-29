<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Freight.aspx.cs" Inherits="WebPages_Freight" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2>Address Search</h2>
            <asp:GridView ID="gvOneAddress" runat="server"></asp:GridView>
            <br />
            <asp:Label ID="lblTest" runat="server" Text=""></asp:Label>
            <br />
            <asp:Button ID="btnPrevPage" runat="server" Text="Prev" OnClick="btnPrevPage_Click" />
            <asp:Button ID="btnNextPage" runat="server" Text="Next" OnClick="btnNextPage_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

