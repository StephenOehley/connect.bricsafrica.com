using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureHelper.Storage;
using BricsWeb.Models;
using BricsWeb.Properties;

namespace BricsWeb.Repository
{
    public class CompanyRepository : CloudRepositoryBase<CompanyModel>
    {
        public CompanyRepository()
            : base(Settings.Default.CompanyTable,Settings.Default.ConnectionString)
        { }

        public CompanyModel GetBySalesEmail(string email)
        {
            var company = (from entity in serviceContext.CreateQuery<CompanyModel>(tableName)
                        where (entity.SalesEmail == email.ToLower())
                        select entity).Take(1).Single();
            return company;
        }

        public CompanyModel GetByAccountsEmail(string email)
        {
            var company = (from entity in serviceContext.CreateQuery<CompanyModel>(tableName)
                        where (entity.AccountsEmail == email.ToLower())
                        select entity).Take(1).Single();
            return company;
        }

        public CompanyModel GetByVatNumber(string vatNumber)
        {
            var company = (from entity in serviceContext.CreateQuery<CompanyModel>(tableName)
                        where (entity.VatNumber == vatNumber.ToLower())
                        select entity).Take(1).Single();
            return company;
        }

        //Override base save method to over storage api limitation
        public override void Save(CompanyModel company)
        {
            if (company.SalesEmail == null)
            {
                company.SalesEmail = string.Empty;
            }

            if (company.AccountsEmail == null)
            {
                company.AccountsEmail = string.Empty;
            }

            company.SalesEmail = company.SalesEmail.ToLower();//ensure that saved email address is lowercase, workaround for MS half-arsed implementation of ODATA that does not support .TOLOWER in queries
            company.AccountsEmail = company.AccountsEmail.ToLower();
            base.Save(company);
        }
    }
}