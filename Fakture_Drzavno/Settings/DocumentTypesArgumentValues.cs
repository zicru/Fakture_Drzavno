using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Settings
{
    internal class DocumentTypesArgumentValues
    {
        public const string AVANSNA_FAKTURA = "avansna";
        public const string FAKTURA = "faktura";
        public const string KNJIZNO_ODOBRENJE = "odobrenje";
        public const string KNJIZNO_ZADUZENJE = "zaduzenje";
        public const string KONACNA_FAKTURA = "konacna";
        public const string TEST_FAKTURA = "test";

        public static readonly string[] Values = new string[]
        {
            AVANSNA_FAKTURA,
            FAKTURA,
            KNJIZNO_ODOBRENJE,
            KNJIZNO_ZADUZENJE,
            KONACNA_FAKTURA,
            TEST_FAKTURA
        };
    }
}
