<%@ Page Title="About VAN Trading & Logistics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <table class="gh_table" style="width:100%">
        <tr>
            <td style="vertical-align:top">
                <h3><u>Find out more about us!</u></h3>
                <p>If you want to visit our office, please look below the address details.
                    <br />Also, bus route information is provided below. Feel free to use!
                </p>
            </td>
            <td rowspan="2">
                <img src="http://upload.inven.co.kr/upload/2014/07/05/bbs/i1598057222.jpg" style="width:420px;height:292px"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvRoute" runat="server" Width="100%"></asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <h3><u>How to visit our office through bus transport</u></h3>
                <p>Below</p>
                <asp:GridView ID="gvTest" runat="server"></asp:GridView>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
