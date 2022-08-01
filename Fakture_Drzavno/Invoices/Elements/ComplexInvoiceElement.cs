using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Invoices.Elements
{
    class ComplexInvoiceElement : InvoiceElement
    {
        public IEnumerable<InvoiceElement> Elements { get; set; }

        public XMLSchema Schema { get; set; }
    }
}
