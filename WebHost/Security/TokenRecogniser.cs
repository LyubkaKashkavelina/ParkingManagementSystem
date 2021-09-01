namespace WebHost.Security
{
    using System;
    using WebHost.Repository.RepositoryContracts;
    using WebHost.Security;

    public class TokenRecogniser<T> where T : class
    {
        private readonly IRepository<T> _repository;
        public TokenRecogniser(IRepository<T> repository)
        {
            this._repository = repository;
        }

        public Guid DecodeToken()
        {
            var tokenDecoder = new JwtDecoder();
            var token = tokenDecoder.GetTokenFromRequest();

            if (!this._repository.IsExistingToken(token))
            {
                throw new ArgumentException("Token does not exist.");
            }

            var userId = tokenDecoder.GetCurrentUserId();

            return userId;
        }
    }
}