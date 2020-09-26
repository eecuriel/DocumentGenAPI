using System;

namespace DocumentGenAPI.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Id { get; set; }

    }
}


