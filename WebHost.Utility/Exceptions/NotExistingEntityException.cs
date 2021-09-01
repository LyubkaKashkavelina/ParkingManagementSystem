namespace WebHost.Utility.Exceptions
{
    using System;

    public class NotExistingEntityException<T> : Exception
    {
        public NotExistingEntityException(T entity)
            :base($"The entity {entity.GetType()} does not exist.")
        {

        }
    }
}
