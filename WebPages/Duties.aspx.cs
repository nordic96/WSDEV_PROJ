using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using currencyconverter;
using LogisticsWS;

public partial class WebPages_Duties : System.Web.UI.Page
{
    LogisticsService cis = new LogisticsService();
    Converter ccs = new Converter();
    
    //ExcelRead excel = new ExcelRead();
    DataSet hrcodes = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable hrcodes = cis.GetAllDutiableTaxHSCode();
            gv1.DataSource = hrcodes;
            gv1.DataBind();
            ddlHRCode.DataSource = hrcodes;
            ddlHRCode.DataTextField = hrcodes.Columns[0].ToString();
            ddlHRCode.DataValueField = hrcodes.Columns[0].ToString();
            ddlHRCode.DataBind();
            string[] currencies = ccs.GetCurrencies();
            ddlConvert.DataSource = currencies;
            ddlHRCode.DataValueField = currencies.ToString();
            ddlConvert.DataBind();
            for (int i = 0; i < ddlHRCode.Items.Count; i++)
            {

                //Check for the Empty value
                if (ddlHRCode.Items[i].Value == "")
                {

                    //If Found Then Remove That Value From The DropdownList
                    ddlHRCode.Items.Remove("");
                }
            }
        }
    }


    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        LogisticsWS.Duty duty = new LogisticsWS.Duty();
        DataTable hrcodes = cis.GetAllDutiableTaxHSCode();
        ddlHRCode.DataSource = hrcodes;
        int i = ddlHRCode.SelectedIndex;
        string calculationCustomRate = hrcodes.Rows[i][2].ToString();
        string calculationExciseRate = hrcodes.Rows[i][3].ToString();
        double weight = Int32.Parse(tbWeight.Text);
        double totalproductprice = Int32.Parse(tbTotalProductPrice.Text);
        string HSCode = ddlHRCode.SelectedItem.Text;
        int[] array = { 1, 2 };


        if (ddlManufactured.Text == "Domestic")
        {
            duty = cis.CalculateDomesticProductDuty(HSCode, weight, totalproductprice);
            lblTotPrice.Text = duty.totalDuties.ToString();
        }

        else if (ddlManufactured.Text == "International")
        {
            duty = cis.CalculateIntnProductDuty(HSCode, weight, totalproductprice);

            if (duty.result == false)
            {
                lblTotPrice.Text = "There is no need for custom duty";
            }
            else
            {
                lblTotPrice.Text = duty.totalDuties.ToString();
            }

        }
    }

    protected void btnSearchBy_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string searchBy = ddlSearchBy.SelectedValue.ToString();
        string searchText = tbSearchBy.Text.ToString();
        dt = cis.SearchDutiableTaxHsCode(searchBy, searchText);
        gv1.DataSource = dt;
        gv1.DataBind();

    }


    protected void btnConvert_Click(object sender, EventArgs e)
    {
        if(lblTotPrice.Text == "")
        {
            lblConvertResult.Text = "Calculate the total price first!";
        }
        else
        {
            string CurrencyFrom = "SGD";
            string CurrencyTo = ddlConvert.SelectedValue.ToString();
            DateTime RateDate = DateTime.Now;
            decimal currencyresult = ccs.GetConversionRate(CurrencyFrom, CurrencyTo, RateDate);
            decimal totalpriceforconversion = Convert.ToDecimal(lblTotPrice.Text);
            decimal convertedResult = currencyresult * totalpriceforconversion;
            decimal finalconvertedResult = Math.Round(convertedResult, 2);
            lblConvertResult.Text = "Converted Price : " + ddlConvert.SelectedValue.ToString() + finalconvertedResult.ToString();
        }
    }

}