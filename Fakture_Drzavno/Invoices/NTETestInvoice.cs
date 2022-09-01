using Fakture_Drzavno.Domain;
using Fakture_Drzavno.Invoices.Elements;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fakture_Drzavno.Invoices
{
    internal class NTETestInvoice
    {
        public static async Task<XmlDocument> Generate()
        {
            var invoice = CreateTestInvoice();

            XmlDocument document = new XmlDocument();

            XmlDeclaration xmlDeclaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);

            XmlElement root = document.DocumentElement;
            document.InsertBefore(xmlDeclaration, root);

            // Invoice
            XmlElement rootElement = document.CreateElement(string.Empty, "Invoice", string.Empty);
            rootElement.SetAttribute("xmlns:cec", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            rootElement.SetAttribute("xmlns:cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            rootElement.SetAttribute("xmlns:cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            rootElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootElement.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            rootElement.SetAttribute("xmlns:sbt", "http://mfin.gov.rs/srbdt/srbdtext");
            rootElement.SetAttribute("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            document.AppendChild(rootElement);

            // CustomizationID
            InvoiceCreator.CreateSimpleElement(ref document, ref rootElement, new SimpleInvoiceElement { Name = "CustomizationID", Value = "urn:cen.eu:en16931:2017#compliant#urn:mfin.gov.rs:srbdt:2021" });

            // Broj avansnog racuna
            InvoiceCreator.CreateSimpleElement(ref document, ref rootElement, new SimpleInvoiceElement { Name = "ID", Value = invoice.InvoiceID });

            // Datum izdavanja racuna
            InvoiceCreator.CreateSimpleElement(ref document, ref rootElement, new SimpleInvoiceElement { Name = "IssueDate", Value = DateTime.Now.ToString("yyyy-MM-dd") });

            // Valuta placanja
            InvoiceCreator.CreateSimpleElement(ref document, ref rootElement, new SimpleInvoiceElement { Name = "DueDate", Value = invoice.DueDate.ToString("yyyy-MM-dd") });

            // Medjunarodna oznaka racuna
            InvoiceCreator.CreateSimpleElement(ref document, ref rootElement, new SimpleInvoiceElement { Name = "InvoiceTypeCode", Value = invoice.InvoiceTypeCode.ToString() });

            // Novcana valuta - za dinar je RSD
            InvoiceCreator.CreateSimpleElement(ref document, ref rootElement, new SimpleInvoiceElement { Name = "DocumentCurrencyCode", Value = invoice.DocumentCurrencyCode });

            // Obracunski period
            InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "InvoicePeriod",
                Elements = new List<InvoiceElement>
                {
                    // Kod koji se odnosi na taj racun definisan po standardu ( 3, 35, 432 ) 35
                    new SimpleInvoiceElement { Name = "DescriptionCode", Value = invoice.InvoicePeriodDescriptionCode.ToString() }
                }
            });

            // Polje za unos oznake ugovora / NULLABLE
            InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "ContractDocumentReference",
                Elements = new List<InvoiceElement>
                {
                    new SimpleInvoiceElement { Name = "ID", Value = invoice.ContractDocumentReferenceID }
                }
            });

            // PDF Vizualizacija racuna
            var bytes = File.ReadAllBytes(invoice.FilePath);
            var file = Convert.ToBase64String(bytes);

            InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "AdditionalDocumentReference",
                Elements = new List<InvoiceElement>
                {
                    // ID Dokumenta
                    new SimpleInvoiceElement { Name = "ID", Value = invoice.AdditionalDocumentReferenceID },

                    new ComplexInvoiceElement {
                        Name = "Attachment",
                        Elements = new List<InvoiceElement>
                        {
                            new SimpleInvoiceElement {
                                Name = "EmbeddedDocumentBinaryObject",
                                Value = file,
                                Attributes = new List<InvoiceElementAttribute> {
                                    new InvoiceElementAttribute { Name = "mimeCode", Value = "application/pdf" },
                                    new InvoiceElementAttribute { Name = "filename", Value = $"{invoice.InvoiceID}.pdf" },
                                },
                            }
                        }
                    }
                }
            });

            if (!String.IsNullOrEmpty(invoice.SecondFilePath))
            {
                // PDF Vizualizacija racuna 2
                var bytes2 = File.ReadAllBytes(invoice.SecondFilePath);
                var file2 = Convert.ToBase64String(bytes2);

                InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
                {
                    Name = "AdditionalDocumentReference",
                    Elements = new List<InvoiceElement>
                    {
                        // ID Dokumenta
                        new SimpleInvoiceElement { Name = "ID", Value = invoice.AdditionalDocumentReferenceID },

                        new ComplexInvoiceElement {
                            Name = "Attachment",
                            Elements = new List<InvoiceElement>
                            {
                                new SimpleInvoiceElement {
                                    Name = "EmbeddedDocumentBinaryObject",
                                    Value = file2,
                                    Attributes = new List<InvoiceElementAttribute> {
                                        new InvoiceElementAttribute { Name = "mimeCode", Value = "application/pdf" },
                                        new InvoiceElementAttribute { Name = "filename", Value = $"{invoice.InvoiceID}_2.pdf" },
                                    },
                                }
                            }
                        }
                    }
                });
            }

            if (!String.IsNullOrEmpty(invoice.ThirdFilePath))
            {
                // PDF Vizualizacija racuna 3
                var bytes3 = File.ReadAllBytes(invoice.ThirdFilePath);
                var file3 = Convert.ToBase64String(bytes3);

                InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
                {
                    Name = "AdditionalDocumentReference",
                    Elements = new List<InvoiceElement>
                    {
                        // ID Dokumenta
                        new SimpleInvoiceElement { Name = "ID", Value = invoice.AdditionalDocumentReferenceID },

                        new ComplexInvoiceElement {
                            Name = "Attachment",
                            Elements = new List<InvoiceElement>
                            {
                                new SimpleInvoiceElement {
                                    Name = "EmbeddedDocumentBinaryObject",
                                    Value = file3,
                                    Attributes = new List<InvoiceElementAttribute> {
                                        new InvoiceElementAttribute { Name = "mimeCode", Value = "application/pdf" },
                                        new InvoiceElementAttribute { Name = "filename", Value = $"{invoice.InvoiceID}_3.pdf" },
                                    },
                                }
                            }
                        }
                    }
                });
            }

            // Podaci o posiljaocu
            InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "AccountingSupplierParty",
                Elements = new List<InvoiceElement>
                {
                    new ComplexInvoiceElement {
                        Name = "Party",
                        Elements = new List<InvoiceElement>
                        {
                            // Elektronska adresa pošiljaoca. Ako je pošiljalac ima registrovanu, koristi se schemeID="0088" u kombinaciji sa GLN-om.U suprotnom, koristi se šifra države (9948 je šifra za Srbiju) u kombinaciji sa PIB-om dobavljača / NULLABLE
                            new SimpleInvoiceElement {
                                Name = "EndpointID",
                                Value = invoice.SupplierPartyEndpointID,
                                Attributes = new List<InvoiceElementAttribute> {
                                    new InvoiceElementAttribute { Name = "schemeID", Value = invoice.SupplierPartySchemeID == null ? "" : invoice.SupplierPartySchemeID.ToString() }
                                },
                            },

                            // U polju ID ovog čvora se upisuje šifra države sa PIB-om primaoca (9948:PIB), ako je u pitanju javna ustanova onda se unosi JBKJS:BROJ / NULLABLE
                            new ComplexInvoiceElement {
                                Name = "PartyIdentification",
                                Elements = new List<InvoiceElement>
                                {
                                    new SimpleInvoiceElement { Name = "ID", Value = invoice.SupplierPartyIdentificationID }
                                }
                            },

                            // Naziv subjekta posiljaoca / NULLABLE
                            new ComplexInvoiceElement {
                                Name = "PartyName",
                                Elements = new List<InvoiceElement>
                                {
                                    new SimpleInvoiceElement { Name = "Name", Value = invoice.SupplierPartyName }
                                }
                            },

                            // Podaci o adresi posiljaoca
                            new ComplexInvoiceElement {
                                Name = "PostalAddress",
                                Elements = new List<InvoiceElement>
                                {
                                    // Sediste ( grad ) posiljaoca / NULLABLE
                                    new SimpleInvoiceElement { Name = "CityName", Value = invoice.SupplierPartyCityName },
                                    
                                    // Sediste ( drzava ) dobavljaca / NULLABLE
                                    new ComplexInvoiceElement {
                                        Name = "Country",
                                        Elements = new List<InvoiceElement>
                                        {
                                            // Oznaka drzave posiljaoca prema sifarniku. Za Srbiju je RS / NULLABLE
                                            new SimpleInvoiceElement { Name = "IdentificationCode", Value = invoice.SupplierPartyCountryIdentificationCode?.ToString() }
                                        }
                                    },
                                }
                            },

                            // Čvor je obavezan kada su u pitanju kategorije poreza PDV, oslobođeno poreza i PPO
                            new ComplexInvoiceElement {
                                Name = "PartyTaxScheme",
                                Elements = new List<InvoiceElement>
                                {
                                    // RSPIB pravnog lica A
                                    new SimpleInvoiceElement { Name = "CompanyID", Value = invoice.SupplierPartyCompanyID },

                                    // Ime poreske kategorije - VAT
                                    new ComplexInvoiceElement {
                                        Name = "TaxScheme",
                                        Elements = new List<InvoiceElement>
                                        {
                                            // Puna adresa posiljaoca / NULLABLE
                                            new SimpleInvoiceElement { Name = "ID", Value = invoice.SupplierPartyTaxSchemeID }
                                        }
                                    },
                                }
                            },

                            // Zvanicni podaci ( iz APR-a )
                            new ComplexInvoiceElement {
                                Name = "PartyLegalEntity",
                                Elements = new List<InvoiceElement>
                                {
                                    // Naziv subjekta posiljaoca
                                    new SimpleInvoiceElement { Name = "RegistrationName", Value = invoice.SupplierPartyLegalEntityName },
                                    
                                    // Maticni broj subjekta posiljaoca
                                    new SimpleInvoiceElement { Name = "CompanyID", Value = invoice.SupplierPartyLegalEntityCompanyID }
                                }
                            }
                        }
                    }
                }
            });

            // Podaci o primaocu
            InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "AccountingCustomerParty",
                Elements = new List<InvoiceElement>
                {
                    new ComplexInvoiceElement {
                        Name = "Party",
                        Elements = new List<InvoiceElement>
                        {
                            // Elektronska adresa primaoca. Ako je primaoc ima registrovanu, koristi se schemeID="0088" u kombinaciji sa GLN-om.U suprotnom, koristi se šifra države (9948 je šifra za Srbiju) u kombinaciji sa PIB-om primaoca / NULLABLE
                            new SimpleInvoiceElement {
                                Name = "EndpointID",
                                Value = invoice.CustomerPartyEndpointID,
                                Attributes = new List<InvoiceElementAttribute> {
                                    new InvoiceElementAttribute { Name = "schemeID", Value = "9948" }
                                },
                            },

                            // U polju ID ovog čvora se upisuje šifra države sa PIB-om primaoca (9948:PIB), ako je u pitanju javna ustanova onda se unosi JBKJS:BROJ / NULLABLE
                            new ComplexInvoiceElement {
                                Name = "PartyIdentification",
                                Elements = new List<InvoiceElement>
                                {
                                    new SimpleInvoiceElement { Name = "ID", Value = invoice.CustomerPartyIdentificationID }
                                }
                            },

                            // Naziv subjekta primaoca / NULLABLE
                            new ComplexInvoiceElement {
                                Name = "PartyName",
                                Elements = new List<InvoiceElement>
                                {
                                    new SimpleInvoiceElement { Name = "Name", Value = invoice.CustomerPartyName }
                                }
                            },

                            // Podaci o adresi primaoca
                            new ComplexInvoiceElement {
                                Name = "PostalAddress",
                                Elements = new List<InvoiceElement>
                                {
                                    // Registrovana adresa primaoca / NULLABLE
                                    new SimpleInvoiceElement { Name = "StreetName", Value = invoice.CustomerPartyStreetName },
                                    
                                    // Sediste ( grad ) primaoca / NULLABLE
                                    new SimpleInvoiceElement { Name = "CityName", Value = invoice.CustomerPartyCityName },
                                    
                                    // Postanski broj sedista primaoca / NULLABLE
                                    new SimpleInvoiceElement { Name = "PostalZone", Value = invoice.CustomerPartyPostalZone },

                                    // Sediste ( drzava ) primaoca / NULLABLE
                                    new ComplexInvoiceElement {
                                        Name = "Country",
                                        Elements = new List<InvoiceElement>
                                        {
                                            // Oznaka drzave primaoca prema sifarniku. Za Srbiju je RS / NULLABLE
                                            new SimpleInvoiceElement { Name = "IdentificationCode", Value = invoice.CustomerPartyCountryIdentificationCode }
                                        }
                                    },
                                }
                            },

                            // Čvor je obavezan kada su u pitanju kategorije poreza PDV, oslobođeno poreza i PPO / NULLABLE
                            new ComplexInvoiceElement {
                                Name = "PartyTaxScheme",
                                Elements = new List<InvoiceElement>
                                {
                                    // PIB Subjekta primaoca  / NULLABLE
                                    new SimpleInvoiceElement { Name = "CompanyID", Value = invoice.CustomerPartyCompanyID },

                                    // Ime poreske kategorije - VAT / NULLABLE
                                    new ComplexInvoiceElement {
                                        Name = "TaxScheme",
                                        Elements = new List<InvoiceElement>
                                        {
                                            new SimpleInvoiceElement { Name = "ID", Value = invoice.CustomerPartyTaxSchemeID }
                                        }
                                    },
                                }
                            },

                            // Zvanicni podaci ( iz APR-a )
                            new ComplexInvoiceElement {
                                Name = "PartyLegalEntity",
                                Elements = new List<InvoiceElement>
                                {
                                    // Naziv subjekta primaoca
                                    new SimpleInvoiceElement { Name = "RegistrationName", Value = invoice.CustomerPartyLegalEntityName },
                                    
                                    // Maticni broj subjekta primaoca
                                    new SimpleInvoiceElement { Name = "CompanyID", Value = invoice.CustomerPartyLegalEntityCompanyID }
                                }
                            }
                        }
                    }
                }
            });

            // Podaci o placanju
            InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "Delivery",
                Elements = new List<InvoiceElement>
                {
                    // Sifra nacina placanja
                    new SimpleInvoiceElement { Name = "ActualDeliveryDate", Value = invoice.ActualDeliveryDate.HasValue ? invoice.ActualDeliveryDate.Value.ToString("yyyy-MM-dd") : null },
                }
            });

            // Podaci o placanju
            InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "PaymentMeans",
                Elements = new List<InvoiceElement>
                {
                    // Sifra nacina placanja
                    new SimpleInvoiceElement { Name = "PaymentMeansCode", Value = invoice.PaymentMeansCode.ToString() },
                    
                    // Model placanja i poziv na broj placanja / NULLABLE
                    new SimpleInvoiceElement { Name = "PaymentID", Value = invoice.PaymentID },

                    // Podaci o bankovnom racunu posiljaoca
                    new ComplexInvoiceElement {
                        Name = "PayeeFinancialAccount",
                        Elements = new List<InvoiceElement>
                        {
                            new SimpleInvoiceElement { Name = "ID", Value = invoice.PayeeFinancialAccountID }
                        }
                    }
                }
            });

            // Unos podataka o popustu ili trosku na nivou dokumenta / NULLABLE
            var taxElement = InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "TaxTotal",
                Elements = new List<InvoiceElement>
                {
                    // Ukupan iznos poreza
                    new SimpleInvoiceElement { Name = "TaxAmount", Value = invoice.TaxTotalAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},
                }
            });

            // Porezi
            foreach (var invoiceTax in invoice.InvoiceTaxes)
            {
                InvoiceCreator.CreateComplexElement(ref document, ref taxElement, new ComplexInvoiceElement
                {
                    Name = "TaxSubtotal",
                    Elements = new List<InvoiceElement>
                    {
                        // Ukupna osnovica poreske stope
                        new SimpleInvoiceElement { Name = "TaxableAmount", Value = invoiceTax.TaxSubtotalTaxableAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},

                        // Ukupan iznos obracunatog poreza
                        new SimpleInvoiceElement { Name = "TaxAmount", Value = invoiceTax.TaxSubtotalAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},

                        // Poreska kategorija
                        new ComplexInvoiceElement {
                            Name = "TaxCategory",
                            Elements = new List<InvoiceElement>
                            {
                                // ID poreske kategorije
                                new SimpleInvoiceElement { Name = "ID", Value = invoiceTax.TaxSubtotalCategoryID.ToString(), Attributes = new List<InvoiceElementAttribute> {
                                    new InvoiceElementAttribute {
                                        Name = "schemeID",
                                        Value = "UN/ECE 5305"
                                    },
                                    new InvoiceElementAttribute {
                                        Name = "schemeAgencyID",
                                        Value = "6"
                                    },
                                    new InvoiceElementAttribute {
                                        Name = "schemeURI",
                                        Value = "http://www.unece.org/trade/untdid/d07a/tred/tred5305.htm"
                                    },
                                }},

                                // Procenat poreza
                                new SimpleInvoiceElement { Name = "Percent", Value = invoiceTax.TaxSubtotalPercent.ToString("#.##") },

                                // Ime poreske kategorije - VAT
                                new ComplexInvoiceElement {
                                    Name = "TaxScheme",
                                    Elements = new List<InvoiceElement>
                                    {
                                        new SimpleInvoiceElement { Name = "ID", Value = invoiceTax.TaxSubtotalSchemeID }
                                    }
                                },
                            }
                        }
                    }
                });
            }

            // Rekapitulacija ukupnih iznosa
            InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
            {
                Name = "LegalMonetaryTotal",
                Elements = new List<InvoiceElement>
                {
                    // Ukupan neto iznos stavki (sa već uračunatim popustom po stavkama) bez poreza, popusta na nivou računa i trošarine na nivou računa
                    new SimpleInvoiceElement { Name = "LineExtensionAmount", Value = invoice.LineExtensionAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},

                    // Ukupan neto iznos stavki umanjen za popust na nivou računa i uvećan za trošarinu na nivou računa, bez poreza 
                    new SimpleInvoiceElement { Name = "TaxExclusiveAmount", Value = invoice.TaxExclusiveAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},

                    // Bruto iznos računa, tj. ukupan iznos stavki - popust na nivou računa + trošarina na nivou računa + porez
                    new SimpleInvoiceElement { Name = "TaxInclusiveAmount", Value = invoice.TaxInclusiveAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},

                    // Ukupan iznos popusta na nivou racuna
                    new SimpleInvoiceElement { Name = "AllowanceTotalAmount", Value = invoice.AllowanceTotalAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},

                    // Ukupan iznos avansne uplate / NULLABLE
                    new SimpleInvoiceElement { Name = "PrepaidAmount", Value = invoice.PrepaidAmount.HasValue ? invoice.PrepaidAmount.Value.ToString("#.##") : "", Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},

                    // Ukupan iznos racuna ( TaxInclusiveAmount ) za uplatu - umanjen za PrepaidAmount
                    new SimpleInvoiceElement { Name = "PayableAmount", Value = invoice.PayableAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},
                }
            });

            foreach (var item in invoice.InvoiceItems)
            {
                // Stavka
                InvoiceCreator.CreateComplexElement(ref document, ref rootElement, new ComplexInvoiceElement
                {
                    Name = "InvoiceLine",
                    Elements = new List<InvoiceElement>
                    {
                        // Redni broj stavke u racunu
                        new SimpleInvoiceElement { Name = "ID", Value = item.LineID.ToString() },

                        // Kolicina
                        new SimpleInvoiceElement { Name = "InvoicedQuantity", Value = item.InvoicedQuantity.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "unitCode", Value = "H87" }}},

                        // Iznos stavke bez poreza umanjen za popust i uvećan za trošarinu
                        new SimpleInvoiceElement { Name = "LineExtensionAmount", Value = item.LineExtensionAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}},

                        // Period / NULLABLE
                        new ComplexInvoiceElement {
                            Name = "InvoicePeriod",
                            Elements = new List<InvoiceElement>
                            {
                                // NULLABLE
                                new SimpleInvoiceElement { Name = "StartDate", Value = !item.InvoiceLineStartDate.HasValue ? null : item.InvoiceLineStartDate.Value.ToString("yyyy-MM-dd") },

                                // NULLABLE
                                new SimpleInvoiceElement { Name = "EndDate", Value = !item.InvoiceLineEndDate.HasValue ? null : item.InvoiceLineEndDate.Value.ToString("yyyy-MM-dd") },
                            }
                        },

                        // Podaci o artiklu ili usluzi
                        new ComplexInvoiceElement {
                            Name = "Item",
                            Elements = new List<InvoiceElement>
                            {
                                // Ime artikla ili usluge
                                new SimpleInvoiceElement { Name = "Name", Value = item.InvoiceLineItemName.ToString() },

                                // Sifra artikla od posiljaoca / NULLABLE
                                new ComplexInvoiceElement {
                                    Name = "SellersItemIdentification",
                                    Elements = new List<InvoiceElement>
                                    {
                                        new SimpleInvoiceElement { Name = "ID", Value = null }
                                    }
                                },

                                // Podaci o porezu po svakoj stavci
                                new ComplexInvoiceElement {
                                    Name = "ClassifiedTaxCategory",
                                    Elements = new List<InvoiceElement>
                                    {
                                        // ID poreske kategorije S / E
                                        new SimpleInvoiceElement { Name = "ID", Value = item.ClassifiedTaxCategoryID },

                                        // Procenat poreza (tag se ne navodi kod kategorije poreza NEOPOREZIVO)
                                        new SimpleInvoiceElement { Name = "Percent", Value = item.ClassifiedTaxPercent.ToString("#.##") },

                                        // NULLABLE
                                        new SimpleInvoiceElement { Name = "TaxExemptionReason", Value = item.ClassifiedTaxExemptionReason },

                                        // Ime poreske kategorije - VAT
                                        new ComplexInvoiceElement {
                                            Name = "TaxScheme",
                                            Elements = new List<InvoiceElement>
                                            {
                                                new SimpleInvoiceElement { Name = "ID", Value = item.ClassifiedTaxSchemeID }
                                            }
                                        },
                                    }
                                }
                            }
                        },

                        new ComplexInvoiceElement {
                            Name = "Price",
                            Elements = new List<InvoiceElement>
                            {
                                // Jedinicna cena artikla ili usluge
                                new SimpleInvoiceElement { Name = "PriceAmount", Value = item.PriceAmount.ToString("#.##"), Attributes = new List<InvoiceElementAttribute> { new InvoiceElementAttribute { Name = "currencyID", Value = "RSD" }}}
                            }
                        },
                    }
                });
            }

            var saveXml = Boolean.Parse(ConfigurationManager.AppSettings.Get("SaveXML"));
            var xmlLocation = ConfigurationManager.AppSettings.Get("XMLSavingLocation");

            if (saveXml)
            {
                document.Save(xmlLocation + $"/document_faktura_{invoice.InternalInvoiceID}.xml");
            }
            return document;
        }

        public static EInvoice CreateTestInvoice()
        {
            var invoiceResult = new EInvoice
            {
                InternalInvoiceID = "0001",
                InvoiceID = "1/0001",
                InvoiceIssueDate = DateTime.Now,
                DueDate = DateTime.Now,
                InvoiceTypeCode = 380,
                TimeNote = null,
                OperatorNote = "Srdjan Vukovic",
                ResponsiblePersonNote = "Srdjan Vukovic",
                DocumentNote = "Ugovor :  IV-9/22 od 12.01.2022",
                DocumentCurrencyCode = "RSD",
                InvoicePeriodStartDate = DateTime.Now,
                InvoicePeriodEndDate = DateTime.Now,
                InvoicePeriodDescriptionCode = 35,
                OrderReferenceID = null,
                DespatchDocumentReferenceID = null,
                OriginatorDocumentReferenceID = null,
                ContractDocumentReferenceID = null,
                AdditionalDocumentReferenceID = "1/0001",
                FilePath = "testfile.pdf",
                SupplierPartySchemeID = 9948,
                SupplierPartyEndpointID = "101684773",
                SupplierPartyIdentificationID = null,
                SupplierPartyName = null,
                SupplierPartyStreetName = null,
                SupplierPartyCityName = null,
                SupplierPartyPostalZone = null,
                SupplierPartyPostalAddressLine = null,
                SupplierPartyCountryIdentificationCode = "RS",
                SupplierPartyCompanyID = "RS101684773",
                SupplierPartyTaxSchemeID = "VAT",
                SupplierPartyLegalEntityName = "NTE ENGINEERING DOO",
                SupplierPartyLegalEntityCompanyID = "17331213",
                SupplierPartyLegalEntityCompanyForm = null,
                SupplierPartyContactName = null,
                SupplierPartyContactPhone = null,
                SupplierPartyContactElectronicMail = null,
                CustomerPartyEndpointID = "101684773",
                CustomerPartyIdentificationID = null,
                CustomerPartyName = null,
                CustomerPartyStreetName = null,
                CustomerPartyCityName = null,
                CustomerPartyPostalZone = null,
                CustomerPartyPostalAddressLine = null,
                CustomerPartyCountryIdentificationCode = null,
                CustomerPartyCompanyID = "RS101684773",
                CustomerPartyTaxSchemeID = "VAT",
                CustomerPartyLegalEntityName = "NTE ENGINEERING DOO",
                CustomerPartyLegalEntityCompanyID = "17331213",
                CustomerPartyLegalEntityCompanyForm = null,
                CustomerPartyContactName = null,
                CustomerPartyContactPhone = null,
                CustomerPartyContactElectronicMail = null,
                ActualDeliveryDate = DateTime.Now,
                DeliveryStreetName = null,
                DeliveryCityName = null,
                DeliveryPostalZone = null,
                DeliveryCountryIdentificationCode = null,
                PaymentMeansCode = 42,
                InstructionNote = null,
                PaymentID = null,
                PayeeFinancialAccountID = "205-23809-53",
                PaymentTermsNote = null,
                AllowanceChargeIndicator = null,
                AllowanceChargeReason = null,
                AllowanceChargeMultiplierFactorNumeric = null,
                AllowanceChargeAmount = null,
                AllowanceChargeBaseAmount = null,
                TaxTotalAmount = 5000,
                LineExtensionAmount = 25000,
                TaxExclusiveAmount = 25000,
                TaxInclusiveAmount = 30000,
                AllowanceTotalAmount = 0,
                ChargeTotalAmount = null,
                PrepaidAmount = null,
                PayableAmount = 30000,
                SecondFilePath = null,
                ThirdFilePath = null,
            };

            var invoiceItemsResults = new List<EInvoiceItem>();
            for (var i = 0; i < 1; i++)
            {
                invoiceItemsResults.Add(new EInvoiceItem
                {
                    InternalInvoiceID = "0001",
                    LineID = 1,
                    InvoicedQuantity = 1,
                    LineExtensionAmount = 25000,
                    InvoiceLineChargeIndicator = null,
                    InvoiceLineAllowanceChargeReason = null,
                    InvoiceLineMultiplierFactorNumeric = null,
                    InvoiceLineAmount = null,
                    InvoiceLineBaseAmount = null,
                    TaxTotalAmount = 5000,
                    TaxSubtotalTaxableAmount = 25000,
                    TaxSubtotalAmount = 25000,
                    TaxSubtotalCategoryID = "S",
                    TaxSubtotalPercent = 20,
                    TaxSubtotalExemptionReason = null,
                    TaxSubtotalSchemeID = null,
                    InvoiceLineItemName = "NTE Test",
                    ClassifiedTaxCategoryID = "S",
                    ClassifiedTaxPercent = 20,
                    ClassifiedTaxExemptionReason = null,
                    ClassifiedTaxSchemeID = "VAT",
                    PriceAmount = 25000,
                    BaseQuantity = 1
                });
            }

            var invoicesTaxes = new List<EInvoiceTax>();
            for (var i = 0; i < 1; i++)
            {
                invoicesTaxes.Add(new EInvoiceTax
                {
                    InternalInvoiceID = "0001",
                    TaxSubtotalTaxableAmount = 25000,
                    TaxSubtotalAmount = 5000,
                    TaxSubtotalCategoryID = "S",
                    TaxSubtotalPercent = 20,
                    TaxSubtotalExemptionReason = null,
                    TaxSubtotalSchemeID = "VAT",
                });
            }

            invoiceResult.InvoiceItems = invoiceItemsResults.Where(x => x.InternalInvoiceID == invoiceResult.InternalInvoiceID).ToList();
            invoiceResult.InvoiceTaxes = invoicesTaxes.Where(x => x.InternalInvoiceID == invoiceResult.InternalInvoiceID).ToList();

            return invoiceResult;
        }

    }
}
