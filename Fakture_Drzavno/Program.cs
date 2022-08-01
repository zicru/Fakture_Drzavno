using Fakture_Drzavno.Contracts;
using Fakture_Drzavno.Contracts.Models;
using Fakture_Drzavno.Domain;
using Fakture_Drzavno.Invoices;
using Fakture_Drzavno.Settings;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace Fakture_Drzavno
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var mainArgument = args[0];

            var documentType = "";
            var hasDocumentTypeOption = false;

            var operation = "";
            var hasOperationOption = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-t")
                {
                    hasDocumentTypeOption = true;
                    documentType = args[i + 1];
                }

                if (args[i] == "-o")
                {
                    hasOperationOption = true;
                    operation = args[i + 1];
                }
            }

            // Document type checking
            if (!hasDocumentTypeOption && operation != OperationsArgumentValues.STATUS_IZDATE_FAKTURE)
            {
                Console.WriteLine($"Nije uneta opcija za tip fakture (-t). Podrazumevan tip \"faktura\"");
                documentType = "faktura";
            }

            // Add check if its the right type
            if (!DocumentTypesArgumentValues.Values.Contains(documentType) && operation != OperationsArgumentValues.STATUS_IZDATE_FAKTURE)
            {
                Console.Error.WriteLine($"Vrednost opcije dokumenta ne postoji. Moguce vrednosti: {DocumentTypesArgumentValues.Values.Aggregate((prev, next) => $"{prev}, {next}")}");
                return;
            }

            // Operation checking
            if (!hasOperationOption)
            {
                Console.WriteLine($"Nije uneta opcija za operaciju (-o). Podrazumevana opcija \"izdaj\"");
                operation = "izdaj";
            }

            var enviroment = ConfigurationManager.AppSettings.Get("Enviroment");
            var apiKey = ConfigurationManager.AppSettings.Get($"{enviroment}APIKey");
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            switch (operation)
            {
                case OperationsArgumentValues.IZDAVANJE_FAKTURE:
                    var errors = false;
                    XmlDocument xmlDocument = new XmlDocument();

                    switch (documentType)
                    {
                        case DocumentTypesArgumentValues.AVANSNA_FAKTURA:
                            xmlDocument = await AvansnaFaktura.Generate(mainArgument);
                            break;
                        case DocumentTypesArgumentValues.FAKTURA:
                            xmlDocument = await Faktura.Generate(mainArgument);
                            break;
                        case DocumentTypesArgumentValues.KNJIZNO_ODOBRENJE:
                            xmlDocument = await KnjiznoOdobrenje.Generate(mainArgument);
                            break;
                        case DocumentTypesArgumentValues.KONACNA_FAKTURA:
                            xmlDocument = await KonacnaFaktura.Generate(mainArgument);
                            break;
                        case DocumentTypesArgumentValues.TEST_FAKTURA:
                            xmlDocument = await NTETestInvoice.Generate();
                            break;
                        default:
                            Console.Error.WriteLine($"Vrednost opcije dokumenta ne postoji. Moguce vrednosti: {DocumentTypesArgumentValues.Values.Aggregate((prev, next) => $"{prev}, {next}")}");
                            errors = true;
                            break;
                    }

                    if (errors)
                    {
                        return;
                    }

                    // Send to API
                    var invoice = await InvoiceData.GetDataAsync(mainArgument);
                    var issueRequest = new IssueInvoiceRequest(String.IsNullOrEmpty(invoice.RegisterToCRF) ? "Auto" : invoice.RegisterToCRF)
                    {
                        Body = xmlDocument.OuterXml
                    };

                    // THE POST
                    var content = new StringContent(issueRequest.Body, Encoding.UTF8, "application/xml");
                    var response = await client.PostAsync(issueRequest.URL, content);
                    var sentAtUtc = DateTime.UtcNow;

                    var responseContents = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"StatusCode: {response.StatusCode}");

                    var apiResponse = new APIResponse
                    {
                        StatusCode = (int)response.StatusCode,
                        SentAtUtc = sentAtUtc
                    };

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var responseContent = JsonSerializer.Deserialize<InvoiceErrorResponse>(responseContents);

                        Console.WriteLine($"Message: {responseContent.Message}");
                        apiResponse.Message = responseContent.Message;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseObject = JsonSerializer.Deserialize<IssueInvoiceResponse>(responseContents);
                        responseObject.InternalInvoiceId = Int32.Parse(mainArgument);
                        apiResponse.Message = "Everything OK. Data written to [dbo].[ERPREsponses].";

                        await InvoiceRequestOperations.SaveInvoiceIssueResponse(responseObject);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        apiResponse.Message = $"API couldn't find the endpoint {issueRequest.URL}.";
                    }
                    
                    await InvoiceRequestOperations.SaveAPICallResponse(apiResponse);
                    await InvoiceRequestOperations.SaveInvoiceIssueRequest(mainArgument, issueRequest);
                    break;
                case OperationsArgumentValues.STATUS_IZDATE_FAKTURE:

                    // Send to API
                    var statusRequest = new InvoiceStatusRequest(mainArgument);

                    // THE GET
                    var statusResponse = await client.GetAsync(statusRequest.URL);
                    var statusSentAtUtc = DateTime.UtcNow;

                    var statusResponseContents = await statusResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"StatusCode: {statusResponse.StatusCode}");

                    var statusApiResponse = new APIResponse
                    {
                        StatusCode = (int)statusResponse.StatusCode,
                        SentAtUtc = statusSentAtUtc
                    };

                    if (statusResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var statusResponseData = JsonSerializer.Deserialize<InvoiceErrorResponse>(statusResponseContents);
                        Console.WriteLine($"Message: {statusResponseData.Message}");
                        statusApiResponse.Message = statusResponseData.Message;
                    }
                    else if (statusResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseObject = JsonSerializer.Deserialize<InvoiceStatusResponse>(statusResponseContents);
                        statusApiResponse.Message = "Everything OK. Data written to [dbo].[ERPInvoices].";

                        await InvoiceRequestOperations.SaveInvoiceStatusResponse(responseObject);
                    }
                    else if (statusResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        statusApiResponse.Message = $"API couldn't find the endpoint {statusRequest.URL}.";
                    }

                    await InvoiceRequestOperations.SaveAPICallResponse(statusApiResponse);
                    break;
                default:
                    Console.WriteLine("No operation");
                    break;
            }

            Console.WriteLine("Program zavrsen. Pritisnite bilo koje dugme da izadjete.");
            Console.ReadKey();
        }
    }
}
