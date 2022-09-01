using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Contracts.Responses
{
    internal class IssueInvoiceResponse
    {
        public int InvoiceId { get; set; }
        public int InternalInvoiceId { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public int SalesInvoiceId { get; set; }
    }
}
