using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using System.ComponentModel.DataAnnotations;

namespace BricsWeb.Models
{
    public class CategoryModel : TableServiceEntity
    {
        public CategoryModel()
        { }

        public CategoryModel(string id, string name, string description, int depth, string parentid)
        {
            RowKey = id;
            PartitionKey = id[0].ToString();

            Name = name;
            Description = description;
            Depth = depth;
            ParentID = parentid;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Category Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Depth")]
        public int Depth { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ParentID")]
        public string ParentID { get; set; }
    }
}