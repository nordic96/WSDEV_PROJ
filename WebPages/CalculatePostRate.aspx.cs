using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CountryService;
using System.Xml;
using System.Data;
using System.Windows.Forms;
using Logistics;

public partial class WebPages_CalculatePostRate : System.Web.UI.Page
{
    country cis = new country();
    LogisticsService logistics = new LogisticsService(); //WS Proxy

    protected void Page_Load(object sender, EventArgs e)
    {

        lblPostTitle.Text = ddlPostCalCategory.SelectedItem.Text + " ";
        localPostCalFrm.Style.Add("display", "block");
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
                p = logistics.calculateLocalPostMailPrice(select, weight);
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
                        lblTotalPrice.Text = "Total post charge: S$" + p.price.ToString("f2");
                    else
                        lblTotalPrice.Text = "";
                }
            }

        }
        else if (ddlPostSizeLocal.SelectedValue.Equals("empty") || txtPostWeightLocal.Text.Equals(""))
        {
            //Display Error message to choose one of the size
            MessageBox.Show("Please choose an option", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }
    }

    //Calculating Local Post Rate. To be implemented into WS.
    //private PostalPrice calculateLocalPost(string size, double weight)
    //{
    //    PostalPrice postalPrice = new PostalPrice();
    //    postalPrice.result = true;

    //    double total = 0.0;
    //    string key = "LocalRate_" + size;
    //    int rate_key = 0;

    //    string[] rate_string = System.Web.Configuration.WebConfigurationManager.AppSettings[key].Split(',');

    //    if (weight <= 20)
    //        rate_key = 0;
    //    else if (20 < weight && weight <= 40)
    //        rate_key = 1;
    //    else if (40 < weight && weight <= 100)
    //        rate_key = 2;
    //    else if (100 < weight && weight <= 250)
    //        rate_key = 3;
    //    else if (250 < weight && weight <= 500)
    //        rate_key = 4;
    //    else if (500 < weight && weight <= 1000)
    //        rate_key = 5;
    //    else if (1000 < weight && weight <= 2000)
    //        rate_key = 6;

    //    if(rate_key+1 <= rate_string.Length)
    //        total = Convert.ToDouble(rate_string[rate_key]);

    //    else if (rate_key+1 > rate_string.Length)
    //        postalPrice.result = false;

    //    postalPrice.price = total;
    //    return postalPrice;
    //}
}