using Fakture_Drzavno.Contracts.Models;
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

            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var Insert_Response = "INSERT INTO [dbo].[ERPInvoices]([SalesInvoiceId],[Status],[LastModifiedUtc]) VALUES (@param1, @param2, @param3)";
            var cmd = new SqlCommand(Insert_Response, connection);

            cmd.Parameters.Add("@param1", SqlDbType.Int).Value = response.InvoiceId;
            cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 30).Value = response.Status;
            cmd.Parameters.Add("@param3", SqlDbType.DateTime).Value = response.LastModifiedUtc;

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
    }
}
