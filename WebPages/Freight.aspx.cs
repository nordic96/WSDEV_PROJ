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
    OneMapData data = new OneMapData();
    OneMapData_Result[] results = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            Session["AttemptCount"] = 1;
            data = load_data_result(1);
            Session["TotalPage"] = data.totalNumPages;
        }
    }

    private OneMapData load_data_result(int page_index)
    {
        //OneMapData_Result[] results = null;
        IList<OneMapData_Result[]> result_list = new List<OneMapData_Result[]>();
        string url = "https://developers.onemap.sg/commonapi/search?searchVal=logistics&returnGeom=N&getAddrDetails=Y&pageNum=" + page_index.ToString();
        //int count = 1;

        //OneMapData data = new OneMapData();
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


        //DataSet ds = Dummy.ToDataSet<OneMapData_Result[]>(result_list);
        lblTest.Text = data.totalNumPages.ToString() + "\n";
        lblTest.Text += url;
        //lblTest.Text += "\nResult.Count " + result_list.Count.ToString() + "\nCount " + count.ToString();
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
        int counter = (int)Session["AttemptCount"];
        int totalPage = (int)Session["TotalPage"];

        if(0 < counter && counter < totalPage)
        {
            counter++;
            Session["AttemptCount"] = counter;
            data = load_data_result(counter);
            lblTest.Text = "Current Page : " + counter + " Total Page: " + totalPage;
        }
        else
        {
            //lblTest.Text = "ATTEMPT COUNT LIMIT REACHED";
            popup_error();
        }

        //do
        //{
        //    counter++;

        //    Session["AttemptCount"] = counter;
        //    lblTest.Text += "COUNTER" + counter + "\nTOTAL PAGE NO : " + data.totalNumPages;

        //    load_data_result(counter);
        //} while (counter == totalPage);
    }

    protected void btnPrevPage_Click(object sender, EventArgs e)
    {
        int counter = (int)Session["AttemptCount"];
        int totalPage = (int)Session["TotalPage"];

        if (1 < counter && counter <= totalPage)
        {
            counter--;
            Session["AttemptCount"] = counter;
            data = load_data_result(counter);
            lblTest.Text = "Current Page : " + counter + " Total Page: " + totalPage;
        }
        else
        {
            //lblTest.Text = "ATTEMPT COUNT LIMIT REACHED";
            popup_error();
        }
    }

    private void popup_error()
    {
        MessageBox.Show("page limit exceeded", "Warning",
    MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
    }

}