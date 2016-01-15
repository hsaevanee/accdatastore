using System;

namespace ACCDataStore.Helpers.ORM
{
    public interface IUnitOfWork3nd : IDisposable
    {
        void SaveChanges();
    }
}
