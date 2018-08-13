using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BricsWeb.LocalModels
{
    public class TransactionHistoryModel
    {
        public string TransactionID { get; private set; }

        public string ChamberName { get; private set; }
        public string MemberName { get; private set; }
        public double PurchaseAmount { get;private set; }
        public double Commission { get; private set; }
        public string ProductDescription { get; private set; }
        public DateTime TransactionTimeStamp { get; private set; }
        public double ChamberRunningTotal { get; set; }

        public TransactionHistoryModel(string id, string memberName, double purchaseAmount, string productDescription, DateTime timestamp,double commission,string chamberName)
        {
            TransactionID = id;
            MemberName = memberName;
            PurchaseAmount = purchaseAmount;
            ProductDescription = productDescription;
            TransactionTimeStamp = timestamp;
            Commission = commission;
            ChamberName = chamberName;
        }
    }
}