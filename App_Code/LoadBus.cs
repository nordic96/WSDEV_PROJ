using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
/// <summary>
/// Summary description for LoadBusData
/// </summary>
public class LoadBus
{
    public LoadBus()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Bus loadBusDataResult(string busStopCode, string busServiceNumber)
    {
        Bus data = new Bus();
        Bus_Result[] results;


        

        //IList<Bus_Result[]> result_list = new List<Bus_Result[]>();
        // url = System.Web.Configuration.WebConfigurationManager.AppSettings["BusUrl"] + "?busStopCode=" + busStopCode + "&busServiceNumber=" + busServiceNumber;

        //try
        //{
         //   var webRequest = (HttpWebRequest)WebRequest.Create(url);
         //   var webResponse = (HttpWebResponse)webRequest.GetResponse();

          //  if (webResponse.StatusCode == HttpStatusCode.OK)
          //  {
           //     JavaScriptSerializer json = new JavaScriptSerializer();
           //     StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
           //     string resString = sr.ReadToEnd();

           //     data = json.Deserialize<Bus>(resString);
            //    results = data.bus_results;
           // }
        //}
        //catch (Exception ex)
        //{
            
        //}
        //return data;
    }
}