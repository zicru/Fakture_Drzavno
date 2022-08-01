using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Domain
{
    class EInvoiceItem
    {
        public static readonly string VIEW_NAME = "v_za_reregistrovanje_fakture_stavka";

        // Internal ID fakture
        public string InternalInvoiceID { get; set; }

        // Redni broj stavke u racunu
        public int LineID { get; set; }

        // Kolicina
        public decimal InvoicedQuantity { get; set; }

        // Iznos stavke bez poreza umanjen za popust i uvecan za trosarinu
        public decimal LineExtensionAmount { get; set; }

        // Period
        public DateTime? InvoiceLineStartDate { get; set; }

        public DateTime? InvoiceLineEndDate { get; set; }

        // Unos podataka o popustu ili trosku po stavci
        public bool? InvoiceLineChargeIndicator { get; set; }

        // Tekstualni opis troska ili popusta
        public string? InvoiceLineAllowanceChargeReason { get; set; }

        // Procenat
        public decimal? InvoiceLineMultiplierFactorNumeric { get; set; }

        // Iznos popusta ili troska po stavci
        public decimal? InvoiceLineAmount { get; set; }

        // Taxable sta? Base amount
        public decimal? InvoiceLineBaseAmount { get; set; }

        // Podaci o porezu po stavic
        // Ukupan iznos poreza
        public decimal TaxTotalAmount { get; set; }

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

        // U šifarnik osnova izuzeća se uključuju šifre prema odredbama ZPDV - NEMA GA
        public string? TaxExemptionReasonCode { get; set; }

        // Ime poreske kategorije - VAT
        public string? TaxSubtotalSchemeID { get; set; }

        // Podaci o artiklu ili usluzi
        // Naziv artikla ili usluge
        public string InvoiceLineItemName { get; set; }

        // Sifre artikala...

        // Podaci o porezu po svakoj stavci
        // ID Poreske kategorije ( S/E )
        public string ClassifiedTaxCategoryID { get; set; }

        // Procenat poreza - ne navodi se ako je neoporezivo
        public decimal ClassifiedTaxPercent { get; set; }

        public string? ClassifiedTaxExemptionReason { get; set; }

        // Ime poreske kategorije - VAT
        public string ClassifiedTaxSchemeID { get; set; }

        // TODO - ova dva ne mogu biti null a jesu u bazi
        // Jedinicna cena artikla ili usluge
        public decimal PriceAmount { get; set; }

        // Jedinicna kolicina artikla ili usluge
        public int BaseQuantity { get; set; }
    }
}
