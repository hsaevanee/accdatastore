using System;

namespace ACCDataStore.Helpers.ORM
{
    public interface IUnitOfWork2nd : IDisposable
    {
        void SaveChanges();
    }
}
