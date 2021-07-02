using System;

namespace Admin.Models
{
    public class Token
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }

    }
}