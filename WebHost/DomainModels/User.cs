using System;

namespace WebHost.DomainModels
{
    public class User
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public string PhoneNumber { get; set; }
    }
}