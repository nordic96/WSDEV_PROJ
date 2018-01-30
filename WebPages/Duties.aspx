<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Duties.aspx.cs" Inherits="WebPages_Duties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="calculationTab">
                <table>
                <tr>
                    <td>
                        <asp:Label ID="lblHSCode" runat="server" Text="HS Code"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblWeight" runat="server" Text="Weight (in kg/litres)"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTotalProductPrice" runat="server" Text="Total Product Price"></asp:Label>
                    </td>    
                    <td>
                        <asp:Label ID="lblManufactured" runat="server" Text="Manufactured Country"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlHRCode" runat="server"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="tbWeight" runat="server" TextMode="Number"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="tbTotalProductPrice" runat="server" TextMode="Number"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlManufactured" runat="server">
                            <asp:ListItem>Domestic</asp:ListItem>
                            <asp:ListItem>International</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTotPrice" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                </table>
                <div class="content_scroll">
            
                <div>
                    <br />  
                    <asp:GridView ID="gv1" runat="server"></asp:GridView>
                </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

