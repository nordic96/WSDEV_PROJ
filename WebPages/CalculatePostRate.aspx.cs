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
        }
        else if (selected.Equals("air"))
        {
            airPostCalFrm.Style.Add("display", "block");

            surfacePostCalFrm.Style.Add("display", "none");
            bulkPostCalFrm.Style.Add("display", "none");
            localPostCalFrm.Style.Add("display", "none");
        }
        else if (selected.Equals("surface"))
        {
            surfacePostCalFrm.Style.Add("display", "block");

            bulkPostCalFrm.Style.Add("display", "none");
            localPostCalFrm.Style.Add("display", "none");
            airPostCalFrm.Style.Add("display", "none");
        }
        else if (selected.Equals("bulk"))
        {
            bulkPostCalFrm.Style.Add("display", "block");

            localPostCalFrm.Style.Add("display", "none");
            airPostCalFrm.Style.Add("display", "none");
            surfacePostCalFrm.Style.Add("display", "none");
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
            string zone_area = get_zone_area(ddlBulkCountry.SelectedValue);

            p = calculateBulkMail(transport_mode, zone_area, weight);
            result = p.result;
            if (result == false)
            {
                popup_error();
                txtBulkWeight.Text = "";
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
    
    //To be implemented into ASMX
    private PostalPrice calculateBulkMail(string transport_mode, string zone_area, double weight)
    {
        PostalPrice p = new PostalPrice();
        bool result = true;
        List<ZoneRateList> zone_rate_lists = new List<ZoneRateList>();

        double totalRate = 0, first5Kg = 0, addlKg = 0;

        string[] zone_A_rate = "16;3;NA;NA".Split(';');
        string[] zone_B_rate = "30;5;18;2".Split(';');
        string[] zone_C_rate = "30;5;18;2".Split(';');
        string[] zone_R_rate = "40;7;20;2".Split(';');
        string[] zone_S_rate = "50;9;25;2".Split(';');
        string[] zone_T_rate = "50;9;25;2".Split(';');

        ZoneRateList a = new ZoneRateList("A", zone_A_rate);
        ZoneRateList b = new ZoneRateList("B", zone_B_rate);
        ZoneRateList c = new ZoneRateList("C", zone_C_rate);
        ZoneRateList r = new ZoneRateList("R", zone_R_rate);
        ZoneRateList s = new ZoneRateList("S", zone_S_rate);
        ZoneRateList t = new ZoneRateList("T", zone_T_rate);

        zone_rate_lists.Add(a);
        zone_rate_lists.Add(b);
        zone_rate_lists.Add(c);
        zone_rate_lists.Add(r);
        zone_rate_lists.Add(s);
        zone_rate_lists.Add(t);

        foreach (ZoneRateList list in zone_rate_lists)
        {
            if(list.zone_name.Equals(zone_area))
            {
                if (transport_mode.Equals("air"))
                {
                    first5Kg = Convert.ToDouble(list.zone_rate[0]);
                    addlKg = Convert.ToDouble(list.zone_rate[1]);
                }
                else if (transport_mode.Equals("surface"))
                {
                    if (zone_area.Equals("A"))
                    {
                        result = false;
                    }

                    else
                    {
                        first5Kg = Convert.ToDouble(list.zone_rate[2]);
                        addlKg = Convert.ToDouble(list.zone_rate[3]);
                    }
                }
                else
                    result = false;
            }
        }

        //Calculation
        if (0 < weight && weight <= 5)
        {
            totalRate = first5Kg;
        }
        else if (weight > 5 && weight <= 30)
        {
            totalRate = first5Kg + (Math.Ceiling(weight - 5) * addlKg);
        }
        else
            result = false;

        p.price = totalRate;
        p.result = result;
        return p;
    }
}