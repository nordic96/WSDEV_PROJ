using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for LoadOneMapData
/// </summary>
public class LoadOneMapData
{
    public LoadOneMapData()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public OneMapData load_data_result(string searchVal, int page_index)
    {
        OneMapData data = new OneMapData();
        OneMapData_Result[] results;

        IList<OneMapData_Result[]> result_list = new List<OneMapData_Result[]>();
        string url = System.Web.Configuration.WebConfigurationManager.AppSettings["OneMapAPIUrl"] +
            "?searchVal=" + searchVal + "&returnGeom=N&getAddrDetails=Y&pageNum=" + page_index.ToString();

        try
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            var webResponse = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                string resString = sr.ReadToEnd();

                data = json.Deserialize<OneMapData>(resString);
                results = data.results;
            }
        }
        catch (Exception ex)
        {
            
        }
        return data;
    }
}