using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MrtWS;
using System.Xml;
using System.Data;
using System.IO;
using System.Xml.Linq;
using BusWS;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;

public partial class About : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        BusWebService bus = new BusWebService();
        LoadOneMapData data = new LoadOneMapData();
        Root root = initialize();

        List<string> lineNames = new List<string>();

        if (!Page.IsPostBack)
        {
            for(int i=0; i<root.Lines.Length; i++)
            {
                lineNames.Add(root.Lines[i].LineName);
            }
            ddlLineStart.DataSource = lineNames;
            ddlLineStart.DataBind();
            ddlLineEnd.DataSource = lineNames;
            ddlLineEnd.DataBind();
            
            gvRoute.DataSource = data.load_data_result("Temasek Polytechnic", 1).results;
            gvRoute.DataBind();
            gvRoute.HeaderRow.Cells[0].Text = "Address";
            gvRoute.HeaderRow.Cells[1].Text = "Block";
            gvRoute.HeaderRow.Cells[2].Text = "Road";
            gvRoute.HeaderRow.Cells[3].Text = "Building";
            gvRoute.HeaderRow.Cells[4].Text = "Address Details";
            gvRoute.HeaderRow.Cells[5].Text = "Postal Code";

            row[] rowsStart = root.Lines[ddlLineStart.SelectedIndex].rows;
            
            ddlMrtStart.DataSource = rowsStart; 

            ddlMrtStart.DataTextField = "mrt_station_english";
            ddlMrtStart.DataValueField = "stn_code";
            ddlMrtStart.DataBind();

            //Fixed value for Destination MRT
            ddlLineEnd.SelectedValue = "EW";

            row[] rowsEnd = root.Lines[ddlLineEnd.SelectedIndex].rows;
            ddlMrtEnd.DataSource = rowsEnd;
            ddlMrtEnd.DataTextField = "mrt_station_english";
            ddlMrtEnd.DataValueField = "stn_code";
            ddlMrtEnd.DataBind();

            //Fixed Tampines for Destination MRT Station
            ddlMrtEnd.SelectedValue = "EW2";
        }
    }


    protected void btnSearchRoute_Click(object sender, EventArgs e)
    {
        string startMRT = ddlMrtStart.SelectedValue;
        string endMRT = ddlMrtEnd.SelectedValue;

        //lblTest.Text = "START MRT: " + startMRT + "\nEND MRT: " + endMRT;

        GoThereSGWS mrtservice = new GoThereSGWS();
        string[] mrtListToGo = mrtservice.getRoute(startMRT, endMRT);
        gvMrtRoute.DataSource = mrtListToGo;
        gvMrtRoute.DataBind();

        //This is for which line to start
        string[] lineRouteOrder = new string[gvMrtRoute.Rows.Count];

        //For drawing arrow length
        int greenCount = 0, yellowCount = 0, redCount = 0, blueCount = 0, purpleCount = 0;

        DataTable dt = new DataTable();
        dt.Columns.Add("arrow_url");

        List<arrow> arrows = new List<arrow>();

        for (int i=0; i<gvMrtRoute.Rows.Count; i++)
        {
            string lineCheck = gvMrtRoute.Rows[i].Cells[0].Text.Split('(')[1];
            if (lineCheck.StartsWith("EW"))
            {
                greenCount++;
                lineRouteOrder[i] = "EW";
                arrows.Add(new arrow(@"../Content/green_arrow.png"));
            }   
            else if (lineCheck.StartsWith("NS"))
            {
                redCount++;
                lineRouteOrder[i] = "NS";
                arrows.Add(new arrow(@"../Content/red_arrow.png"));
            }
            else if (lineCheck.StartsWith("NE"))
            {
                purpleCount++;
                lineRouteOrder[i] = "NE";
                arrows.Add(new arrow(@"../Content/purple_arrow.png"));
            }
            else if (lineCheck.StartsWith("CC"))
            {
                yellowCount++;
                lineRouteOrder[i] = "CC";
                arrows.Add(new arrow(@"../Content/yellow_arrow.png"));
            }
            else if (lineCheck.StartsWith("DT"))
            {
                blueCount++;
                lineRouteOrder[i] = "DT";
                arrows.Add(new arrow(@"../Content/blue_arrow.png"));
            }
        }
        lblTotalMrtStationsToGo.Text = "Total MRT Stations to travel : " + gvMrtRoute.Rows.Count.ToString();
        gvArrow.DataSource = arrows;
        gvArrow.DataBind();
    }

    protected void ddlLineStart_SelectedIndexChanged(object sender, EventArgs e)
    {
        Root root = initialize();

        int lineIndex = ddlLineStart.SelectedIndex;
        row[] rows = root.Lines[lineIndex].rows;
        ddlMrtStart.DataSource = rows;
        ddlMrtEnd.DataSource = rows;

        ddlMrtStart.DataTextField = "mrt_station_english";
        ddlMrtStart.DataValueField = "stn_code";
        ddlMrtStart.DataBind();

        //Removing Tampines Station from Start MRT Drop down list.
        ddlMrtStart.Items.Remove(ddlMrtStart.Items.FindByText("Tampines"));
    }

    protected void ddlLineEnd_SelectedIndexChanged(object sender, EventArgs e)
    {
        Root root = initialize();

        int lineIndex = ddlLineEnd.SelectedIndex;
        row[] rows = root.Lines[lineIndex].rows;
        ddlMrtEnd.DataSource = rows;
        ddlMrtEnd.DataSource = rows;

        ddlMrtEnd.DataTextField = "mrt_station_english";
        ddlMrtEnd.DataValueField = "stn_code";
        ddlMrtEnd.DataBind();
    }

    public T Deserialize<T>(XDocument doc)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

        using (var reader = doc.Root.CreateReader())
        {
            return (T)xmlSerializer.Deserialize(reader);
        }
    }

    public static XDocument Serialize<T>(T value)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

        XDocument doc = new XDocument();
        using (var writer = doc.CreateWriter())
        {
            xmlSerializer.Serialize(writer, value);
        }

        return doc;
    }

    //Getting the Root object, which is the MRT stations info.
    private Root initialize()
    {
        GoThereSGWS service = new GoThereSGWS();
        XmlNode result = service.getAllMRTStations();
        XDocument xml = XDocument.Parse(result.OuterXml);
        Root root = Deserialize<Root>(xml);

        return root;
    }
}