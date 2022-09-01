using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Contracts.Responses
{
    internal class InvoiceStatusResponse
    {
        public int InvoiceId { get; set; }
        public string Status { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Comment { get; set; }
        public string CancelComment { get; set; }
        public string StornoComment { get; set; }
    }
}
