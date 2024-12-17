namespace LdapAuthentication.Model
{
    public class LdapConfig
    {
        public string Path { get; set; }
        public string DomainName { get; set; }
        public string Url { get; set; }
        public string BindDn { get; set; }
        public string BindCredentials { get; set; }
        public string SearchBase { get; set; }
        public string SearchFilter { get; set; }
        public string AdminCn { get; set; }
        public int Port{ get; set; }
        public string Username{ get; set; }
        public string Password{ get; set; }
        public string Host{ get; set; }
    }
}
