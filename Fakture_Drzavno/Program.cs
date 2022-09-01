using Fakture_Drzavno.Contracts;
using Fakture_Drzavno.Contracts.Requests;
using Fakture_Drzavno.Contracts.Responses;
using Fakture_Drzavno.Domain;
using Fakture_Drzavno.Invoices;
using Fakture_Drzavno.Settings;
using System;
using System.Collections.Generic;
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
            if (!hasDocumentTypeOption && (operation != OperationsArgumentValues.STATUS_IZDATE_FAKTURE && operation != OperationsArgumentValues.PROMENA_STATUSA_NA_DAN))
            {
                Console.WriteLine($"Nije uneta opcija za tip fakture (-t). Podrazumevan tip \"faktura\"");
                documentType = "faktura";
            }

            // Add check if its the right type
            if (!DocumentTypesArgumentValues.Values.Contains(documentType) && (operation != OperationsArgumentValues.STATUS_IZDATE_FAKTURE && operation != OperationsArgumentValues.PROMENA_STATUSA_NA_DAN))
            {
                Console.Error.WriteLine($"Vrednost tipa dokumenta ne postoji. Moguce vrednosti: {DocumentTypesArgumentValues.Values.Aggregate((prev, next) => $"{prev}, {next}")}");
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
                        case DocumentTypesArgumentValues.KNJIZNO_ZADUZENJE:
                            xmlDocument = await KnjiznoZaduzenje.Generate(mainArgument);
                            break;
                        case DocumentTypesArgumentValues.KONACNA_FAKTURA:
                            xmlDocument = await KonacnaFaktura.Generate(mainArgument);
                            break;
                        case DocumentTypesArgumentValues.TEST_FAKTURA:
                            xmlDocument = await NTETestInvoice.Generate();
                            break;
                        default:
                            Console.Error.WriteLine($"Vrednost tipa dokumenta ne postoji. Moguce vrednosti: {DocumentTypesArgumentValues.Values.Aggregate((prev, next) => $"{prev}, {next}")}");
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
                    var statusSentAtUtc = DateTime.Now;

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
                        responseObject.UpdatedAt = statusSentAtUtc;

                        statusApiResponse.Message = "Everything OK. Data written to [dbo].[ERPInvoices].";

                        await InvoiceRequestOperations.SaveInvoiceStatusResponse(responseObject);
                    }
                    else if (statusResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        statusApiResponse.Message = $"API couldn't find the endpoint {statusRequest.URL}.";
                    }

                    await InvoiceRequestOperations.SaveAPICallResponse(statusApiResponse);
                    break;
                case OperationsArgumentValues.PROMENA_STATUSA_NA_DAN:
                    // Send to API
                    var statusChangesRequest = new InvoicesChangedOnDateRequest(mainArgument);

                    // THE GET
                    var statusChangesResponse = await client.PostAsync(statusChangesRequest.URL, null);

                    var statusChangesSentAtUtc = DateTime.Now;

                    var statusChangesResponseContents = await statusChangesResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"StatusCode: {statusChangesResponse.StatusCode}");

                    var statusChangesApiResponse = new APIResponse
                    {
                        StatusCode = (int)statusChangesResponse.StatusCode,
                        SentAtUtc = statusChangesSentAtUtc
                    };

                    if (statusChangesResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var statusResponseData = JsonSerializer.Deserialize<InvoiceErrorResponse>(statusChangesResponseContents);
                        Console.WriteLine($"Message: {statusResponseData.Message}");
                        statusChangesApiResponse.Message = statusResponseData.Message;
                    }
                    else if (statusChangesResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseObject = JsonSerializer.Deserialize<List<InvoiceStatusResponseOnDateResponse>>(statusChangesResponseContents);
                        if (responseObject.Count == 0)
                        {
                            Console.WriteLine($"Nije bilo promena na dan {mainArgument}.");
                        }
                        else {
                            foreach (var item in responseObject)
                            {
                                item.UpdatedAt = statusChangesSentAtUtc;
                            }
                            await InvoiceRequestOperations.SaveMultipleInvoiceStatusResponse(responseObject);
                        }
                        
                        statusChangesApiResponse.Message = "Everything OK. Data written to [dbo].[ERPInvoices].";
                    }
                    else if (statusChangesResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        statusChangesApiResponse.Message = $"API couldn't find the endpoint {statusChangesRequest.URL}.";
                    }

                    await InvoiceRequestOperations.SaveAPICallResponse(statusChangesApiResponse);

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
