using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BricsWeb.Properties;
using System.Security.Cryptography;
using System.Text;

namespace BricsWeb.LocalModels
{
    public class TransactionViewModel
    {
        public string TerminalID_p1 { get; set; }//p1
        public string TransactionID_p2 { get; set; }//p2
        public string Description_p3 { get; set; }//p3
        public string Amount_p4 { get; set; }//p4

        public string CardholderEmail_CardHolderEmail { get; set; }//CardHolderEmail

        public string CompanyID_m_1 { get; set; }//m_1

        public string Hash_Hash { get; set; }//Hash

        public string HashPassword_HashPassword { get; set; }

        public string GenerateHash()
        {
            var md5 = MD5.Create();

            var input = TerminalID_p1 + TransactionID_p2 + Description_p3 + Amount_p4 + CardholderEmail_CardHolderEmail + CompanyID_m_1 + HashPassword_HashPassword;

            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var resultStringBuilder = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                resultStringBuilder.Append(b.ToString("x2"));
            }

            return resultStringBuilder.ToString();
        }
        
    }
}