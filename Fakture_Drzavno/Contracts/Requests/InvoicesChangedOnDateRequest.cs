using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Fakture_Drzavno.Contracts.Requests
{
    internal class InvoicesChangedOnDateRequest
    {
        public readonly string URL;
        public string DateOfChange { get; set; }

        public InvoicesChangedOnDateRequest(string dateOfChange)
        {
            DateOfChange = dateOfChange;

            var enviroment = ConfigurationManager.AppSettings.Get("Enviroment");
            var apiURI = ConfigurationManager.AppSettings.Get($"{enviroment}URI");

            URL = $"{apiURI}/sales-invoice/changes?date={DateOfChange}";
        }
    }
}
