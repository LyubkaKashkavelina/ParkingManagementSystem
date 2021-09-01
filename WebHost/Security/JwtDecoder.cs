namespace WebHost.Security
{
    using Jose;
    using System;
    using System.Net;
    using System.ServiceModel.Web;

    public class JwtDecoder
    {
        public Guid GetCurrentUserId()
        {
            var jwtGenerator = new JwtGenerator();

            byte[] securityKey = jwtGenerator.Base64UrlDecode(SecurityKeyReader.GetSecurityKey());

            var token = GetTokenFromRequest();

            var user = JWT.Decode<DomainModels.User>(token, securityKey, JwsAlgorithm.HS256);

            return user.UserId;
        }

        public string GetTokenFromRequest()
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;

            var token = headers.Get("token");

            return token;
        }
    }
}