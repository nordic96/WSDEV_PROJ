<%@ Page Title="Sea Port Search" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SeaPortSearch.aspx.cs" Inherits="WebPages_SeaPortSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>Search Sea Port Information</h2>
    <p>This information is provided by PORT SERVICES CORPORATION (S.A.O.G), PORT SULTAN QABOOS. Visit this website for more information.</p>
    <a href="http://www.pscoman.com/" ><img src="../Content/SAOG-logo.PNG" style="width:497px;height:96px;"/></a>  
    <br />
    <p>Select the drop down list to search specific port information.<br />*Please note that all columns are case sensitive.*</p>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width:auto">
                <tr style="height:50px;">
                    <td>Search By : <asp:DropDownList ID="ddlSearchPort" runat="server" OnSelectedIndexChanged="ddlSearchPort_SelectedIndexChanged"></asp:DropDownList></td>
                    <td><asp:TextBox ID="txtSearchPort" runat="server"></asp:TextBox>
                    </td>
                    <td><asp:Button ID="btnSearchPort" runat="server" Text="Search" OnClick="btnSearchPort_Click" /></td>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                Loading...<img src="../Content/loading-icon.gif" style="width:70px;height:50px;"/>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTest" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
            <br />
            <div class="gh_scroll">
                <asp:GridView ID="gvSeaPort" runat="server" Width="100%"></asp:GridView>
            </div>         
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

