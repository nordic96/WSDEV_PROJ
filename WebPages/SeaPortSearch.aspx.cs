using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Logistics;

public partial class WebPages_SeaPortSearch : System.Web.UI.Page
{
    LogisticsService cis = new LogisticsService();
    DataSet portExcelDs = null;
    //ExcelRead excel = new ExcelRead();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            portExcelDs = cis.GetAllSeaPortInformation();
            loadDropDownList();
            gvSeaPort.DataSource = portExcelDs;
            gvSeaPort.DataBind();
        }
    }

    protected void ddlSearchPort_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSearchPort_Click(object sender, EventArgs e)
    {
        string searchBy = ddlSearchPort.SelectedValue, searchText = txtSearchPort.Text;
        portExcelDs = cis.SearchSeaPortInformation(searchBy, searchText);

        gvSeaPort.DataSource = portExcelDs;
        gvSeaPort.DataBind();
    }

    //Load port columns into the  drop down list
    private void loadDropDownList()
    {
        foreach (DataColumn column in portExcelDs.Tables[0].Columns)
        {
            ddlSearchPort.Items.Add(new ListItem(column.ColumnName));
        }
    }
}