using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Fakture_Drzavno.Contracts.Models
{
    internal class IssueInvoiceRequest
    {
        public readonly string URL;
        public string Body { get; set; }
        public string RequestID { get; set; }

        public IssueInvoiceRequest(string registerToCir)
        {
            RequestID = Guid.NewGuid().ToString();
            var enviroment = ConfigurationManager.AppSettings.Get("Enviroment");
            var apiURI = ConfigurationManager.AppSettings.Get($"{enviroment}URI");

            URL = $"{apiURI}/sales-invoice/ubl?requestId={RequestID}&sendToCir={registerToCir}";
        }
    }
}
