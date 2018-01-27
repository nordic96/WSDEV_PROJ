<%@ Page Title="Post Rate" Language="C#" MasterPageFile="../Site.master" AutoEventWireup="true" CodeFile="CalculatePostRate.aspx.cs" Inherits="WebPages_CalculatePostRate" %>

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
            <h3><asp:Label ID="lblPostTitle" runat="server" Text=""></asp:Label>Post Rate</h3>
            <div id="localPostCalFrm" runat="server" class="gh_postCal_div">
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
            <div id="airPostCalFrm" runat="server" class="gh_postCal_div">
                <asp:Label ID="lblErrorAir" runat="server" Text=""></asp:Label>
                <p>1. GST is not applicable for international mail rates.<br />
                    2. Printed paper include annual reports, books, catalogues, direct mail, newspaper, periodicals.<br />
                    3. Small Packets are mail containing goods or merchandise that are up to 2kg in weight. The largest dimension should not exceed 600mm, with length, width and height combined not exceeding 900mm.<br />
                    4. Letter rate applies if the postcard exceeds the maximum dimensions of 120mm x 235mm (for overseas posting only).
                </p>
                <table class="gh_table">
                    <tr>
                        <td>Destination</td>
                        <td>: <asp:DropDownList ID="ddlCountryAir" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCountryAir_SelectedIndexChanged"></asp:DropDownList></td>
                        <td><asp:Label ID="lblZoneAir" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Airmail Type</td>
                        <td>: <asp:DropDownList ID="ddlAirMailType" runat="server">
                            <asp:ListItem Value="empty">--Select Type--</asp:ListItem>
                            <asp:ListItem Value="papers">Letters and Printed Papers</asp:ListItem>
                            <asp:ListItem Value="packets">Small Packets</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Airmail Weight (g)</td>
                        <td>: <asp:TextBox ID="txtAirMailWeight" runat="server" TextMode="Number"></asp:TextBox></td>
                        <td>(Max weight: 2kg)</td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnCalAir" runat="server" Text="Calculate" OnClick="btnCalAir_Click" /></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div id="surfacePostCalFrm" runat="server" class="gh_postCal_div">
                <p>1. GST not applicable for international mail rates.<br />
                    2. Printed papers include annual reports, books, catalogues, direct mail, newspapers or periodicals.<br />
                    3. Letter rate applies if the postcard exceeds the maximum dimensions of 120mm x 235mm (for overseas posting only).<br /><br />
                </p>
                <p>*Only Airmail rates are available for mail to Malaysia & Brunei, which will be delivered via the fastest mode of transport*</p>
                <table class="gh_table">
                    <tr>
                        <td>Destination</td>
                        <td>: <asp:DropDownList ID="ddlCountrySurf" runat="server"></asp:DropDownList></td>
                        <td><asp:Label ID="lblSurf" runat="server" Text="(Countries excluded: Malaysia, Brunai)" style="color:red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Surface Mail Type</td>
                        <td>: <asp:DropDownList ID="ddlMailTypeSurface" runat="server" AutoPostBack="True" Enabled="false">
                            <asp:ListItem Value="papers">Letters and Printed Papers</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Surface mail Weight (g)</td>
                        <td>: <asp:TextBox ID="txtSurfaceMailWeight" runat="server" TextMode="Number"></asp:TextBox></td>
                        <td>(Max weight: 2kg)</td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnSurfaceMailCal" runat="server" Text="Calculate" OnClick="btnSurfaceMailCal_Click" /></td>
                    </tr>
                </table>
            </div>
            <div id="bulkPostCalFrm" runat="server" class="gh_postCal_div">
                <p>
                    1. Only printed papers, books, catalogues are accepted for posting under this service.<br />
                    2. Letter, packets of goods and merchandise are not acceptable.<br />
                    3. M Bag cannot be registered or insured.<br />
                    4. Every M Bag must be accompanied with a M Bag label and Customs Declaration forms CN22 or CN23.<br />
                    5. Each Bag must be addressed to only one addressee as listed in the M Bag label<br />
                    6. Items enclosed in the M Bags must be packed in carton or box.<br />
                    7. The addressee’s and sender’s addresses must be shown on each packet in the M Bag and they shall be sent to the same addressee as listed on the M Bag label.<br />
                    8. Service is only available to countries where SingPost has direct Bag dispatch<br />
                    9. M Bags and labels are available at Bulk Mail Centre.<br />
                    10. Delivery of M Bag will depend on the mode accorded by the country of destination.<br />
                    11. Posting is only available at the SingPost Bulk Mail Centre at the following address, and payment of charges shall be in cash or NETS.<br /><br />
                    <i>Bulk Mail Centre, 10 Eunos Road 5, East Entrance, Singapore Post Centre, Singapore 408600</i>
                </p>
                <table class="gh_table">
                    <tr>
                        <td>Transport Mode</td>
                        <td>: <asp:DropDownList ID="ddlBulkTransport" runat="server" OnSelectedIndexChanged="ddlBulkTransport_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="empty">--Select Transport Mode--</asp:ListItem>
                            <asp:ListItem Value="air">Air</asp:ListItem>
                            <asp:ListItem Value="surface">Surface</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Destination</td>
                        <td>: <asp:DropDownList ID="ddlBulkCountry" runat="server" OnSelectedIndexChanged="ddlBulkCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
                        <td><asp:Label ID="lblBulkZone" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>&nbsp&nbsp<asp:Label ID="lblBulkExcluded" runat="server" Text="" style="color:red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Bulk mail Weight (kg)</td>
                        <td>: <asp:TextBox ID="txtBulkWeight" runat="server" TextMode="Number"></asp:TextBox></td>
                        <td>(Max weight: 30kg)</td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnBulkMailCal" runat="server" Text="Calculate" OnClick="btnBulkMailCal_Click"/></td>
                    </tr>
                </table>
            </div>
            <br />
            <asp:Label ID="lblTotalPrice" runat="server" Text=""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

