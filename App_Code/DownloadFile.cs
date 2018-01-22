using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

/// <summary>
/// Summary description for DownloadFile
/// </summary>
public class DownloadFile
{
    public string DownloadFileTemp(string url)
    {
        var client = new WebClient();

        var fullPath = Path.GetTempFileName();
        client.DownloadFile(url, fullPath);
        return fullPath;
        //
        // TODO: Add constructor logic here
        //
    }
}