using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
//using System.Web.Script.Serialization;
using System.Xml;
using BricsWeb.LocalModels;
using BricsWeb.Models;
using BricsWeb.Repository;

namespace BricsWeb.WebHelper
{
    public class DropdownGenerator
    {
        public static IEnumerable<SelectListItem> GetCountrySelectList()
        {
            var countryList = new CountryRepository().GetAll();

            var countrySelectList = from country in countryList
                                    select new SelectListItem
                                    {
                                        Value = country.Name,
                                        Text = country.Name
                                    };

            return countrySelectList;
        }
    }
}

