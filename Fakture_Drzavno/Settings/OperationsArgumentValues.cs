using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Settings
{
    internal class OperationsArgumentValues
    {
        public const string IZDAVANJE_FAKTURE = "izdaj";
        public const string STATUS_IZDATE_FAKTURE = "istatus";
        public const string PROMENA_STATUSA_NA_DAN = "dnevnistatuspromena";
        public const string SADRZAJ_IZDATE_FAKTURE = "isadrzaj";
        public const string REGISTRACIJA_CALLBACK_OPERACIJE = "callback";

        public static readonly string[] Values = new string[]
        {
                IZDAVANJE_FAKTURE,
                STATUS_IZDATE_FAKTURE,
                PROMENA_STATUSA_NA_DAN,
                SADRZAJ_IZDATE_FAKTURE,
                REGISTRACIJA_CALLBACK_OPERACIJE,
        };
    }
}
