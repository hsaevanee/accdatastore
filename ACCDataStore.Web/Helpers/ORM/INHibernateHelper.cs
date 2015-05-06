using NHibernate;

namespace ACCDataStore.Helpers.ORM
{
    public interface INHibernateHelper
    {
        ISessionFactory CreateSessionFactory();
    }
}
