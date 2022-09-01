using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Domain
{

    class EInvoice
    {
        public static readonly string VIEW_NAME = "v_za_reregistrovanje_fakture";

        // Fields
        // Internal ID
        public string InternalInvoiceID { get; set; }

        // Broj racuna
        public string InvoiceID { get; set; }

        // Datum izdavanja racuna
        public DateTime InvoiceIssueDate { get; set; }

        // Valuta placanja ( datum valute? )
        public DateTime DueDate { get; set; }

        // Medjunarodna oznaka racuna
        public int InvoiceTypeCode { get; set; }

        // Vreme izdavanja racuna
        public string TimeNote { get; set; }

        // Oznaka operatera
        public string OperatorNote { get; set; }

        // Ime i prezime odgovorne osobe
        public string ResponsiblePersonNote { get; set; }

        // Napomena na nivou dokumenta
        public string? DocumentNote { get; set; }

        // Novcana valuta - za dinar je rsd
        public string DocumentCurrencyCode { get; set; }

        // Obracunski period
        public DateTime InvoicePeriodStartDate { get; set; }

        public DateTime InvoicePeriodEndDate { get; set; }

        // Kod koji se odnosi na taj racun definisan po standardu ( 3, 35, 432 )
        public int InvoicePeriodDescriptionCode { get; set; }

        // Polje za unos oznake narudzbine
        public string? OrderReferenceID { get; set; }

        // Polje za unos oznake otpremnice
        public string? DespatchDocumentReferenceID { get; set; }

        // Polje za unos reference na avans
        public string? OriginatorDocumentReferenceID { get; set; }

        // Polje za unos oznake ugovora
        public string? ContractDocumentReferenceID { get; set; }

        // PDF vizualizacija racuna 
        // ID dokumenta
        public string? AdditionalDocumentReferenceID { get; set; }

        // Prilog
        public int? AdditionalDocumentReferenceTypeCode { get; set; }

        // Putanja do fajla
        public string FilePath { get; set; }

        // Putanja do fajla 2
        public string? SecondFilePath { get; set; }

        // Putanja do fajla 3
        public string? ThirdFilePath { get; set; }

        // Elektronska adresa posiljaoca. Ako je posiljalac ima, schemeID = 0088 u kombinaciji sa GLN. Ako ne, 9948 sa PIB
        // PIB ili GLN
        public string SupplierPartyEndpointID { get; set; }

        // SchemeID
        public int? SupplierPartySchemeID { get; set; }

        // Sifra drzave sa PIB-om dobavljaca. 9948:PIB, za javne ustanove JBKJS:BROJ
        public string SupplierPartyIdentificationID { get; set; }

        // Naziv subjekta posiljaoca
        public string? SupplierPartyName { get; set; }

        // Podaci o adresi posiljaoca
        // Registrovana adresa posiljaoca
        public string? SupplierPartyStreetName { get; set; }

        // Sediste ( grad ) posiljaoca
        public string? SupplierPartyCityName { get; set; }

        // Postanski broj sedista posiljaoca
        public string? SupplierPartyPostalZone { get; set; }

        // Puna adresa posiljaoca
        public string? SupplierPartyPostalAddressLine { get; set; }

        // Sediste ( drzava ) posiljaoca
        public string? SupplierPartyCountryIdentificationCode { get; set; }

        // PIB subjekta posiljaoca
        public string SupplierPartyCompanyID { get; set; }

        // Ime poreske kategorije - VAT
        public string SupplierPartyTaxSchemeID { get; set; }

        // Zvanicni podaci iz APR-a
        // Naziv subjekta posiljaoca
        public string SupplierPartyLegalEntityName { get; set; }

        // Maticni broj subjekta posiljaoca
        public string SupplierPartyLegalEntityCompanyID { get; set; }

        // Podaci o firmi upisani u registru
        public string? SupplierPartyLegalEntityCompanyForm { get; set; }

        // Ime i prezime odgovorne osobe posiljaoca
        public string? SupplierPartyContactName { get; set; }

        // Broj telefona posiljaoca
        public string? SupplierPartyContactPhone { get; set; }

        // Korespodentna email adresa posiljaoca
        public string? SupplierPartyContactElectronicMail { get; set; }

        // Elektronska adresa primaoca. Ako je primaoc ima, schemeID = 0088 u kombinaciji sa GLN. Ako ne, 9948 sa PIB
        // PIB ili GLN
        public string CustomerPartyEndpointID { get; set; }

        // Sifra drzave sa PIB-om dobavljaca. 9948:PIB, za javne ustanove JBKJS:BROJ
        public string CustomerPartyIdentificationID { get; set; }

        // Naziv subjekta primaoca
        public string? CustomerPartyName { get; set; }

        // Podaci o adresi primaoca
        // Registrovana adresa primaoca
        public string? CustomerPartyStreetName { get; set; }

        // Sediste ( grad ) primaoca
        public string? CustomerPartyCityName { get; set; }

        // Postanski broj sedista primaoca
        public string? CustomerPartyPostalZone { get; set; }

        // Puna adresa primaoca
        public string? CustomerPartyPostalAddressLine { get; set; }

        // Sediste ( drzava ) primaoca
        public string? CustomerPartyCountryIdentificationCode { get; set; }

        // PIB subjekta primaoca
        public string CustomerPartyCompanyID { get; set; }

        // Ime poreske kategorije - VAT
        public string CustomerPartyTaxSchemeID { get; set; }

        // Zvanicni podaci iz APR-a
        // Naziv subjekta primaoca
        public string CustomerPartyLegalEntityName { get; set; }

        // Maticni broj subjekta primaoca
        public string CustomerPartyLegalEntityCompanyID { get; set; }

        // Podaci o firmi upisani u registru
        public string? CustomerPartyLegalEntityCompanyForm { get; set; }

        // Ime i prezime odgovorne osobe primaoca
        public string? CustomerPartyContactName { get; set; }

        // Broj telefona primaoca
        public string? CustomerPartyContactPhone { get; set; }

        // Korespodentna email adresa primaoca
        public string? CustomerPartyContactElectronicMail { get; set; }

        // Podaci o isporuci
        // Datum kada je usluga izvrsena ili je izvrsena isporuka
        public DateTime? ActualDeliveryDate { get; set; }

        public string? DeliveryStreetName { get; set; }

        public string? DeliveryCityName { get; set; }

        public int? DeliveryPostalZone { get; set; }

        public string? DeliveryCountryIdentificationCode { get; set; }

        // Podaci u placanju
        // Sifra nacina placanja
        public int PaymentMeansCode { get; set; }

        // Opis placanja
        public string? InstructionNote { get; set; }

        // Model placanja i poziv na broj placanja
        public string? PaymentID { get; set; }

        // Podaci o bankovnom racunu posiljaoca
        public string PayeeFinancialAccountID { get; set; }

        // Podaci o uslovima placanja
        public string? PaymentTermsNote { get; set; }

        // Unos podataka o popustu ili trosku na nivou dokumenta
        public bool? AllowanceChargeIndicator { get; set; }

        // Tekstualni opis p ili t
        public string? AllowanceChargeReason { get; set; }

        // Procenat
        public decimal? AllowanceChargeMultiplierFactorNumeric { get; set; }

        // Iznos p ili t na nivou dokumenta
        public decimal? AllowanceChargeAmount { get; set; }

        // Osnovica na koju se racuna p ili t
        public decimal? AllowanceChargeBaseAmount { get; set; }

        // Podaci o porezu
        // Ukupan iznos poreza
        public decimal TaxTotalAmount { get; set; }

        // Rekapitulacija ukupnih iznosa
        // Ukupan neto iznos stavki sa vec uracunatim popustom ili troskom po stavkama bez poreza, popusta n.n.r. i trosarine n.n.r.
        public decimal LineExtensionAmount { get; set; }

        // Ukupan neto iznos stavki umanjen za popust n.n.r. i uvecan za trosarinu n.n.r., bez poreza
        public decimal TaxExclusiveAmount { get; set; }

        // Bruto iznos racuna, tj ukupan iznos stavki - popust n.n.r. + trosarina n.n.r.
        public decimal TaxInclusiveAmount { get; set; }

        // Ukupan iznos popusta n.n.r.
        public decimal AllowanceTotalAmount { get; set; }

        // Ukupan iznos troska n.n.r.
        public decimal? ChargeTotalAmount { get; set; }

        // Ukupan iznos avansne uplate
        public decimal? PrepaidAmount { get; set; }

        // Ukupan iznos racuna ( T.I.A. umanjen za prepaid amount ) 
        public decimal PayableAmount { get; set; }

        // Da li da se registruje u CRF
        public string RegisterToCRF { get; set; }
        
        public string? BillingReferenceID { get; set; }

        public DateTime? BillingReferenceIssueDate { get; set; }



        // Porezi
        public IEnumerable<EInvoiceTax> InvoiceTaxes { get; set; }

        // Stavke
        public IEnumerable<EInvoiceItem> InvoiceItems { get; set; }
    }

}
