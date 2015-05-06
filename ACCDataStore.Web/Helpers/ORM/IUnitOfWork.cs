using System;

namespace ACCDataStore.Helpers.ORM
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
