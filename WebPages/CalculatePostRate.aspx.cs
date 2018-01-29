using System;
using CountryService;
using System.Windows.Forms;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class WebPages_CalculatePostRate : System.Web.UI.Page
{
    country cis = new country();
    LogisticsService logistics = new LogisticsService(); //WS Proxy
    string getCountryJSONUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["GetCountryListUrl"];

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            //Initial loading
            loadCountryList(getCountryJSONUrl);

            lblPostTitle.Text = ddlPostCalCategory.SelectedItem.Text + " ";
            localPostCalFrm.Style.Add("display", "block");
        }
    }

    protected void ddlPostCalCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selected = ddlPostCalCategory.SelectedValue;
        lblPostTitle.Text = ddlPostCalCategory.SelectedItem.Text + " ";
        if (selected.Equals("local"))
        {
            localPostCalFrm.Style.Add("display", "block");

            airPostCalFrm.Style.Add("display", "none");
            surfacePostCalFrm.Style.Add("display", "none");
            bulkPostCalFrm.Style.Add("display", "none");
            lblTotalPrice.Text = "";
        }
        else if (selected.Equals("air"))
        {
            airPostCalFrm.Style.Add("display", "block");

            surfacePostCalFrm.Style.Add("display", "none");
            bulkPostCalFrm.Style.Add("display", "none");
            localPostCalFrm.Style.Add("display", "none");
            lblTotalPrice.Text = "";
        }
        else if (selected.Equals("surface"))
        {
            surfacePostCalFrm.Style.Add("display", "block");

            bulkPostCalFrm.Style.Add("display", "none");
            localPostCalFrm.Style.Add("display", "none");
            airPostCalFrm.Style.Add("display", "none");
            lblTotalPrice.Text = "";
        }
        else if (selected.Equals("bulk"))
        {
            bulkPostCalFrm.Style.Add("display", "block");

            localPostCalFrm.Style.Add("display", "none");
            airPostCalFrm.Style.Add("display", "none");
            surfacePostCalFrm.Style.Add("display", "none");
            lblTotalPrice.Text = "";
        }
    }

    protected void ddlPostSizeLocal_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Do nothing
    }

    protected void btnCalLocal_Click(object sender, EventArgs e)
    {
        PostalPrice p = new PostalPrice();
        if (!ddlPostSizeLocal.SelectedValue.Equals("empty") && !txtPostWeightLocal.Text.Equals(""))
        {
            string select = ddlPostSizeLocal.SelectedValue;
            double weight = Convert.ToDouble(txtPostWeightLocal.Text);
            if(weight < 0 || weight > 2000)
            {
                MessageBox.Show("Invalid weight amount", "Weight Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                lblTotalPrice.Text = "";
            }
            else if (weight <= 2000 && weight > 0)
            {
                p = logistics.CalculatePostRateLocal(select, weight);
                if(p.result == false)
                {
                    MessageBox.Show("Overweight error. Please choose different post size", "Overweight Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    lblTotalPrice.Text = "";
                    txtPostWeightLocal.Text = "";
                }
                else if(p.result == true)
                {
                    if (p.price != 0)
                        lblTotalPrice.Text = "Total post rate: S$" + p.price.ToString("f2");
                    else
                        lblTotalPrice.Text = "";
                }
            }

        }
        else if (ddlPostSizeLocal.SelectedValue.Equals("empty") || txtPostWeightLocal.Text.Equals(""))
        {
            //Display Error message to choose one of the size
            popup_empty();
        }
    }

    //For Airmail Calculation
    protected void btnCalAir_Click(object sender, EventArgs e)
    {
        if(ddlAirMailType.SelectedValue.Equals("empty") || txtAirMailWeight.Text.Equals(""))
            popup_empty();
        else
        {
            PostalPrice p = new PostalPrice();
            string countrySelect = ddlCountryAir.SelectedValue;
            string mailType = ddlAirMailType.SelectedValue;
            double weight = Convert.ToDouble(txtAirMailWeight.Text);

            p = logistics.CalculatePostRateAir(countrySelect, mailType, weight);
            if (p.result.Equals(true))
                lblTotalPrice.Text = "Total post rate: S$" + p.price.ToString("f2");
            else
            {
                popup_error();
                lblTotalPrice.Text = "";
                txtAirMailWeight.Text = "";
            }
        }
    }

    protected void ddlCountryAir_SelectedIndexChanged(object sender, EventArgs e)
    {
        string select;
        select = ddlCountryAir.SelectedValue;

        lblZoneAir.Text = "Zone " + get_zone_no(select).ToString();
    }

    protected void btnSurfaceMailCal_Click(object sender, EventArgs e)
    {
        if (txtSurfaceMailWeight.Text.Equals(""))
            popup_empty();

        else
        {
            PostalPrice p = new PostalPrice();
            string countrySelect = ddlCountrySurf.SelectedValue;
            string mailType = ddlMailTypeSurface.SelectedValue;
            double weight = Convert.ToDouble(txtSurfaceMailWeight.Text);

            p = logistics.CalculatePostRateSurface(mailType, weight);
            if (p.result.Equals(true))
                lblTotalPrice.Text = "Total post rate: S$" + p.price.ToString("f2");
            else
            {
                popup_error();
                lblTotalPrice.Text = "";
                txtSurfaceMailWeight.Text = "";
            }
        }
    }

    protected void ddlBulkTransport_SelectedIndexChanged(object sender, EventArgs e)
    {
        string select = ddlBulkTransport.SelectedValue.ToString();

        if (select.Equals("air"))
        {
            lblBulkExcluded.Text = "";
            loadCountryList(getCountryJSONUrl);
        }

        else if (select.Equals("surface"))
        {
            ddlBulkCountry.SelectedValue = "AF"; //Initializing

            //Excluding Malaysia & Brunai from ddl
            ListItem ml = ddlBulkCountry.Items.FindByValue("MY"),
                bn = ddlBulkCountry.Items.FindByValue("BN");
            if (ml != null && bn != null)
            {
                ddlBulkCountry.Items.Remove(ml);
                ddlBulkCountry.Items.Remove(bn);
            }
            lblBulkExcluded.Text = "Excluded Country: Malaysia, Brunai";
        }
    }

    protected void btnBulkMailCal_Click(object sender, EventArgs e)
    {
        if(ddlBulkTransport.SelectedValue.Equals("empty") || txtBulkWeight.Text.Equals(""))
        {
            popup_empty();
        }
        else if(!ddlBulkTransport.SelectedValue.Equals("empty") && !txtBulkWeight.Text.Equals(""))
        {
            PostalPrice p = new PostalPrice();
            bool result;
            double weight = Convert.ToDouble(txtBulkWeight.Text);
            string transport_mode = ddlBulkTransport.SelectedValue;
            string destination = ddlBulkCountry.SelectedValue;

            p = logistics.CalculatePostRateBulk(transport_mode, destination, weight);
            result = p.result;
            if (result == false)
            {
                popup_error();
                txtBulkWeight.Text = "";
                lblTotalPrice.Text = "";
            }
            else if (result == true)
            {
                lblTotalPrice.Text = "Total post rate: S$" + p.price.ToString("f2");
            }
        }
    }

    protected void ddlBulkCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        string zone_area = get_zone_area(ddlBulkCountry.SelectedValue);
        lblBulkZone.Text = "Zone : " + zone_area;
    }

    //GetCountryList (External REST WS)
    private void loadCountryList(string url)
    {
        CountryInfo[] countries;
        try
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            var webResponse = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                string resString = sr.ReadToEnd();

                CountryInfoList list = json.Deserialize<CountryInfoList>(resString);
                countries = list.RestResponse.result;

                ddlCountryAir.DataSource = countries;
                ddlCountryAir.DataTextField = "name";
                ddlCountryAir.DataValueField = "alpha2_code";
                ddlCountryAir.DataBind();

                ddlCountrySurf.DataSource = countries;
                ddlCountrySurf.DataTextField = "name";
                ddlCountrySurf.DataValueField = "alpha2_code";
                ddlCountrySurf.DataBind();

                ddlBulkCountry.DataSource = countries;
                ddlBulkCountry.DataTextField = "name";
                ddlBulkCountry.DataValueField = "alpha2_code";
                ddlBulkCountry.DataBind();

                //Excluding Malaysia & Brunai from ddl
                ListItem ml = ddlCountrySurf.Items.FindByValue("MY"),
                    bn = ddlCountrySurf.Items.FindByValue("BN");
                if (ml != null && bn != null)
                {
                    ddlCountrySurf.Items.Remove(ml);
                    ddlCountrySurf.Items.Remove(bn);
                }
            }
        }
        catch (Exception e)
        {
            lblErrorAir.Text = e.ToString();
        }
    }

    private void popup_error()
    {
        MessageBox.Show("Invalid weight amount", "Weight Error",
    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
    }
    private void popup_empty()
    {
        MessageBox.Show("Please choose an option", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, 
            MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
    }

    private int get_zone_no(string select)
    {
        string[] zone1 = System.Web.Configuration.WebConfigurationManager.AppSettings["AirMail_Zone1"].Split(';'),
zone2 = System.Web.Configuration.WebConfigurationManager.AppSettings["AirMail_Zone2"].Split(';');

        int zoneNumber = 3;

        for (int i = 0; i < zone1.Length; i++)
        {
            if (select.Equals(zone1[i]))
                zoneNumber = 1;
        }
        for (int i = 0; i < zone2.Length; i++)
        {
            if (select.Equals(zone2[i]))
                zoneNumber = 2;
        }

        return zoneNumber;
    }

    //To be implemented into ASMX
    private string get_zone_area(string select)
    {
        string zone_area = "";

        string[] zone_A = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_A"].Split(';');
        string[] zone_B = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_B"].Split(';');
        string[] zone_C = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_C"].Split(';');
        string[] zone_R = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_R"].Split(';');
        string[] zone_S = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_S"].Split(';');
        string[] zone_T = System.Web.Configuration.WebConfigurationManager.AppSettings["Bulk_Zone_T"].Split(';');

        ZoneList a = new ZoneList("A", zone_A);
        ZoneList b = new ZoneList("B", zone_B);
        ZoneList c = new ZoneList("C", zone_C);
        ZoneList r = new ZoneList("R", zone_R);
        ZoneList s = new ZoneList("S", zone_S);
        ZoneList t = new ZoneList("T", zone_T);

        List<ZoneList> zone_lists = new List<ZoneList>();
        zone_lists.Add(a);
        zone_lists.Add(b);
        zone_lists.Add(c);
        zone_lists.Add(r);
        zone_lists.Add(s);
        zone_lists.Add(t);

        foreach (ZoneList zone_list in zone_lists)
        {
            for(int i=0; i < zone_list.countries.Length; i++)
            {
                if(zone_list.countries[i].Equals(select))
                {
                    zone_area = zone_list.zone_name;
                }
            }
        }
        return zone_area;
    }
}