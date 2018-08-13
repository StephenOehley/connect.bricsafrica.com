using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Models;

namespace BricsWeb.Repository
{
    public class SubscriptionProductRepository
    {
        List<SubscriptionProductModel> _ProductDatasource;

        public SubscriptionProductRepository()
        {
            _ProductDatasource = new List<SubscriptionProductModel>();

            _ProductDatasource.Add(new SubscriptionProductModel { ID = "2012SILVER", Name = "Annual Silver Subscription", Description = string.Empty,Price = 200000,Level = 1,MaxFeatured = 10 });
            _ProductDatasource.Add(new SubscriptionProductModel { ID = "2012GOLD", Name = "Annual Gold Subscription", Description = string.Empty, Price = 300000, Level = 2, MaxFeatured = 25});
            _ProductDatasource.Add(new SubscriptionProductModel { ID = "2012PLATINUM", Name = "Annual Platinum Subscription", Description = string.Empty, Price = 400000, Level = 3,MaxFeatured = 1000});
        }

        public IEnumerable<SubscriptionProductModel> GetAll()
        {
            return _ProductDatasource;
        }

    }
}