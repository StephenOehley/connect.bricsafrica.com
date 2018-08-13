using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using BricsWeb.Models;

namespace BricsWeb.Repository
{
    public class CountryRepository
    {
        public IEnumerable<CountryModel> GetAll()
        {
            var xml = File.ReadAllText(HostingEnvironment.ApplicationPhysicalPath + @"/Data/CountryList.xml");
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            var countryList = new List<CountryModel>();
            foreach (XmlNode node in doc.SelectNodes("//country"))
            {
                countryList.Add(new CountryModel { Name = node.InnerText, Iso2DigitCode = node.Attributes["code"].InnerText });
            }

            return countryList;
        }
    }
}