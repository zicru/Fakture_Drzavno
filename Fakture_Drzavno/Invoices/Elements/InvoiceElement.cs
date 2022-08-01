using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Invoices.Elements
{
    abstract class InvoiceElement
    {
        public string Name { get; set; }
        public IEnumerable<InvoiceElementAttribute> Attributes { get; set; }
    }
}
