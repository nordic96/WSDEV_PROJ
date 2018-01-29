using System;
using System.Collections.Generic;
using System.Data;
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
    DataSet hrcodes = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hrcodes = excel.ExcelReadData("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en");
            for (int t = 0; t < 5; t++)
            {
                hrcodes.Tables[0].Rows[t].Delete(); 
                
            }
            gv1.DataSource = hrcodes;
            gv1.DataBind();
            ddlHRCode.DataSource = hrcodes;
            ddlHRCode.DataTextField = hrcodes.Tables[0].Columns[0].ToString();
            ddlHRCode.DataValueField = hrcodes.Tables[0].Columns[0].ToString();
            ddlHRCode.DataBind();
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
        hrcodes = excel.ExcelReadData("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en");
        ddlHRCode.DataSource = hrcodes;
        int i = ddlHRCode.SelectedIndex + 5;
        string calculationCustomRate = hrcodes.Tables[0].Rows[i][2].ToString();
        string calculationExciseRate = hrcodes.Tables[0].Rows[i][3].ToString();
        double weight = Int32.Parse(tbWeight.Text);
        double totalproductprice = Int32.Parse(tbTotalProductPrice.Text);
        if (ddlManufactured.Text == "Domestic")
        {
            char[] MyChar = { ' ','p','c','e' };
            string subC = calculationCustomRate.Substring(0, 1);
            string subE = calculationExciseRate.Substring(0, 1);
            if (subC == "$") //CHECKING FOR CUSTOM DUTY THAT STARTS WITH '$'
            {
                string subC2 = calculationCustomRate.Substring(0, 7);
                string newsubC2 = subC2.Remove(0, 1);
                string NewString = newsubC2.TrimEnd(MyChar).ToString();
                double customDutiesValue = Convert.ToDouble(NewString);

                if (subE.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
                {
                    string subE2 = calculationExciseRate.Substring(0, 7);
                    string newsubE2 = subE2.Remove(0, 1);
                    string NewStringE = newsubE2.TrimEnd(MyChar).ToString();
                    double exciseDutiesValue = Convert.ToDouble(NewStringE);
                    double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue * weight);
                    string totalPrice = Convert.ToString(totPriceCalculation);
                    lblTotPrice.Text = "$"+totalPrice;
                }

                else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
                {
                    string subE2 = calculationExciseRate.Substring(0, 4);
                    string NewStringE = subE2.TrimEnd(MyChar).ToString();
                    double exciseDutiesValue = Convert.ToDouble(NewStringE);
                    double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue * weight);
                    string totalPrice = Convert.ToString(totPriceCalculation);
                    lblTotPrice.Text = "$" + totalPrice;
                }

                else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
                {
                    string subE2 = calculationExciseRate.Substring(0, 2);
                    double exciseDutiesValue = Convert.ToDouble(subE2);
                    double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue / 100) * (totalproductprice);
                    string totalPrice = Convert.ToString(totPriceCalculation);
                    lblTotPrice.Text = "$" + totalPrice;
                }

            }
            else if(subC == "N") //CHECKING FOR CUSTOM DUTY 'NIL'
            {
                if (subE.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
                {
                    string subE2 = calculationExciseRate.Substring(0, 7);
                    string newsubE2 = subE2.Remove(0, 1);
                    string NewStringE = newsubE2.TrimEnd(MyChar).ToString();
                    double exciseDutiesValue = Convert.ToDouble(NewStringE);
                    double totPriceCalculation = exciseDutiesValue * weight;
                    string totalPrice = Convert.ToString(totPriceCalculation);
                    lblTotPrice.Text = "$" + totalPrice;
                }

                else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
                {
                    string subE2 = calculationExciseRate.Substring(0, 4);
                    string NewStringE = subE2.TrimEnd(MyChar).ToString();
                    double exciseDutiesValue = Convert.ToDouble(NewStringE);
                    double totPriceCalculation = exciseDutiesValue * weight;
                    string totalPrice = Convert.ToString(totPriceCalculation);
                    lblTotPrice.Text = "$" + totalPrice;
                }

                else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
                {
                    string subE2 = calculationExciseRate.Substring(0, 2);
                    double exciseDutiesValue = Convert.ToDouble(subE2);
                    double totPriceCalculation = (exciseDutiesValue / 100) * (totalproductprice);
                    string totalPrice = Convert.ToString(totPriceCalculation);
                    lblTotPrice.Text = "$" + totalPrice;
                }
            }


        }

        else if (ddlManufactured.Text == "International")
        {
            char[] MyChar = { ' ', 'p', 'c', 'e' };
            string subC = calculationCustomRate.Substring(0, 1);
            if (subC.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
            {
                string subC2 = calculationCustomRate.Substring(0, 7);
                string newsubC2 = subC2.Remove(0, 1);
                string NewString = newsubC2.TrimEnd(MyChar).ToString();
                double customDutiesValue = Convert.ToDouble(NewString);
                double totPriceCalculation = customDutiesValue * weight;
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }

            else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
            {
                string subC2 = calculationExciseRate.Substring(0, 4);
                string NewStringC = subC2.TrimEnd(MyChar).ToString();
                double customDutiesValue = Convert.ToDouble(NewStringC);
                double totPriceCalculation = customDutiesValue * weight;
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }

            else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
            {
                string subC2 = calculationExciseRate.Substring(0, 2);
                double customDutiesValue = Convert.ToDouble(subC2);
                double totPriceCalculation = (customDutiesValue / 100) * (totalproductprice);
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }

            else if (subC == "N")
            {
                lblTotPrice.Text = "There is no need for custom duty";
            }
        }
    }

    private void CalculateDomesticProductDuty()
    {
        hrcodes = excel.ExcelReadData("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en");
        ddlHRCode.DataSource = hrcodes;
        int i = ddlHRCode.SelectedIndex + 5;
        string calculationCustomRate = hrcodes.Tables[0].Rows[i][2].ToString();
        string calculationExciseRate = hrcodes.Tables[0].Rows[i][3].ToString();
        double weight = Int32.Parse(tbWeight.Text);
        double totalproductprice = Int32.Parse(tbTotalProductPrice.Text);

        char[] MyChar = { ' ', 'p', 'c', 'e' };
        string subC = calculationCustomRate.Substring(0, 1);
        string subE = calculationExciseRate.Substring(0, 1);
        if (subC == "$") //CHECKING FOR CUSTOM DUTY THAT STARTS WITH '$'
        {
            string subC2 = calculationCustomRate.Substring(0, 7);
            string newsubC2 = subC2.Remove(0, 1);
            string NewString = newsubC2.TrimEnd(MyChar).ToString();
            double customDutiesValue = Convert.ToDouble(NewString);

            if (subE.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
            {
                string subE2 = calculationExciseRate.Substring(0, 7);
                string newsubE2 = subE2.Remove(0, 1);
                string NewStringE = newsubE2.TrimEnd(MyChar).ToString();
                double exciseDutiesValue = Convert.ToDouble(NewStringE);
                double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue * weight);
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }

            else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
            {
                string subE2 = calculationExciseRate.Substring(0, 4);
                string NewStringE = subE2.TrimEnd(MyChar).ToString();
                double exciseDutiesValue = Convert.ToDouble(NewStringE);
                double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue * weight);
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }

            else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
            {
                string subE2 = calculationExciseRate.Substring(0, 2);
                double exciseDutiesValue = Convert.ToDouble(subE2);
                double totPriceCalculation = (customDutiesValue * weight) + (exciseDutiesValue / 100) * (totalproductprice);
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }

        }
        else if (subC == "N") //CHECKING FOR CUSTOM DUTY 'NIL'
        {
            if (subE.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
            {
                string subE2 = calculationExciseRate.Substring(0, 7);
                string newsubE2 = subE2.Remove(0, 1);
                string NewStringE = newsubE2.TrimEnd(MyChar).ToString();
                double exciseDutiesValue = Convert.ToDouble(NewStringE);
                double totPriceCalculation = exciseDutiesValue * weight;
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }

            else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
            {
                string subE2 = calculationExciseRate.Substring(0, 4);
                string NewStringE = subE2.TrimEnd(MyChar).ToString();
                double exciseDutiesValue = Convert.ToDouble(NewStringE);
                double totPriceCalculation = exciseDutiesValue * weight;
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }

            else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
            {
                string subE2 = calculationExciseRate.Substring(0, 2);
                double exciseDutiesValue = Convert.ToDouble(subE2);
                double totPriceCalculation = (exciseDutiesValue / 100) * (totalproductprice);
                string totalPrice = Convert.ToString(totPriceCalculation);
                lblTotPrice.Text = "$" + totalPrice;
            }
        }
    }

    private double CalculateIntnProductDuty()
    {
        hrcodes = excel.ExcelReadData("https://www.customs.gov.sg/~/media/cus/files/business/valuation%20duties%20taxes%20and%20fees/list%20of%20dutiable%20goods20feb2017.xlsx?la=en");
        ddlHRCode.DataSource = hrcodes;
        int i = ddlHRCode.SelectedIndex + 5;
        string calculationCustomRate = hrcodes.Tables[0].Rows[i][2].ToString();
        string calculationExciseRate = hrcodes.Tables[0].Rows[i][3].ToString();
        double weight = Int32.Parse(tbWeight.Text);
        double totalproductprice = Int32.Parse(tbTotalProductPrice.Text);

        double totPriceCalculation = 0;

        char[] MyChar = { ' ', 'p', 'c', 'e' };
        string subC = calculationCustomRate.Substring(0, 1);
        if (subC.Equals("$")) //CHECKING FOR EXCISE DUTY THAT STARTS WITH '$'
        {
            string subC2 = calculationCustomRate.Substring(0, 7);
            string newsubC2 = subC2.Remove(0, 1);
            string NewString = newsubC2.TrimEnd(MyChar).ToString();
            double customDutiesValue = Convert.ToDouble(NewString);
            totPriceCalculation = customDutiesValue * weight;
            //string totalPrice = Convert.ToString(totPriceCalculation);
            //lblTotPrice.Text = "$" + totalPrice;
        }

        else if (calculationExciseRate.Contains("cents")) //CHECKING FOR EXCISE DUTY THAT CONTAINS 'CENTS'
        {
            string subC2 = calculationExciseRate.Substring(0, 4);
            string NewStringC = subC2.TrimEnd(MyChar).ToString();
            double customDutiesValue = Convert.ToDouble(NewStringC);
            totPriceCalculation = customDutiesValue * weight;
            //string totalPrice = Convert.ToString(totPriceCalculation);
            //lblTotPrice.Text = "$" + totalPrice;
        }

        else if (calculationExciseRate.Contains("%")) //CHECKING FOR EXCISE DUTY THAT CONTAINS '%'
        {
            string subC2 = calculationExciseRate.Substring(0, 2);
            double customDutiesValue = Convert.ToDouble(subC2);
            totPriceCalculation = (customDutiesValue / 100) * (totalproductprice);
            //string totalPrice = Convert.ToString(totPriceCalculation);
            //lblTotPrice.Text = "$" + totalPrice;
        }

        else if (subC == "N")
        {
            lblTotPrice.Text = "There is no need for custom duty";
        }

        return totPriceCalculation;
    }
}

