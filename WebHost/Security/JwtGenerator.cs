namespace WebHost.Security
{
    using Jose;
    using System;

    public class JwtGenerator
    {
        public string GetJwt(DomainModels.User targetUser)
        {
            byte[] secretKey = Base64UrlDecode(SecurityKeyReader.GetSecurityKey());

            string token = JWT.Encode(targetUser, secretKey, JwsAlgorithm.HS256);

            return token;
        }

        public byte[] Base64UrlDecode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); 
            s = s.Replace('_', '/');
            switch (s.Length % 4) 
            {
                case 0: break; 
                case 2: s += "=="; break; 
                case 3: s += "="; break;
                default:
                    throw new System.Exception(
                "Illegal base64url string!");
            }
            return Convert.FromBase64String(s);  
        }
    }
}
