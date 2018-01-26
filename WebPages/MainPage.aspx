<%@ Page Title="Home" Language="C#" MasterPageFile="../Site.Master" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<%--    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>--%>

<%--    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>

        </div>
    </div>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div class="row">
        <div class="col-md-4">
            <table style="width:1000px">
                <tr><td><h2><asp:Label ID="lblRssFeedSubject" runat="server" Text=""></asp:Label>News</h2></td></tr>
                <tr><td><p>This news feed is provided by RealWire (RSS). Visit this website for more information <a href="https://www.realwire.com/">(https://www.realwire.com/)</a>.</p></td></tr>
                <tr><td>Change news topic: </td>
                    <td>
                        <asp:DropDownList ID="ddlNewsTopic" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNewsTopic_SelectedIndexChanged">
                            <asp:ListItem>Maritime</asp:ListItem>
                            <asp:ListItem>Cargo</asp:ListItem>
                            <asp:ListItem>Freight</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
            </table>

            <asp:GridView ID="gvRSS" runat="server" Witdh="1000px" AutoGenerateColumns="false" BorderStyle="None" GridLines="None">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table style="width:1000px;border-spacing:5px">
                                <tr>
                                    <td><h3 style="color:#B52A33"><%#Eval("Title") %></h3></td>
                                </tr>
                                <tr>
                                    <td><%#Eval("PublishDate") %></td>
                                </tr>
                                <tr>
                                    <td><%#Eval("Description") %></td>
                                </tr>
                                <tr>
                                    <td style="text-align:right">
                                        <a href='<%#Eval("Link") %>' target="_blank">Read More...</a>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
