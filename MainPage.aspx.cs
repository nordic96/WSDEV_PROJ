﻿using System;
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
    airport cis = new airport();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string search = TextBox1.Text;
        string ds = cis.GetAirportInformationByCountry(search);
        Label1.Text = ds;
    }
}