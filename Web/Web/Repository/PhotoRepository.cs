using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureHelper.Storage;
using BricsWeb.Properties;

namespace BricsWeb.Repository
{
    public class PhotoRepository
    {
        BlobHelper Blobber;
        const string containerName = "productphoto";

        public PhotoRepository()
        {
            //Blobber = new BlobHelper(@"DefaultEndpointsProtocol=http;AccountName=%1;AccountKey=%2".Replace("%1", accountname).Replace("%2", accountkey));
            Blobber = new BlobHelper(Settings.Default.ConnectionString);
        }

        public string SavePhoto(byte[] photo, string id)
        {
            return Blobber.QuickSaveBlob(containerName, photo, id);
        }

        public void DeletePhoto(string url)
        {

        }

    }
}