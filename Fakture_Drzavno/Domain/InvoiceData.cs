using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakture_Drzavno.Domain
{
    internal class InvoiceData
    {
        public static async Task<EInvoice> GetDataAsync(string internalInvoiceID)
        {
            var connectionEnviroment = ConfigurationManager.AppSettings.Get("Connection");
            var connectionString = ConfigurationManager.ConnectionStrings[$"{connectionEnviroment}Connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var Get_Invoice_Data = $"SELECT " +
                $"[idFakture]" +
                $",[InvoiceID]" +
                $",[InvoiceIssueDate]" +
                $",[DueDate]" +
                $",[InvoiceTypeCode]" +
                $",[TimeNote]" +
                $",[OperaterNote]" +
                $",[ResponiblePersonNote]" +
                $",[DocumentNote]" +
                $",[DocumentCurrencyCode]" +
                $",[StartDate]" +
                $",[Endate]" +
                $",[DescriptionCode]" +
                $",[OrderReferenceID]" +
                $",[DispetchDocumentReferenceID]" +
                $",[OriginatorDocumentReferenceID]" +
                $",[ContractDocumentReferenceID]" +
                $",[AdditionalDocumentReferenceID]" +
                $",[FilePath]" +
                $",[PartySchmeID]" +
                $",[PartyEndPiont]" +
                $",[PartyidentificatioID]" +
                $",[PartyName]" +
                $",[PostalAdressStreatAdress]" +
                $",[PostalAdressCityName]" +
                $",[PostalAdressZone]" +
                $",[PostalAdressLine]" +
                $",[CoyntryIdentificationCode]" +
                $",[PartyTaxSchemeCompanyID]" +
                $",[PartyTaxSchemeID]" +
                $",[PartylegalEntityName]" +
                $",[PartylegalEntityCmpanyid]" +
                $",[PartylegalEntityForm]" +
                $",[ContactName]" +
                $",[Contacttel]" +
                $",[ContactEmail]" +
                $",[CustomerPartyEndPiont]" +
                $",[CustomerPartyidentificatioID]" +
                $",[CustomerPartyName]" +
                $",[CustomerPostalAdressStreatAdress]" +
                $",[CustomerPostalAdressCityName]" +
                $",[CustomerPostalAdressZone]" +
                $",[CustomerPostalAdressLine]" +
                $",[CustomerCoyntryIdentificationCode]" +
                $",[CustomerPartyTaxSchemeCompanyID]" +
                $",[CustomerPartyTaxSchemeID]" +
                $",[CustomerPartylegalEntityName]" +
                $",[CustomerPartylegalEntityCmpanyid]" +
                $",[CustomerPartylegalEntityForm]" +
                $",[CustomerContactName]" +
                $",[CustomerContacttel]" +
                $",[CustomerContactEmail]" +
                $",[Diliverydate]" +
                $",[DiliveryStreatName]" +
                $",[DiliveryCityName]" +
                $",[DiliveryPostalZone]" +
                $",[DiliveryContryIdentificationCode]" +
                $",[PaymentMeansCode]" +
                $",[PaymentInstructionNote]" +
                $",[PaymentMeansID]" +
                $",[PaymeeFinancialAccountID]" +
                $",[PaymentTermsNote]" +
                $",[AllowAnceChargeIndicator]" +
                $",[AllowAnceChargeReasen]" +
                $",[MultipleFactorNumeric]" +
                $",[AllowAnceChargeAmount]" +
                $",[AllowAnceBaseAmount]" +
                $",[TaxTotalAmount]" +
                $",[LineExtencionalAmount]" +
                $",[TaxExclusiveAmount]" +
                $",[TaxInclusiveAmount]" +
                $",[AlouenceTotalAmount]" +
                $",[CharrgeTotalAmount]" +
                $",[PrepayedAmount]" +
                $",[PaybleAmount]" +
                $",[FilePath2]" +
                $",[FilePath3]" +
                $",[RegistertoCRF]" +
                $" FROM [dbo].[{EInvoice.VIEW_NAME}] WHERE idFakture IN ({internalInvoiceID})";

            var Get_Invoice_Items_Data = $"SELECT [idfakture]" +
                $",[LineID]" +
                $",[InvoiceQuantiy]" +
                $",[LineExtencionalAmount]" +
                $",[AllowAnceChargeIndicator]" +
                $",[AllowAnceChargeReasen]" +
                $",[MultipleFactorNumeric]" +
                $",[AllowAnceChargeAmount]" +
                $",[AllowAnceBaseAmount]" +
                $",[TaxTotalAmount]" +
                $",[TaxSubTotaltaxableAmount]" +
                $",[TaxSubTotalAmount]" +
                $",[TaxCategotyID]" +
                $",[TaxCategotypervent]" +
                $",[TaxExemptionReason]" +
                $",[TaxSchemeID]" +
                $",[ItemName]" +
                $",[ClasifiedCategoryID]" +
                $",[ClasifiedPercentTax]" +
                $",[ClasifiedTaxExemptionReason]" +
                $",[ClasifiedTaxSchemeID]" +
                $",[PriceAmount]" +
                $",[BaseQuantity]" +
                $" FROM[dbo].[{EInvoiceItem.VIEW_NAME}] WHERE idFakture IN ({internalInvoiceID})";

            var Get_Invoice_Taxes_Data = $"SELECT [idfakture]" +
                $",[TaxSubTotaltaxableAmount]" +
                $",[TaxSubTotalAmount]" +
                $",[TaxCategotyID]" +
                $",[TaxCategotypervent]" +
                $",[TaxExemptionReason]" +
                $",[TaxSchemeID]" +
                $" FROM[dbo].[{EInvoiceTax.VIEW_NAME}] WHERE idFakture IN ({internalInvoiceID})";

            // INVOICES
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = Get_Invoice_Data;

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("[dbo].[Invoices]");
            sda.Fill(dt);

            // ITEMS
            SqlCommand cmd2 = connection.CreateCommand();
            cmd2.CommandText = Get_Invoice_Items_Data;

            SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable("[dbo].[InvoiceItems]");
            sda2.Fill(dt2);

            // TAXES
            SqlCommand cmd3 = connection.CreateCommand();
            cmd3.CommandText = Get_Invoice_Taxes_Data;

            SqlDataAdapter sda3 = new SqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable("[dbo].[InvoiceTaxes]");
            sda3.Fill(dt3);

            var invoiceResult = new EInvoice
            {
                InternalInvoiceID = Convert.ToString(dt.Rows[0].ItemArray[0]),
                InvoiceID = Convert.ToString(dt.Rows[0].ItemArray[1]),
                InvoiceIssueDate = Convert.ToDateTime(dt.Rows[0].ItemArray[2]),
                DueDate = Convert.ToDateTime(dt.Rows[0].ItemArray[3]),
                InvoiceTypeCode = Convert.ToInt32(dt.Rows[0].ItemArray[4]),
                TimeNote = Convert.ToString(dt.Rows[0].ItemArray[5]),
                OperatorNote = Convert.ToString(dt.Rows[0].ItemArray[6]),
                ResponsiblePersonNote = Convert.ToString(dt.Rows[0].ItemArray[7]),
                DocumentNote = Convert.IsDBNull(dt.Rows[0].ItemArray[8]) ? null : Convert.ToString(dt.Rows[0].ItemArray[8]),
                DocumentCurrencyCode = Convert.ToString(dt.Rows[0].ItemArray[9]),
                InvoicePeriodStartDate = Convert.ToDateTime(dt.Rows[0].ItemArray[10]),
                InvoicePeriodEndDate = Convert.ToDateTime(dt.Rows[0].ItemArray[11]),
                InvoicePeriodDescriptionCode = Convert.ToInt32(dt.Rows[0].ItemArray[12]),
                OrderReferenceID = Convert.IsDBNull(dt.Rows[0].ItemArray[13]) ? null : Convert.ToString(dt.Rows[0].ItemArray[13]),
                DespatchDocumentReferenceID = Convert.IsDBNull(dt.Rows[0].ItemArray[14]) ? null : Convert.ToString(dt.Rows[0].ItemArray[14]),
                OriginatorDocumentReferenceID = Convert.IsDBNull(dt.Rows[0].ItemArray[15]) ? null : Convert.ToString(dt.Rows[0].ItemArray[15]),
                ContractDocumentReferenceID = Convert.IsDBNull(dt.Rows[0].ItemArray[16]) ? null : Convert.ToString(dt.Rows[0].ItemArray[16]),
                AdditionalDocumentReferenceID = Convert.IsDBNull(dt.Rows[0].ItemArray[17]) ? null : Convert.ToString(dt.Rows[0].ItemArray[17]),
                FilePath = Convert.ToString(dt.Rows[0].ItemArray[18]),
                SupplierPartySchemeID = Convert.IsDBNull(dt.Rows[0].ItemArray[19]) ? (int?)null : Convert.ToInt32(dt.Rows[0].ItemArray[19]),
                SupplierPartyEndpointID = Convert.ToString(dt.Rows[0].ItemArray[20]),
                SupplierPartyIdentificationID = Convert.IsDBNull(dt.Rows[0].ItemArray[21]) ? null : Convert.ToString(dt.Rows[0].ItemArray[21]),
                SupplierPartyName = Convert.IsDBNull(dt.Rows[0].ItemArray[22]) ? null : Convert.ToString(dt.Rows[0].ItemArray[22]),
                SupplierPartyStreetName = Convert.IsDBNull(dt.Rows[0].ItemArray[23]) ? null : Convert.ToString(dt.Rows[0].ItemArray[23]),
                SupplierPartyCityName = Convert.IsDBNull(dt.Rows[0].ItemArray[24]) ? null : Convert.ToString(dt.Rows[0].ItemArray[24]),
                SupplierPartyPostalZone = Convert.IsDBNull(dt.Rows[0].ItemArray[25]) ? null : Convert.ToString(dt.Rows[0].ItemArray[25]),
                SupplierPartyPostalAddressLine = Convert.IsDBNull(dt.Rows[0].ItemArray[26]) ? null : Convert.ToString(dt.Rows[0].ItemArray[26]),
                SupplierPartyCountryIdentificationCode = Convert.IsDBNull(dt.Rows[0].ItemArray[27]) ? null : Convert.ToString(dt.Rows[0].ItemArray[27]),
                SupplierPartyCompanyID = Convert.ToString(dt.Rows[0].ItemArray[28]),
                SupplierPartyTaxSchemeID = Convert.ToString(dt.Rows[0].ItemArray[29]),
                SupplierPartyLegalEntityName = Convert.ToString(dt.Rows[0].ItemArray[30]),
                SupplierPartyLegalEntityCompanyID = Convert.ToString(dt.Rows[0].ItemArray[31]),
                SupplierPartyLegalEntityCompanyForm = Convert.IsDBNull(dt.Rows[0].ItemArray[32]) ? null : Convert.ToString(dt.Rows[0].ItemArray[32]),
                SupplierPartyContactName = Convert.IsDBNull(dt.Rows[0].ItemArray[33]) ? null : Convert.ToString(dt.Rows[0].ItemArray[33]),
                SupplierPartyContactPhone = Convert.IsDBNull(dt.Rows[0].ItemArray[34]) ? null : Convert.ToString(dt.Rows[0].ItemArray[34]),
                SupplierPartyContactElectronicMail = Convert.IsDBNull(dt.Rows[0].ItemArray[35]) ? null : Convert.ToString(dt.Rows[0].ItemArray[35]),
                CustomerPartyEndpointID = Convert.ToString(dt.Rows[0].ItemArray[36]),
                CustomerPartyIdentificationID = Convert.ToString(dt.Rows[0].ItemArray[37]),
                CustomerPartyName = Convert.IsDBNull(dt.Rows[0].ItemArray[38]) ? null : Convert.ToString(dt.Rows[0].ItemArray[38]),
                CustomerPartyStreetName = Convert.IsDBNull(dt.Rows[0].ItemArray[39]) ? null : Convert.ToString(dt.Rows[0].ItemArray[39]),
                CustomerPartyCityName = Convert.IsDBNull(dt.Rows[0].ItemArray[40]) ? null : Convert.ToString(dt.Rows[0].ItemArray[40]),
                CustomerPartyPostalZone = Convert.IsDBNull(dt.Rows[0].ItemArray[41]) ? null : Convert.ToString(dt.Rows[0].ItemArray[41]),
                CustomerPartyPostalAddressLine = Convert.IsDBNull(dt.Rows[0].ItemArray[42]) ? null : Convert.ToString(dt.Rows[0].ItemArray[42]),
                CustomerPartyCountryIdentificationCode = Convert.IsDBNull(dt.Rows[0].ItemArray[43]) ? null : Convert.ToString(dt.Rows[0].ItemArray[43]),
                CustomerPartyCompanyID = Convert.ToString(dt.Rows[0].ItemArray[44]),
                CustomerPartyTaxSchemeID = Convert.ToString(dt.Rows[0].ItemArray[45]),
                CustomerPartyLegalEntityName = Convert.ToString(dt.Rows[0].ItemArray[46]),
                CustomerPartyLegalEntityCompanyID = Convert.ToString(dt.Rows[0].ItemArray[47]),
                CustomerPartyLegalEntityCompanyForm = Convert.IsDBNull(dt.Rows[0].ItemArray[48]) ? null : Convert.ToString(dt.Rows[0].ItemArray[48]),
                CustomerPartyContactName = Convert.IsDBNull(dt.Rows[0].ItemArray[49]) ? null : Convert.ToString(dt.Rows[0].ItemArray[49]),
                CustomerPartyContactPhone = Convert.IsDBNull(dt.Rows[0].ItemArray[50]) ? null : Convert.ToString(dt.Rows[0].ItemArray[50]),
                CustomerPartyContactElectronicMail = Convert.IsDBNull(dt.Rows[0].ItemArray[51]) ? null : Convert.ToString(dt.Rows[0].ItemArray[51]),
                ActualDeliveryDate = Convert.IsDBNull(dt.Rows[0].ItemArray[52]) ? (DateTime?)null : Convert.ToDateTime(dt.Rows[0].ItemArray[52]),
                DeliveryStreetName = Convert.IsDBNull(dt.Rows[0].ItemArray[53]) ? null : Convert.ToString(dt.Rows[0].ItemArray[53]),
                DeliveryCityName = Convert.IsDBNull(dt.Rows[0].ItemArray[54]) ? null : Convert.ToString(dt.Rows[0].ItemArray[54]),
                DeliveryPostalZone = Convert.IsDBNull(dt.Rows[0].ItemArray[55]) ? (int?)null : Convert.ToInt32(dt.Rows[0].ItemArray[55]),
                DeliveryCountryIdentificationCode = Convert.IsDBNull(dt.Rows[0].ItemArray[56]) ? null : Convert.ToString(dt.Rows[0].ItemArray[56]),
                PaymentMeansCode = Convert.ToInt32(dt.Rows[0].ItemArray[57]),
                InstructionNote = Convert.IsDBNull(dt.Rows[0].ItemArray[58]) ? null : Convert.ToString(dt.Rows[0].ItemArray[58]),
                PaymentID = Convert.IsDBNull(dt.Rows[0].ItemArray[59]) ? null : Convert.ToString(dt.Rows[0].ItemArray[59]),
                PayeeFinancialAccountID = Convert.ToString(dt.Rows[0].ItemArray[60]),
                PaymentTermsNote = Convert.IsDBNull(dt.Rows[0].ItemArray[61]) ? null : Convert.ToString(dt.Rows[0].ItemArray[61]),
                AllowanceChargeIndicator = Convert.IsDBNull(dt.Rows[0].ItemArray[62]) ? (bool?)null : Convert.ToBoolean(dt.Rows[0].ItemArray[62]),
                AllowanceChargeReason = Convert.IsDBNull(dt.Rows[0].ItemArray[63]) ? null : Convert.ToString(dt.Rows[0].ItemArray[63]),
                AllowanceChargeMultiplierFactorNumeric = Convert.IsDBNull(dt.Rows[0].ItemArray[64]) ? (decimal?)null : Convert.ToDecimal(dt.Rows[0].ItemArray[64]),
                AllowanceChargeAmount = Convert.IsDBNull(dt.Rows[0].ItemArray[65]) ? (decimal?)null : Convert.ToDecimal(dt.Rows[0].ItemArray[65]),
                AllowanceChargeBaseAmount = Convert.IsDBNull(dt.Rows[0].ItemArray[66]) ? (decimal?)null : Convert.ToDecimal(dt.Rows[0].ItemArray[66]),
                TaxTotalAmount = Convert.ToDecimal(dt.Rows[0].ItemArray[67]),
                LineExtensionAmount = Convert.ToDecimal(dt.Rows[0].ItemArray[68]),
                TaxExclusiveAmount = Convert.ToDecimal(dt.Rows[0].ItemArray[69]),
                TaxInclusiveAmount = Convert.ToDecimal(dt.Rows[0].ItemArray[70]),
                AllowanceTotalAmount = Convert.IsDBNull(dt.Rows[0].ItemArray[71]) ? 0 : Convert.ToDecimal(dt.Rows[0].ItemArray[71]),
                ChargeTotalAmount = Convert.IsDBNull(dt.Rows[0].ItemArray[72]) ? (decimal?)null : Convert.ToDecimal(dt.Rows[0].ItemArray[72]),
                PrepaidAmount = Convert.IsDBNull(dt.Rows[0].ItemArray[73]) ? (decimal?)null : Convert.ToDecimal(dt.Rows[0].ItemArray[73]),
                PayableAmount = Convert.ToDecimal(dt.Rows[0].ItemArray[74]),
                SecondFilePath = Convert.IsDBNull(dt.Rows[0].ItemArray[75]) ? null : Convert.ToString(dt.Rows[0].ItemArray[75]),
                ThirdFilePath = Convert.IsDBNull(dt.Rows[0].ItemArray[76]) ? null : Convert.ToString(dt.Rows[0].ItemArray[76]),
                RegisterToCRF = Convert.IsDBNull(dt.Rows[0].ItemArray[77]) ? "Auto" : Convert.ToString(dt.Rows[0].ItemArray[77]),
            };

            var invoiceItemsResults = new List<EInvoiceItem>();
            for (var i = 0; i < dt2.Rows.Count; i++)
            {
                invoiceItemsResults.Add(new EInvoiceItem
                {
                    InternalInvoiceID = Convert.ToString(dt2.Rows[i].ItemArray[0]),
                    LineID = Convert.ToInt32(dt2.Rows[i].ItemArray[1]),
                    InvoicedQuantity = Convert.ToInt32(dt2.Rows[i].ItemArray[2]),
                    LineExtensionAmount = Convert.ToDecimal(dt2.Rows[i].ItemArray[3]),
                    InvoiceLineChargeIndicator = Convert.IsDBNull(dt2.Rows[i].ItemArray[4]) ? (bool?)null : Convert.ToBoolean(dt2.Rows[i].ItemArray[4]),
                    InvoiceLineAllowanceChargeReason = Convert.IsDBNull(dt2.Rows[i].ItemArray[5]) ? null : Convert.ToString(dt2.Rows[i].ItemArray[5]),
                    InvoiceLineMultiplierFactorNumeric = Convert.IsDBNull(dt2.Rows[i].ItemArray[6]) ? (decimal?)null : Convert.ToDecimal(dt2.Rows[i].ItemArray[6]),
                    InvoiceLineAmount = Convert.IsDBNull(dt2.Rows[i].ItemArray[7]) ? (decimal?)null : Convert.ToDecimal(dt2.Rows[i].ItemArray[7]),
                    InvoiceLineBaseAmount = Convert.IsDBNull(dt2.Rows[i].ItemArray[8]) ? (decimal?)null : Convert.ToDecimal(dt2.Rows[i].ItemArray[8]),
                    TaxTotalAmount = Convert.ToDecimal(dt2.Rows[i].ItemArray[9]),
                    TaxSubtotalTaxableAmount = Convert.ToDecimal(dt2.Rows[i].ItemArray[10]),
                    TaxSubtotalAmount = Convert.ToDecimal(dt2.Rows[i].ItemArray[11]),
                    TaxSubtotalCategoryID = Convert.ToString(dt2.Rows[i].ItemArray[12]),
                    TaxSubtotalPercent = Convert.ToDecimal(dt2.Rows[i].ItemArray[13]),
                    TaxSubtotalExemptionReason = Convert.IsDBNull(dt2.Rows[i].ItemArray[14]) ? null : Convert.ToString(dt2.Rows[i].ItemArray[14]),
                    TaxSubtotalSchemeID = Convert.IsDBNull(dt2.Rows[i].ItemArray[15]) ? null : Convert.ToString(dt2.Rows[i].ItemArray[15]),
                    InvoiceLineItemName = Convert.ToString(dt2.Rows[i].ItemArray[16]),
                    ClassifiedTaxCategoryID = Convert.ToString(dt2.Rows[i].ItemArray[17]),
                    ClassifiedTaxPercent = Convert.ToDecimal(dt2.Rows[i].ItemArray[18]),
                    ClassifiedTaxExemptionReason = Convert.IsDBNull(dt2.Rows[i].ItemArray[19]) ? null : Convert.ToString(dt2.Rows[i].ItemArray[19]),
                    ClassifiedTaxSchemeID = Convert.ToString(dt2.Rows[i].ItemArray[20]),
                    PriceAmount = Convert.IsDBNull(dt2.Rows[i].ItemArray[21]) ? 1 : Convert.ToDecimal(dt2.Rows[i].ItemArray[21]),
                    BaseQuantity = Convert.IsDBNull(dt2.Rows[i].ItemArray[22]) ? 1 : Convert.ToInt32(dt2.Rows[i].ItemArray[22])
                });
            }

            var invoicesTaxes = new List<EInvoiceTax>();
            for (var i = 0; i < dt3.Rows.Count; i++)
            {
                invoicesTaxes.Add(new EInvoiceTax
                {
                    InternalInvoiceID = Convert.ToString(dt3.Rows[i].ItemArray[0]),
                    TaxSubtotalTaxableAmount = Convert.ToDecimal(dt3.Rows[i].ItemArray[1]),
                    TaxSubtotalAmount = Convert.ToDecimal(dt3.Rows[i].ItemArray[2]),
                    TaxSubtotalCategoryID = Convert.ToString(dt3.Rows[i].ItemArray[3]),
                    TaxSubtotalPercent = Convert.ToDecimal(dt3.Rows[i].ItemArray[4]),
                    TaxSubtotalExemptionReason = Convert.IsDBNull(dt3.Rows[i].ItemArray[5]) ? null : Convert.ToString(dt3.Rows[i].ItemArray[5]),
                    TaxSubtotalSchemeID = Convert.ToString(dt3.Rows[i].ItemArray[6]),
                });
            }

            invoiceResult.InvoiceItems = invoiceItemsResults.Where(x => x.InternalInvoiceID == invoiceResult.InternalInvoiceID).ToList();
            invoiceResult.InvoiceTaxes = invoicesTaxes.Where(x => x.InternalInvoiceID == invoiceResult.InternalInvoiceID).ToList();

            await connection.CloseAsync();

            return invoiceResult;
        }

    }
}
