using System;
using System.Collections.Generic;
using System.Text;

namespace Fakture_Drzavno.Contracts.Models
{
    internal class APIResponse
    {
        public int Id { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public DateTime SentAtUtc { get; set; }
    }
}
