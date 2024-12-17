using LdapAuthentication.Models;
using System.Threading;

namespace LdapAuthentication.Interface
{
    public interface ILdapAuthenticationService
    {
        bool LdapAuthentication(string username, string password, CancellationToken cancellationToken);
    }
}
