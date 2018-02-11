using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoWS;
//using BusWS;
using BusWSJM;
using System.Xml;

public partial class About : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BusWebService bws = new BusWebService();
        GoThereSGWS service = new GoThereSGWS();

        LoadOneMapData data = new LoadOneMapData();
        LoadBus Loadbus = new LoadBus();
        Bus bus = new Bus();
        if (!Page.IsPostBack)
        {
            
            gvRoute.DataSource = data.load_data_result("Temasek Polytechnic", 1).results;
            gvRoute.DataBind();
            gvRoute.HeaderRow.Cells[0].Text = "Address";
            gvRoute.HeaderRow.Cells[1].Text = "Block";
            gvRoute.HeaderRow.Cells[2].Text = "Road";
            gvRoute.HeaderRow.Cells[3].Text = "Building";
            gvRoute.HeaderRow.Cells[4].Text = "Address Details";
            gvRoute.HeaderRow.Cells[5].Text = "Postal Code";


            //gvTest.DataSource = Loadbus.loadBusDataResult("75239", "").bus_results;
            //gvTest.DataBind();
            //gvTest.HeaderRow.Cells[0].Text = "NextBus";
            //gvTest.HeaderRow.Cells[1].Text = "NextBus2";
            //gvTest.HeaderRow.Cells[2].Text = "NextBus3";


            string busArrivalInfo = bws.GetBusArrivalsInformation("75239" , "").ToString();
            XmlNodeList serviceNo = extractData(busArrivalInfo, "ServiceNo");
            XmlNodeList Operator = extractData(busArrivalInfo, "Operator");

        }
    }

    protected XmlNodeList extractData(string stringXML, string stringElementName)
    {
        XmlDocument xd = new XmlDocument();
        xd.LoadXml(stringXML);
        return xd.GetElementsByTagName(stringElementName);
    }
}