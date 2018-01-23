using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for LogisticsService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class LogisticsService : System.Web.Services.WebService
{

    public LogisticsService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
<<<<<<< HEAD
    public string HelloWorld()
    {
        return "Hello World";
    }
=======
    public string searchCountry(string countryCode, string countryName, string portCode, string portName)
    {
        return countryName;
    }//end method
>>>>>>> a1cc6a4f5eea924c263f34bf9fd24f5e703e6d61

}
