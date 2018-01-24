<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
    <div class="row">
        <div class="col-md-4">
            <h2>News Feed</h2>
            <p>This news feed is provided by RealWire (RSS). Visit this website for more information <a href="https://www.realwire.com/">(https://www.realwire.com/)</a>.</p>
            <asp:GridView ID="gvRSS" runat="server" Witdh="1000px" AutoGenerateColumns="false" BorderStyle="None" GridLines="None">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table width="1000px" border="0" cellpadding="0" cellspacing="5">
                                <tr>
                                    <td><h3 style="color:#3E7CFF"><%#Eval("Title") %></h3></td>
                                </tr>
                                <tr>
                                    <td><%#Eval("PublishDate") %></td>
                                </tr>
                                <tr>
                                    <td><%#Eval("Description") %></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <a href='<%#Eval("Link") %>' target="_blank">Read More...</a>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div></asp:Content>
