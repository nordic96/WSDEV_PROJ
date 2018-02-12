using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Xml.Linq;
using System.Xml.Serialization;
using TestWS;
using HealthWS;
using System.Xml;

public partial class _Default : Page
{
    FacilityWS fws = new FacilityWS();
    HealthcareWS hcws = new HealthcareWS();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            //For RSS
            this.PopulateRSSFeed("RssFeedUrlCargo");
            string[] newsTopicList = System.Web.Configuration.WebConfigurationManager.AppSettings["RssFeedMenuList"].Split(',');
            ddlNewsTopic.DataSource = newsTopicList;
            ddlNewsTopic.DataBind();


            //For Adding Gym Information (Drop Down List)
            List<string> gymAreaList = new List<string>();
            GymAreas[] gymAreas = hcws.GetGymAreas();
            foreach(GymAreas g in gymAreas)
            {
                gymAreaList.Add(g.area.ToString());
            }

            ddlGymArea.DataSource = gymAreaList;
            ddlGymArea.DataBind();

            //For Adding Gym Information Gridview Initializing
            changeGymGridView(ddlGymArea.SelectedItem.ToString());

            ddlPolyclinicsArea.DataSource = hcws.GetPolyclinicArea();
            ddlPolyclinicsArea.DataTextField = "area";
            ddlPolyclinicsArea.DataValueField = "area";
            ddlPolyclinicsArea.DataBind();

            HealthWS.Polyclinics[] polyclinics = initializePoliclinicsCentral();
            List<Polyclinic> polycliniclist = new List<Polyclinic>();
            foreach (HealthWS.Polyclinics polyclinic in polyclinics)
            {
                Polyclinic b = new Polyclinic();
                b.name = polyclinic.name;
                b.address = polyclinic.address;
                b.telNumber = polyclinic.telNumber;

                polycliniclist.Add(b);
            }
            gvPolyclinics.DataSource = polycliniclist;
            gvPolyclinics.DataBind();

            gvPolyclinics.HeaderRow.Cells[0].Text = "Name";
            gvPolyclinics.HeaderRow.Cells[1].Text = "Address";
            gvPolyclinics.HeaderRow.Cells[2].Text = "Telephone No.";

        }


    }

    /*
     * Populating RSS Feed
     * Gihun Ko 10/1/2018
     */
    private void PopulateRSSFeed(string urlConfig)
    {
        string rssFeedUrl = ConfigurationManager.AppSettings[urlConfig];
        List<Feeds> feeds = new List<Feeds>();
        XDocument xDoc = XDocument.Load(rssFeedUrl);
        var items = (from x in xDoc.Descendants("item")
                     select new
                     {
                         title = x.Element("title").Value,
                         link = x.Element("link").Value,
                         pubDate = x.Element("pubDate").Value,
                         desc = x.Element("description").Value
                     });
        if(items != null)
        {
            feeds.AddRange(items.Select(i => new Feeds
            {
                Title = i.title, Link = i.link, PublishDate = i.pubDate, Description = i.desc
            }));
        }
        gvRSS.DataSource = feeds;
        gvRSS.DataBind();
        //Setting the label text
        lblRssFeedSubject.Text = urlConfig.Substring(10) + " ";
    }

    protected void ddlNewsTopic_SelectedIndexChanged(object sender, EventArgs e)
    {
        string rssFeedKey = "";
        rssFeedKey = "RssFeedUrl" + ddlNewsTopic.SelectedValue;
        PopulateRSSFeed(rssFeedKey);
    }

    /*
     * Changing the GymGridView's DataSource based on the selected Area in SG 
     * Using Nicholas' WS
     * Gihun Ko 12/2/2018
    */
    private void changeGymGridView(string area)
    {
        HealthcareWS h = new HealthcareWS();
        GymAll[] gyms = null;

        if (area.Equals("North"))
        {
            gyms = h.GetGymAllNorth();
        }
        else if (area.Equals("East"))
        {
            gyms = h.GetGymAllEast();
        }
        else if (area.Equals("West"))
        {
            gyms = h.GetGymAllWest();
        }
        else if (area.Equals("Central"))
        {
            gyms = h.GetGymAllCentral();
        }
        else if (area.Equals("North-East"))
        {
            gyms = h.GetGymAllNorthEast();
        }

        gvGym.DataSource = gyms;
        gvGym.DataBind();
    }

    protected void ddlGymArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        changeGymGridView(ddlGymArea.SelectedItem.ToString());
    }

    private HealthWS.Polyclinics[] initializePoliclinicsCentral()
    {
        HealthcareWS hc = new HealthcareWS();
        HealthWS.Polyclinics[] result = hc.GetPolyclinicCentral();
        return result;
    }

    private HealthWS.Polyclinics[] initializePoliclinicsNorth()
    {
        HealthcareWS hc = new HealthcareWS();
        HealthWS.Polyclinics[] result = hc.GetPolyclinicNorth();
        return result;
    }

    private HealthWS.Polyclinics[] initializePoliclinicsEast()
    {
        HealthcareWS hc = new HealthcareWS();
        HealthWS.Polyclinics[] result = hc.GetPolyclinicEast();
        return result;
    }

    private HealthWS.Polyclinics[] initializePoliclinicsNE()
    {
        HealthcareWS hc = new HealthcareWS();
        HealthWS.Polyclinics[] result = hc.GetPolyclinicNorthEast();
        return result;
    }

    private HealthWS.Polyclinics[] initializePoliclinicsWest()
    {
        HealthcareWS hc = new HealthcareWS();
        HealthWS.Polyclinics[] result = hc.GetPolyclinicWest();
        return result;
    }

    protected void ddlPolyclinicsArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlPolyclinicsArea.SelectedValue == "Central")
        {
            HealthWS.Polyclinics[] polyclinics = initializePoliclinicsCentral();
            List<Polyclinic> polycliniclist = new List<Polyclinic>();
            foreach (HealthWS.Polyclinics polyclinic in polyclinics)
            {
                Polyclinic b = new Polyclinic();
                b.name = polyclinic.name;
                b.address = polyclinic.address;
                b.telNumber = polyclinic.telNumber;

                polycliniclist.Add(b);
            }
            gvPolyclinics.DataSource = polycliniclist;
            gvPolyclinics.DataBind();

            gvPolyclinics.HeaderRow.Cells[0].Text = "Name";
            gvPolyclinics.HeaderRow.Cells[1].Text = "Address";
            gvPolyclinics.HeaderRow.Cells[2].Text = "Telephone No.";
        }

        else if(ddlPolyclinicsArea.SelectedValue == "North")
        {
            HealthWS.Polyclinics[] polyclinics = initializePoliclinicsNorth();
            List<Polyclinic> polycliniclist = new List<Polyclinic>();
            foreach (HealthWS.Polyclinics polyclinic in polyclinics)
            {
                Polyclinic b = new Polyclinic();
                b.name = polyclinic.name;
                b.address = polyclinic.address;
                b.telNumber = polyclinic.telNumber;

                polycliniclist.Add(b);
            }
            gvPolyclinics.DataSource = polycliniclist;
            gvPolyclinics.DataBind();

            gvPolyclinics.HeaderRow.Cells[0].Text = "Name";
            gvPolyclinics.HeaderRow.Cells[1].Text = "Address";
            gvPolyclinics.HeaderRow.Cells[2].Text = "Telephone No.";
        }

        else if(ddlPolyclinicsArea.SelectedValue == "East")
        {
            HealthWS.Polyclinics[] polyclinics = initializePoliclinicsEast();
            List<Polyclinic> polycliniclist = new List<Polyclinic>();
            foreach (HealthWS.Polyclinics polyclinic in polyclinics)
            {
                Polyclinic b = new Polyclinic();
                b.name = polyclinic.name;
                b.address = polyclinic.address;
                b.telNumber = polyclinic.telNumber;

                polycliniclist.Add(b);
            }
            gvPolyclinics.DataSource = polycliniclist;
            gvPolyclinics.DataBind();

            gvPolyclinics.HeaderRow.Cells[0].Text = "Name";
            gvPolyclinics.HeaderRow.Cells[1].Text = "Address";
            gvPolyclinics.HeaderRow.Cells[2].Text = "Telephone No.";
        }

        else if(ddlPolyclinicsArea.SelectedValue == "North-East")
        {
            HealthWS.Polyclinics[] polyclinics = initializePoliclinicsNE();
            List<Polyclinic> polycliniclist = new List<Polyclinic>();
            foreach (HealthWS.Polyclinics polyclinic in polyclinics)
            {
                Polyclinic b = new Polyclinic();
                b.name = polyclinic.name;
                b.address = polyclinic.address;
                b.telNumber = polyclinic.telNumber;

                polycliniclist.Add(b);
            }
            gvPolyclinics.DataSource = polycliniclist;
            gvPolyclinics.DataBind();

            gvPolyclinics.HeaderRow.Cells[0].Text = "Name";
            gvPolyclinics.HeaderRow.Cells[1].Text = "Address";
            gvPolyclinics.HeaderRow.Cells[2].Text = "Telephone No.";
        }

        else if(ddlPolyclinicsArea.SelectedValue == "West")
        {
            HealthWS.Polyclinics[] polyclinics = initializePoliclinicsWest();
            List<Polyclinic> polycliniclist = new List<Polyclinic>();
            foreach (HealthWS.Polyclinics polyclinic in polyclinics)
            {
                Polyclinic b = new Polyclinic();
                b.name = polyclinic.name;
                b.address = polyclinic.address;
                b.telNumber = polyclinic.telNumber;

                polycliniclist.Add(b);
            }
            gvPolyclinics.DataSource = polycliniclist;
            gvPolyclinics.DataBind();

            gvPolyclinics.HeaderRow.Cells[0].Text = "Name";
            gvPolyclinics.HeaderRow.Cells[1].Text = "Address";
            gvPolyclinics.HeaderRow.Cells[2].Text = "Telephone No.";
        }
    }
}