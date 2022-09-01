using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Contracts.Responses
{
    internal class InvoiceStatusResponseOnDateResponse
    {
        public int SalesInvoiceId { get; set; }
        public string NewInvoiceStatus { get; set; }
        public DateTime Date { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Comment { get; set; }
        public int? EventId { get; set; }
    }
}
