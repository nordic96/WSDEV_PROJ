using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using TestWS;

public partial class _Default : Page
{
    FacilityWS fws = new FacilityWS();
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if(!Page.IsPostBack)
        {

            this.PopulateRSSFeed("RssFeedUrlCargo");
            string[] newsTopicList = System.Web.Configuration.WebConfigurationManager.AppSettings["RssFeedMenuList"].Split(',');
            ddlNewsTopic.DataSource = newsTopicList;
            ddlNewsTopic.DataBind();
            gvBankInfo.DataSource = fws.showEquipment();
            gvBankInfo.DataBind();

        }
    }

    //Populating RSS Feed
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
}