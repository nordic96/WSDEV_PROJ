using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class About : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadOneMapData data = new LoadOneMapData();
        if(!Page.IsPostBack)
        {
            gvRoute.DataSource = data.load_data_result("Temasek Polytechnic", 1).results;
            gvRoute.DataBind();
            gvRoute.HeaderRow.Cells[0].Text = "Address";
            gvRoute.HeaderRow.Cells[1].Text = "Block";
            gvRoute.HeaderRow.Cells[2].Text = "Road";
            gvRoute.HeaderRow.Cells[3].Text = "Building";
            gvRoute.HeaderRow.Cells[4].Text = "Address Details";
            gvRoute.HeaderRow.Cells[5].Text = "Postal Code";
        }
    }
}