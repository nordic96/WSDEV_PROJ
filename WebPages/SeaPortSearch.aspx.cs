using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_SeaPortSearch : System.Web.UI.Page
{
    ExcelRead excel = new ExcelRead();
    //ReadExcel excel = new ReadExcel();
    protected void Page_Load(object sender, EventArgs e)
    {
        gvSeaPort.DataSource = excel.ExcelReadData("http://www.pscoman.com/Portals/0/documents/portcode2012.xls");
        gvSeaPort.DataBind();
        //Label1.Text = excel.ReadExcelData(@"C:\Users\rhrlg\Downloads/portcode2012.xls");
    }
}