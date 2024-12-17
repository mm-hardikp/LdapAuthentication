using LdapAuthentication.Interface;
using LdapForNet;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;


namespace LdapAuthentication.Services
{

    public class LdapAuthenticationService : ILdapAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public LdapAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }

        public bool LdapAuthentication(string username, string password, CancellationToken cancellationToken)
        {
            var ldapHost = _configuration["LdapSettings:ServerUrl"]; // Replace with your LDAP server
            var ldapPort = _configuration.GetValue<int>("LdapSettings:PortNumber"); // Port 389 for LDAP
            var bindDn = _configuration["LdapSettings:UserName"]; // Replace with your admin DN
            var bindPassword = _configuration["LdapSettings:Password"]; // Replace with your admin password

            string baseDn = "DC=ldapuser,DC=com";
            string ldapFilter = $"(&(objectClass=*)(sAMAccountName={username.Split('@')[0]}))";

            using (var ldapConnection = new LdapForNet.LdapConnection())
            {
                try
                {
                    // Connect to the LDAP server
                    ldapConnection.Connect(ldapHost, ldapPort);

                    // Bind to the LDAP server with admin credentials
                    ldapConnection.Bind(bindDn, bindPassword);

                    // Perform an LDAP search
                    var searchResults = ldapConnection.Search(baseDn, ldapFilter);

                    // Attempt to bind with the provided username and password
                    ldapConnection.Bind(username, password);

                    // Authentication successful if no exceptions occurred
                    return true;
                }
                catch (LdapForNet.LdapException ex)
                {
                    Console.WriteLine($"LDAP Error: {ex.Message}");
                    // Authentication failed
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected Error: {ex.Message}");
                    // Authentication failed
                    return false;
                }
            }
        }

    }
}

