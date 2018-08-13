using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace BricsWeb.Models
{
    public class TransactionModel : TableServiceEntity
    {
        public TransactionModel()
        { }

        public TransactionModel(string transactionID,string companyID,string description,string productID,string additionalInfo = "",double amount = 0.00)
        {
            RowKey = Guid.NewGuid().ToString();
            PartitionKey = RowKey[0].ToString();

            TransactionID = transactionID;
            CompanyID = companyID;
            Description = description;
            EventDateTime = DateTime.UtcNow;

            ProductID = productID;
            Amount = amount;

            AdditionalInfo = additionalInfo;
        }

        public string TransactionID { get; set; }
        public string CompanyID { get; set; }        
        public string Description { get; set; }
        public DateTime EventDateTime { get; set; }
        public string AdditionalInfo { get; set; }
        
        public string ProductID { get; set; }
        public double Amount { get; set; }
    }
}