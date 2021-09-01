namespace WebHost.Repository.Repositories
{
    using System;
    using System.Collections.Generic;
    using WebHost.Data;
    using WebHost.Repository.RepositoryContracts;
    using WebHost.Utility.Exceptions;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ParkingManagementSystemEntities Context;

        public Repository(ParkingManagementSystemEntities context)
        {
            this.Context = context;
        }

        public bool IsExistingToken(string token)
        {
            if(token == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var jwtToken in this.Context.Tokens)
            {
                if(jwtToken.UserToken == token)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
