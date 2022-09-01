using Fakture_Drzavno.Contracts.Requests;
using Fakture_Drzavno.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Fakture_Drzavno.Contracts
{
    internal class InvoiceRequestOperations
    {
        public static async Task SaveInvoiceIssueRequest(string internalInvoiceID, IssueInvoiceRequest request)
        {
            var connectionEnviroment = ConfigurationManager.AppSettings.Get("Connection");
            var connectionString = ConfigurationManager.ConnectionStrings[$"{connectionEnviroment}Connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var Existing_RequestID = $"SELECT * FROM [dbo].[ERPRequests] WHERE [InternalInvoiceID] LIKE '{internalInvoiceID}'";
            var searchCmd = connection.CreateCommand();
            searchCmd.CommandText = Existing_RequestID;

            SqlDataAdapter sda = new SqlDataAdapter(searchCmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            var requestExists = dt.Rows.Count > 0;
            string Insert_Request;
            if (requestExists)
            {
                Insert_Request = $"UPDATE [dbo].[ERPRequests] SET [LastSent] = @param2 WHERE [InternalInvoiceID] LIKE '{internalInvoiceID}'";
            }
            else
            {
                Insert_Request = "INSERT INTO [dbo].[ERPRequests]([RequestID],[CreatedAt],[LastSent],[InternalInvoiceID]) VALUES (@param1, @param3, @param2, @param4)";
            }

            var cmd = new SqlCommand(Insert_Request, connection);

            if (!requestExists)
            {
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar, 36).Value = request.RequestID;
                cmd.Parameters.Add("@param3", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@param4", SqlDbType.NVarChar, 10).Value = internalInvoiceID;
            }
            cmd.Parameters.Add("@param2", SqlDbType.DateTime).Value = DateTime.Now;

            cmd.CommandType = CommandType.Text;
            await cmd.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }

        public static async Task<bool> SaveInvoiceIssueResponse(IssueInvoiceResponse response)
        {
            var connectionEnviroment = ConfigurationManager.AppSettings.Get("Connection");
            var connectionString = ConfigurationManager.ConnectionStrings[$"{connectionEnviroment}Connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var Existing_RequestID = $"SELECT * FROM [dbo].[ERPResponses] WHERE [InternalInvoiceId] LIKE '{response.InternalInvoiceId}'";
            var searchCmd = connection.CreateCommand();
            searchCmd.CommandText = Existing_RequestID;

            SqlDataAdapter sda = new SqlDataAdapter(searchCmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            var requestExists = dt.Rows.Count > 0;
            if (requestExists)
            {
                return false;
            }

            var Insert_Response = "INSERT INTO [dbo].[ERPResponses]([InvoiceId],[PurchaseInvoiceId],[SalesInvoiceId],[InternalInvoiceId]) VALUES (@param1, @param2, @param3, @param4)";
            var cmd = new SqlCommand(Insert_Response, connection);

            if (!requestExists)
            {
                cmd.Parameters.Add("@param1", SqlDbType.Int).Value = response.InvoiceId;
                cmd.Parameters.Add("@param2", SqlDbType.Int).Value = response.PurchaseInvoiceId;
                cmd.Parameters.Add("@param3", SqlDbType.Int).Value = response.SalesInvoiceId;
                cmd.Parameters.Add("@param4", SqlDbType.Int).Value = response.InternalInvoiceId;
            }

            cmd.CommandType = CommandType.Text;
            await cmd.ExecuteNonQueryAsync();

            await connection.CloseAsync();
            return true;
        }

        public static async Task<bool> SaveInvoiceStatusResponse(InvoiceStatusResponse response)
        {
            var connectionEnviroment = ConfigurationManager.AppSettings.Get("Connection");
            var connectionString = ConfigurationManager.ConnectionStrings[$"{connectionEnviroment}Connection"].ConnectionString;

            string query;
            var alreadyInDatabase = await InvoiceAlreadyExistsWithStatus(response.InvoiceId, response.Status);
            if (alreadyInDatabase)
            {
                query = "UPDATE [dbo].[ERPInvoices] SET " +
                    "[SalesInvoiceId] = @param1, " +
                    "[Status] = @param2, " +
                    "[LastModifiedUtc] = @param3, " +
                    "[UpdatedAt] = @param4, " +
                    "[Comment] = @param5, " +
                    "[CancelComment] = @param6, " +
                    "[StornoComment] = @param7, " +
                    "[EventId] = @param8 " +
                    "WHERE SalesInvoiceId = @param101 AND Status = @param102";
            }
            else {
                query = "INSERT INTO [dbo].[ERPInvoices]([SalesInvoiceId],[Status],[LastModifiedUtc],[UpdatedAt],[Comment],[CancelComment],[StornoComment],[EventId]) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8)";
            }

            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var cmd = new SqlCommand(query, connection);

            if (alreadyInDatabase)
            {
                cmd.Parameters.Add("@param101", SqlDbType.Int).Value = response.InvoiceId;
                cmd.Parameters.Add("@param102", SqlDbType.NVarChar, 30).Value = response.Status;
            }

            cmd.Parameters.Add("@param1", SqlDbType.Int).Value = response.InvoiceId;
            cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 30).Value = response.Status;
            cmd.Parameters.Add("@param3", SqlDbType.DateTime).Value = response.LastModifiedUtc;
            cmd.Parameters.Add("@param4", SqlDbType.DateTime).Value = response.UpdatedAt;
            cmd.Parameters.Add("@param5", SqlDbType.NVarChar, 250).Value = response.Comment ?? (object)DBNull.Value;
            cmd.Parameters.Add("@param6", SqlDbType.NVarChar, 250).Value = response.CancelComment ?? (object)DBNull.Value;
            cmd.Parameters.Add("@param7", SqlDbType.NVarChar, 250).Value = response.StornoComment ?? (object)DBNull.Value;
            cmd.Parameters.Add("@param8", SqlDbType.Int).Value = DBNull.Value;

            cmd.CommandType = CommandType.Text;
            await cmd.ExecuteNonQueryAsync();

            await connection.CloseAsync();
            return true;
        }

        public static async Task<bool> SaveMultipleInvoiceStatusResponse(List<InvoiceStatusResponseOnDateResponse> response)
        {
            var connectionEnviroment = ConfigurationManager.AppSettings.Get("Connection");
            var connectionString = ConfigurationManager.ConnectionStrings[$"{connectionEnviroment}Connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var valuesString = "";
            var index = 1;
            foreach (var invoice in response)
            {
                if (await InvoiceAlreadyExistsWithStatus(invoice.SalesInvoiceId, invoice.NewInvoiceStatus))
                {
                    index++;
                    continue;
                }
                valuesString += $"(@param{index}1,@param{index}2,@param{index}3,@param{index}4,@param{index}5,@param{index}6){(index < response.Count ? "," : "")}";
                index++;
            }

            if (valuesString == "")
            {
                return false;
            }

            var Insert_Response = $"INSERT INTO [dbo].[ERPInvoices]([SalesInvoiceId],[Status],[LastModifiedUtc],[UpdatedAt],[Comment],[EventId]) VALUES {valuesString}";
            var cmd = new SqlCommand(Insert_Response, connection);

            var paramValueIndex = 1;
            foreach (var invoice in response)
            {
                if (await InvoiceAlreadyExistsWithStatus(invoice.SalesInvoiceId, invoice.NewInvoiceStatus))
                {
                    paramValueIndex++;
                    continue;
                }

                cmd.Parameters.Add($"@param{paramValueIndex}1", SqlDbType.Int).Value = invoice.SalesInvoiceId;
                cmd.Parameters.Add($"@param{paramValueIndex}2", SqlDbType.NVarChar, 30).Value = invoice.NewInvoiceStatus;
                cmd.Parameters.Add($"@param{paramValueIndex}3", SqlDbType.DateTime).Value = invoice.Date;
                cmd.Parameters.Add($"@param{paramValueIndex}4", SqlDbType.DateTime).Value = invoice.UpdatedAt;
                cmd.Parameters.Add($"@param{paramValueIndex}5", SqlDbType.NVarChar, 250).Value = invoice.Comment ?? (object)DBNull.Value;
                cmd.Parameters.Add($"@param{paramValueIndex}6", SqlDbType.Int).Value = invoice.EventId;

                paramValueIndex++;
            }

            cmd.CommandType = CommandType.Text;
            await cmd.ExecuteNonQueryAsync();

            await connection.CloseAsync();
            return true;
        }

        public static async Task<bool> SaveAPICallResponse(APIResponse response)
        {
            var connectionEnviroment = ConfigurationManager.AppSettings.Get("Connection");
            var connectionString = ConfigurationManager.ConnectionStrings[$"{connectionEnviroment}Connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var Insert_Response = "INSERT INTO [dbo].[ERPApiResponses]([StatusCode],[Message],[SentAtUtc]) VALUES (@param1, @param2, @param3)";
            var cmd = new SqlCommand(Insert_Response, connection);

            cmd.Parameters.Add("@param1", SqlDbType.Int).Value = response.StatusCode;
            cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 1000).Value = response.Message;
            cmd.Parameters.Add("@param3", SqlDbType.DateTime).Value = response.SentAtUtc;

            cmd.CommandType = CommandType.Text;
            await cmd.ExecuteNonQueryAsync();

            await connection.CloseAsync();
            return true;
        }

        private static async Task<bool> InvoiceAlreadyExistsWithStatus(int salesInvoiceId, string status)
        {
            var connectionEnviroment = ConfigurationManager.AppSettings.Get("Connection");
            var connectionString = ConfigurationManager.ConnectionStrings[$"{connectionEnviroment}Connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var Existing_RequestID = $"SELECT * FROM [dbo].[ERPInvoices] WHERE [SalesInvoiceId] LIKE '{salesInvoiceId}' AND [Status] LIKE '{status}'";
            var searchCmd = connection.CreateCommand();
            searchCmd.CommandText = Existing_RequestID;

            SqlDataAdapter sda = new SqlDataAdapter(searchCmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            return dt.Rows.Count > 0;
        }
    }
}
