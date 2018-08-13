using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;
using AzureHelper.Storage;
using BricsWeb.Properties;

namespace BricsWeb.Repository
{
    public class TransactionRepository : CloudRepositoryBase<TransactionModel>
    {
        public TransactionRepository()
            : base(Settings.Default.TransactionTable, Settings.Default.ConnectionString)
        { }

        public bool IsTransactionValid(string transactionID)
        {
            var result = (from c in serviceContext.CreateQuery<TransactionModel>(tableName)
                          where c.TransactionID == transactionID
                          select c).ToArray().Count();

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public TransactionModel[] GetTransactionsByTransactionID(string transactionID)
        {
            var result = (from c in serviceContext.CreateQuery<TransactionModel>(tableName)
                          where c.TransactionID == transactionID
                          select c).ToArray();

            return result;
        }

        public TransactionModel[] GetTransactionsByCompanyID(string companyID)
        {
            var result = (from c in serviceContext.CreateQuery<TransactionModel>(tableName)
                          where c.CompanyID == companyID
                          select c).ToArray();

            return result;
        }

    }
}