<%@ Page Title="About VAN Trading & Logistics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
                        <asp:GridView ID="gvRoute" runat="server" Width="100%" GridLines="None"></asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h3><u>How to visit our office through public transport</u></h3>
                        <p>Choose your starting MRT Station.</p>
                        <table class="gh_table" style="width:100%">
                            <tr>
                                <td>(Line)</td>
                                <td><asp:DropDownList ID="ddlLineStart" runat="server" OnSelectedIndexChanged="ddlLineStart_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                                <td>Start</td>
                                <td>: <asp:DropDownList ID="ddlMrtStart" runat="server"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>(Line)</td>
                                <td><asp:DropDownList ID="ddlLineEnd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLineEnd_SelectedIndexChanged" Enabled="false"></asp:DropDownList></td>
                                <td>Destination</td>
                                <td>: <asp:DropDownList ID="ddlMrtEnd" runat="server" Enabled="false"/></td>
                                <td style="text-align:right"><asp:Button ID="btnSearchRoute" runat="server" Text="Search" OnClick="btnSearchRoute_Click" /></td>
                            </tr>
                            <tr>
                                <td colspan="5"><asp:Label ID="lblTotalMrtStationsToGo" runat="server" Text=""></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width:100%">
                            <tr>
                                <td style="width:50%">
                                    <asp:GridView ID="gvMrtRoute" runat="server" GridLines="None" ShowHeader="false"></asp:GridView></td>
                                <td style="width:50%">
                                    <asp:GridView ID="gvArrow" runat="server" GridLines="None" ShowHeader="false" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" runat="server" Height="15px" Width="15px"
                                                        ImageUrl='<%# Eval("arrow_url")%>'/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlBusStops" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBusStops_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Timer ID="Timer1" runat="server" Interval="60000" ontick="Timer1_Tick"></asp:Timer>
                                            <asp:Label ID="lblRefresh" runat="server"></asp:Label>
                                            <asp:GridView ID="gvBus" runat="server"></asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td><asp:Label ID="lblTest" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
