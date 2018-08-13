using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace BricsWeb.Models
{
    public class CompanySubscriptionModel : TableServiceEntity
    {
        public CompanySubscriptionModel()
        { }

        public CompanySubscriptionModel(string productid,string chamber1,string chamber2,string chamber3,string chamber4,string transactionID,string companyID)
        {
            RowKey = Guid.NewGuid().ToString();
            PartitionKey = RowKey[0].ToString();

            ProductID = productid;
            StartDateTime = DateTime.UtcNow;

            Chamber1 = chamber1;
            Chamber2 = chamber2;
            Chamber3 = chamber3;
            Chamber4 = chamber4;

            TransactionID = transactionID;

            CompanyRowKey = companyID;
        }

        public string ProductID { get; set; }
        public DateTime StartDateTime { get; set; }

        public string Chamber1 { get; set; }
        public string Chamber2 { get; set; }
        public string Chamber3 { get; set; }
        public string Chamber4 { get; set; }

        public string TransactionID {get;set;}

        public string CompanyRowKey { get; set; }
    }
}