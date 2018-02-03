using System;
using System.Collections.Generic;
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

public partial class WebPages_Freight : System.Web.UI.Page
{
    //ExcelRead excel = new ExcelRead();
    DataTable ds = new DataTable("result");
    LogisticsService cis = new LogisticsService();

    OneMapData data = new OneMapData();
    OneMapData_Result[] results = null;
    string searchVal = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            btnNextPage.Visible = false;
            btnPrevPage.Visible = false;

            //ds = cis.GetAllDutiableTaxHSCode();
            //gvTest.DataSource = ds;
            //gvTest.DataBind();
            ddlCompanyFFC.Visible = false;
            ddlCountryFFC.Visible = false;
            DataTable ffc = cis.GetAllFreightForwarderContacts();
            ddlCompanyFFC.DataSource = ffc;
            ddlCompanyFFC.DataTextField = ffc.Columns[1].ToString();
            ddlCompanyFFC.DataValueField = ffc.Columns[1].ToString();
            ddlCompanyFFC.DataBind();
            ddlCountryFFC.DataSource = ffc;
            ddlCountryFFC.DataTextField = ffc.Columns[0].ToString();
            ddlCountryFFC.DataValueField = ffc.Columns[0].ToString();
            ddlCountryFFC.DataBind();
            gvFreightForwarderContact.DataSource = ffc;
            gvFreightForwarderContact.DataBind();
            for (int i = 0; i < ddlCountryFFC.Items.Count; i++)
            {
                ddlCountryFFC.SelectedIndex = i;
                string str = ddlCountryFFC.SelectedItem.ToString();
                for (int counter = i + 1; counter < ddlCountryFFC.Items.Count; counter++)
                {
                    ddlCountryFFC.SelectedIndex = counter;
                    string compareStr = ddlCountryFFC.SelectedItem.ToString();
                    if (str == compareStr)
                    {
                        ddlCountryFFC.Items.RemoveAt(counter);
                        counter = counter - 1;
                    }
                }
            }

        }
    }

    private OneMapData load_data_result(string searchVal, int page_index)
    {
        IList<OneMapData_Result[]> result_list = new List<OneMapData_Result[]>();
        string url = System.Web.Configuration.WebConfigurationManager.AppSettings["OneMapAPIUrl"] + 
            "?searchVal=" + searchVal + "&returnGeom=N&getAddrDetails=Y&pageNum=" + page_index.ToString();

        try
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            var webResponse = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                string resString = sr.ReadToEnd();

                data = json.Deserialize<OneMapData>(resString);
                results = data.results;
            }
        }
        catch (Exception ex)
        {
            lblTest.Text = ex.ToString();
        }

        //Validating empty search result.

        gvOneAddress.DataSource = results;
        gvOneAddress.DataBind();
        gvOneAddress.HeaderRow.Cells[0].Text = "Address Name";
        gvOneAddress.HeaderRow.Cells[1].Text = "BLK";
        gvOneAddress.HeaderRow.Cells[2].Text = "Road Name";
        gvOneAddress.HeaderRow.Cells[3].Text = "Building";
        gvOneAddress.HeaderRow.Cells[4].Text = "Address Details";
        gvOneAddress.HeaderRow.Cells[5].Text = "Postal Code";
        return data;
    }

    protected void btnNextPage_Click(object sender, EventArgs e)
    {
        string value = (string)Session["searchval"];
        int counter = (int)Session["AttemptCount"];
        int totalPage = (int)Session["TotalPage"];

        if(0 < counter && counter < totalPage)
        {
            counter++;
            Session["AttemptCount"] = counter;
            data = load_data_result(value, counter);
            lblTest.Text = "Current Page : " + counter + " Total Page: " + totalPage;
        }
        else
        {
            popup_error("page limit exceeded.");
        }
    }

    protected void btnPrevPage_Click(object sender, EventArgs e)
    {
        string value = (string)Session["searchval"];
        int counter = (int)Session["AttemptCount"];
        int totalPage = (int)Session["TotalPage"];

        if (1 < counter && counter <= totalPage)
        {
            counter--;
            Session["AttemptCount"] = counter;
            data = load_data_result(value, counter);
            lblTest.Text = "Current Page : " + counter + " Total Page: " + totalPage;
        }
        else
        {
            popup_error("page limit exceeded.");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        searchVal = txtSearchAddress.Text;
        if (searchVal.Equals(""))
        {
            popup_error("Please enter any value into the textbox.");
        }
        else
        {
            btnNextPage.Visible = true;
            btnPrevPage.Visible = true;

            data = load_data_result(searchVal, 1);

            //Store input data and counter as Session values.
            Session["searchval"] = txtSearchAddress.Text;
            Session["AttemptCount"] = 1;
            Session["TotalPage"] = data.totalNumPages;

            int totalPage = (int)Session["TotalPage"];
            int counter = (int)Session["AttemptCount"];

            lblTest.Text = "Current Page : " + counter + " Total Page: " + totalPage;
        }
    }

    private void popup_error(string message)
    {
        MessageBox.Show(message, "Warning",
    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
    }


    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(RadioButtonList1.SelectedValue == "Country")
        {
            ddlCountryFFC.Visible = true;
            ddlCompanyFFC.Visible = false;
        }
        else if(RadioButtonList1.SelectedValue == "Company Name")
        {
            ddlCompanyFFC.Visible = true;
            ddlCountryFFC.Visible = false;
        }
    }

    protected void btnSearchFFC_Click(object sender, EventArgs e)
    {
        if(RadioButtonList1.SelectedValue == "Country")
        {
            DataTable dt = new DataTable();
            string searchBy = RadioButtonList1.SelectedValue.ToString();
            string searchText = ddlCountryFFC.SelectedValue.ToString();
            dt = cis.SearchFreightForwarders(searchBy, searchText);
            gvFreightForwarderContact.DataSource = dt;
            gvFreightForwarderContact.DataBind();
        }
        else if(RadioButtonList1.SelectedValue == "Company Name")
        {
            DataTable dt = new DataTable();
            string searchBy = RadioButtonList1.SelectedValue.ToString();
            string searchText = ddlCompanyFFC.SelectedValue.ToString();
            dt = cis.SearchFreightForwarders(searchBy, searchText);
            gvFreightForwarderContact.DataSource = dt;
            gvFreightForwarderContact.DataBind();
        }

    }

}