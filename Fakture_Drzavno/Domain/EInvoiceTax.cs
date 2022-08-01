using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Domain
{

    class EInvoiceTax
    {
        public static readonly string VIEW_NAME = "v_za_reregistrovanje_fakture_porezi";

        // Internal ID fakture
        public string InternalInvoiceID { get; set; }

        // Podaci o jednom tipu poreza
        // Ukupna osnovica poreske stope
        public decimal TaxSubtotalTaxableAmount { get; set; }

        // Ukupan iznos obracunatog poreza
        public decimal TaxSubtotalAmount { get; set; }

        // Poreska kategorija
        // Id poreske kategorije
        public string TaxSubtotalCategoryID { get; set; }

        // Procenat poreza
        public decimal TaxSubtotalPercent { get; set; }

        // Tazlog oslobadjanja od poreza
        public string? TaxSubtotalExemptionReason { get; set; }

        // Ime poreske kategorije - VAT
        public string? TaxSubtotalSchemeID { get; set; }
    }

}
