using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Contracts.Models
{
    internal class InvoiceStatusResponse
    {
        public int InvoiceId { get; set; }
        public string Status { get; set; }
        public DateTime LastModifiedUtc { get; set; }
    }
}
