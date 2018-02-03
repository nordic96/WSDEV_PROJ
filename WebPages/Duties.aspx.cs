using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using currencyconverter;

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
            //ddlHRCode.DataTextField = hrcodes.Tables[0].Columns[0].ToString();
            //ddlHRCode.DataValueField = hrcodes.Tables[0].Columns[0].ToString();
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
        Duty duty = new Duty();
        DataTable hrcodes = cis.GetAllDutiableTaxHSCode();
        ddlHRCode.DataSource = hrcodes;
        //int i = ddlHRCode.SelectedIndex + 5;
        int i = ddlHRCode.SelectedIndex;
        //string calculationCustomRate = hrcodes.Tables[0].Rows[i][2].ToString();
        //string calculationExciseRate = hrcodes.Tables[0].Rows[i][3].ToString();
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

    //private void CalculateDomesticProductDuty()
    //{
    //    hrcodes = excel.ExcelReadData("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en");
    //    ddlHRCode.DataSource = hrcodes;
    //    int i = ddlHRCode.SelectedIndex + 5;
    //    string calculationCustomRate = hrcodes.Tables[0].Rows[i][2].ToString();
    //    string calculationExciseRate = hrcodes.Tables[0].Rows[i][3].ToString();
    //    double weight = Int32.Parse(tbWeight.Text);
    //    double totalproductprice = Int32.Parse(tbTotalProductPrice.Text);

    //    char[] MyChar = { ' ', 'p', 'c', 'e' };
    //    string subC = calculationCustomRate.Substring(0, 1);
    //    string subE = calculationExciseRate.Substring(0, 1);
    //    if (subC == "$") //CHECKING FOR CUSTOM DUTY THAT STARTS WITH '$'
    //    {
    //        string subC2 = calculationCustomRate.Substring(0, 7);
    //        string newsubC2 = subC2.Remove(0, 1);
    //        string NewString = newsubC2.TrimEnd(MyChar).ToString();
    //        double customDutiesValue = Convert.ToDouble(NewString);

    //        if (subE.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
    //        {
    //            string subE2 = calculationExciseRate.Substring(0, 7);
    //            string newsubE2 = subE2.Remove(0, 1);
    //            string NewStringE = newsubE2.TrimEnd(MyChar).ToString();
    //            double exciseDutiesValue = Convert.ToDouble(NewStringE);
    //            double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue * weight);
    //            string totalPrice = Convert.ToString(totPriceCalculation);
    //            lblTotPrice.Text = "$" + totalPrice;
    //        }

    //        else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
    //        {
    //            string subE2 = calculationExciseRate.Substring(0, 4);
    //            string NewStringE = subE2.TrimEnd(MyChar).ToString();
    //            double exciseDutiesValue = Convert.ToDouble(NewStringE);
    //            double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue * weight);
    //            string totalPrice = Convert.ToString(totPriceCalculation);
    //            lblTotPrice.Text = "$" + totalPrice;
    //        }

    //        else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
    //        {
    //            string subE2 = calculationExciseRate.Substring(0, 2);
    //            double exciseDutiesValue = Convert.ToDouble(subE2);
    //            double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue / 100) * (totalproductprice);
    //            string totalPrice = Convert.ToString(totPriceCalculation);
    //            lblTotPrice.Text = "$" + totalPrice;
    //        }

    //    }
    //    else if (subC == "N") //CHECKING FOR CUSTOM DUTY 'NIL'
    //    {
    //        if (subE.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
    //        {
    //            string subE2 = calculationExciseRate.Substring(0, 7);
    //            string newsubE2 = subE2.Remove(0, 1);
    //            string NewStringE = newsubE2.TrimEnd(MyChar).ToString();
    //            double exciseDutiesValue = Convert.ToDouble(NewStringE);
    //            double totPriceCalculation = exciseDutiesValue * weight;
    //            string totalPrice = Convert.ToString(totPriceCalculation);
    //            lblTotPrice.Text = "$" + totalPrice;
    //        }

    //        else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
    //        {
    //            string subE2 = calculationExciseRate.Substring(0, 4);
    //            string NewStringE = subE2.TrimEnd(MyChar).ToString();
    //            double exciseDutiesValue = Convert.ToDouble(NewStringE);
    //            double totPriceCalculation = exciseDutiesValue * weight;
    //            string totalPrice = Convert.ToString(totPriceCalculation);
    //            lblTotPrice.Text = "$" + totalPrice;
    //        }

    //        else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
    //        {
    //            string subE2 = calculationExciseRate.Substring(0, 2);
    //            double exciseDutiesValue = Convert.ToDouble(subE2);
    //            double totPriceCalculation = (exciseDutiesValue / 100) * (totalproductprice);
    //            string totalPrice = Convert.ToString(totPriceCalculation);
    //            lblTotPrice.Text = "$" + totalPrice;
    //        }
    //    }
    //}

    //private Duty CalculateIntnProductDuty(string HSCode, double weight, double totalPrice)
    //{
    //    DataSet hrcodes = new DataSet();
    //    Duty duty = new Duty();
    //    bool result = true;
    //    double totPriceCalculation = 0;

    //    ddlHRCode.DataSource = hrcodes;
    //    hrcodes = excel.ExcelSearchData("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en",
    //        "List of Dutiable Goods", HSCode);
    //    //int i = ddlHRCode.SelectedIndex + 5;
    //    //string HSCode = ddlHRCode;
    //    string calculationCustomRate = hrcodes.Tables[0].Rows[1][2].ToString();
    //    string calculationExciseRate = hrcodes.Tables[0].Rows[1][3].ToString();
    //    //double weight = Int32.Parse(tbWeight.Text);
    //    //double totalproductprice = Int32.Parse(tbTotalProductPrice.Text);
    //    double totalproductprice = totalPrice;


    //    char[] MyChar = { ' ', 'p', 'c', 'e' };
    //    string subC = calculationCustomRate.Substring(0, 1);
    //    if (subC.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
    //    {
    //        string subC2 = calculationCustomRate.Substring(0, 7);
    //        string newsubC2 = subC2.Remove(0, 1);
    //        string NewString = newsubC2.TrimEnd(MyChar).ToString();
    //        double customDutiesValue = Convert.ToDouble(NewString);
    //        totPriceCalculation = customDutiesValue * weight;
    //        //string totalPrice = Convert.ToString(totPriceCalculation);
    //        //lblTotPrice.Text = "$" + totalPrice;
    //    }

    //    else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
    //    {
    //        string subC2 = calculationExciseRate.Substring(0, 4);
    //        string NewStringC = subC2.TrimEnd(MyChar).ToString();
    //        double customDutiesValue = Convert.ToDouble(NewStringC);
    //        totPriceCalculation = customDutiesValue * weight;
    //        //string totalPrice = Convert.ToString(totPriceCalculation);
    //        //lblTotPrice.Text = "$" + totalPrice;
    //    }

    //    else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
    //    {
    //        string subC2 = calculationExciseRate.Substring(0, 2);
    //        double customDutiesValue = Convert.ToDouble(subC2);
    //        totPriceCalculation = (customDutiesValue / 100) * (totalproductprice);
    //        //string totalPrice = Convert.ToString(totPriceCalculation);
    //        //lblTotPrice.Text = "$" + totalPrice;
    //    }

    //    else if (subC == "N")
    //    {
    //        result = false;
    //        //lblTotPrice.Text = "There is no need for custom duty";
    //    }

    //    duty.totalDuties = totalproductprice;
    //    duty.result = result;
    //    return duty;
    //}

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