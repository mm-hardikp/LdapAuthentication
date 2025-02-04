﻿namespace LdapAuthentication.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public bool IsAdmin { get; set; }

        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
