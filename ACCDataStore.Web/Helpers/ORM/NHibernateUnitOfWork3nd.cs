using NHibernate;

namespace ACCDataStore.Helpers.ORM
{
    public class NHibernateUnitOfWork3nd : IUnitOfWork3nd
    {
        private readonly ISession _session;
        private ITransaction _transaction;

        public NHibernateUnitOfWork3nd(ISession session)
        {
            _session = session;
            _transaction = session.BeginTransaction();
        }

        public void Dispose()
        {
            if (_transaction == null)
                return;

            if (_transaction.IsActive && !_transaction.WasCommitted)
                _transaction.Rollback();

            _transaction.Dispose();
            _transaction = null;
        }

        public void SaveChanges()
        {
            _transaction.Commit();
        }
    }
}
