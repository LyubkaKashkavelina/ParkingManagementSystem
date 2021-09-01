using System.Collections.Generic;

namespace WebHost.Repository.RepositoryContracts
{

    public interface IRepository<TEntity> where TEntity : class
    {
        bool IsExistingToken(string token);
    }
}
