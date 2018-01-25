using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortService;
using System.Xml;
using System.Data;

public partial class MainPage : System.Web.UI.Page
{
    ExcelRead excel = new ExcelRead();
    //ReadExcel excel = new ReadExcel();
    airport cis = new airport();
    protected void Page_Load(object sender, EventArgs e)
    {
        gv1.DataSource = excel.ExcelReadData("http://www.pscoman.com/Portals/0/documents/portcode2012.xls");
        gv1.DataBind();
        //Label1.Text = excel.ReadExcelData(@"C:\Users\rhrlg\Downloads/portcode2012.xls");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}