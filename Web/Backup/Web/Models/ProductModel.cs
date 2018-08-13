using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.StorageClient;

namespace BricsWeb.Models
{
    public class ProductModel : TableServiceEntity
    {
        public ProductModel()
        { }

        public ProductModel(string name, string description, string photourl, string companyid)
        {
            RowKey = Guid.NewGuid().ToString();

            ProductName = name;
            Description = description;
            PhotoUrl = photourl;
            IsApproved = false;
            CompanyID = companyid;

            BrandName = string.Empty;

            PartitionKey = RowKey.Substring(0, 1);

            FeaturedProductWeight = 0;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Seller CompanyID")]
        public string CompanyID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "CategoryID")]
        public string CategoryID { get; set; }

        public string PhotoUrl { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Product Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FOB Price")]
        public string FobPrice { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Minimum Order Quantity")]
        public int MinimumOrderQuantity { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Port")]
        public string Port { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Model Number")]
        public string ModelNumber { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Supply Ability")]
        public string SupplyAbility { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Payment Terms")]
        public string PaymentTerms { get; set; }

        [Required]
        [Display(Name = "Place Of Origin")]
        public string PlaceOfOrigin { get; set; }

        [Display(Name = "Certificate Number")]
        public string CertificateNumber { get; set; }

        [Display(Name = "Quality")]
        public string Quality { get; set; }

        [Display(Name = "Colour")]
        public string Colour { get; set; }

        [Display(Name = "Packaging & Delivery")]
        public string PackagingAndDelivery { get; set; }

        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }

        public int FeaturedProductWeight { get; set; }
    }
}