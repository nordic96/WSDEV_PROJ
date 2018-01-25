using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using PortService;
using System.Xml;

public partial class CustomDuties : System.Web.UI.Page
{
    ExcelRead excel = new ExcelRead();
    //ReadExcel excel = new ReadExcel();
    //airport cis = new airport();
    protected void Page_Load(object sender, EventArgs e)
    {

        gv1.DataSource = excel.ExcelReadData("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en");
        gv1.DataBind();
        ddlHRCode.DataSource = excel.ExcelReadData("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en");
        //ddlHRCode.DataTextField = "F1";
        //ddlHRCode.DataValueField = "F1";
        ddlHRCode.DataBind();
        //Label1.Text = excel.ReadExcelData(@"C:\Users\rhrlg\Downloads/portcode2012.xls");

    }


    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        int hscode = 1;
        int weight = Convert.ToInt32(tbWeight.Text);
        string mc = ddlManufactured.Text;
        int result;
        if (mc == "Domestic")
        {
            result = 88 * weight;
            lblTotPrice.Text = result.ToString();
        }
    }
}