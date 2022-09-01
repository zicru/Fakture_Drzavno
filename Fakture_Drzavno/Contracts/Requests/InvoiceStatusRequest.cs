using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Fakture_Drzavno.Contracts.Requests
{
    internal class InvoiceStatusRequest
    {
        public readonly string URL;
        public string InvoiceId { get; set; }

        public InvoiceStatusRequest(string invoiceId)
        {
            InvoiceId = invoiceId;

            var enviroment = ConfigurationManager.AppSettings.Get("Enviroment");
            var apiURI = ConfigurationManager.AppSettings.Get($"{enviroment}URI");

            URL = $"{apiURI}/sales-invoice?invoiceId={InvoiceId}";
        }
    }
}
