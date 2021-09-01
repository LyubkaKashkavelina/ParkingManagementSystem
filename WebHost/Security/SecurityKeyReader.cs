namespace WebHost.Security
{
    using System.Configuration;

    public static class SecurityKeyReader
    {
        public static string GetSecurityKey()
        {
            string securityKey = ConfigurationManager.AppSettings["SecurityKey"];

            return securityKey;
        }
    }
}