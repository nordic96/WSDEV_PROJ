<%@ Page Title="" Language="C#" MasterPageFile="../Site.master" AutoEventWireup="true" CodeFile="CalculatePostRate.aspx.cs" Inherits="WebPages_CalculatePostRate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2>Calculate Post Rate (SG)</h2>
    <p>This information is provided by SingPost. Click the logo below to visit SingPost official website.</p>
    <a href="https://www.singpost.com"><img src="../Content/singpost-logo.png" style="width:160px;height:53px;" /></a>
    <br /><br /> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            Please choose one of the postage calculation categories :
            <asp:DropDownList ID="ddlPostCalCategory" runat="server" OnSelectedIndexChanged="ddlPostCalCategory_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="local">Local</asp:ListItem>
                <asp:ListItem Value="air">Overseas (Airmail)</asp:ListItem>
                <asp:ListItem Value="surface">Overseas (Surface)</asp:ListItem>
                <asp:ListItem Value="bulk">Overseas (Bulk)</asp:ListItem>
            </asp:DropDownList>
            <h3><asp:Label ID="lblPostTitle" runat="server" Text=""></asp:Label>Postage Calculation</h3>
            <div id="localPostCalFrm" runat="server">
                <p>*There will be an additional $2.24 (inclusive of GST) applicable for local registered article. Posts only include Letters, Postcards, Printed Papers+ And Packets/Packages.*</p>
                <table class="gh_table">
                    <tr>
                        <td>Post Weight (g)</td>
                        <td>: <asp:TextBox ID="txtPostWeightLocal" runat="server" TextMode="Number"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Post Size</td>
                        <td>: <asp:DropDownList ID="ddlPostSizeLocal" runat="server" OnSelectedIndexChanged="ddlPostSizeLocal_SelectedIndexChanged">
                            <asp:ListItem Value="empty">--Choose Size--</asp:ListItem>
                            <asp:ListItem Value="regular">Standard Regular</asp:ListItem>
                            <asp:ListItem Value="large">Standard Large</asp:ListItem>
                            <asp:ListItem Value="non">Non-Standard</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCalLocal" runat="server" Text="Calculate" OnClick="btnCalLocal_Click" /></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div id="airPostCalFrm" runat="server" class="gh_postCal_div"></div>
            <div id="surfacePostCalFrm" runat="server" class="gh_postCal_div"></div>
            <div id="bulkPostCalFrm" runat="server" class="gh_postCal_div"></div>
            <br />
            <asp:Label ID="lblTotalPrice" runat="server" Text=""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

