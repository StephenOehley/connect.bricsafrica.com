using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using System.ComponentModel.DataAnnotations;

namespace BricsWeb.Models
{
    public class BuyerRequestModel : TableServiceEntity
    {
        public BuyerRequestModel()
        { }

        public BuyerRequestModel(string companyID, string title, string category, string description, int quantity, DateTime dateExpire, string photoUrl)
        {
            DatePosted = DateTime.UtcNow;

            RowKey = Guid.NewGuid().ToString();

            CompanyID = companyID;
            RequestTitle = title;
            Category = category;
            RequestDescription = description;
            Quantity = quantity;
            DateExpire = dateExpire;
            PhotoUrl = photoUrl;

            PartitionKey = RowKey.Substring(0, 1);
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Buyer CompanyID")]
        public string CompanyID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Request Title")]
        public string RequestTitle { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Request Description")]
        public string RequestDescription { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Quantity Required")]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Request Expiry Date")]
        public DateTime DateExpire { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photo Url")]
        public string PhotoUrl { get; set; }
    }
}