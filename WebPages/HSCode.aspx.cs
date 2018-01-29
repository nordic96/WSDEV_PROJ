using System;

public partial class HSCode : System.Web.UI.Page
{
    ExcelRead excel = new ExcelRead();
    //ReadExcel excel = new ReadExcel();
    protected void Page_Load(object sender, EventArgs e)
    {
        gv1.DataSource = excel.ExcelReadData("https://data.gov.in/sites/default/files/datafile/itchs2012.xls");
        gv1.DataBind();
        //Label1.Text = excel.ReadExcelData(@"C:\Users\rhrlg\Downloads/portcode2012.xls");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}