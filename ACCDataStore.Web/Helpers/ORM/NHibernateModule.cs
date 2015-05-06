using NHibernate;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using ACCDataStore.Repository;
using ACCDataStore.Repository.Impl;

namespace ACCDataStore.Helpers.ORM
{
    public class NHibernateModule : NinjectModule
    {
        public override void Load()
        {
            Bind<INHibernateHelper>().To<NHibernateHelper>().InSingletonScope();
            Bind<ISessionFactory>().ToMethod(context => context.Kernel.Get<INHibernateHelper>().CreateSessionFactory()).InSingletonScope();
            Bind<IUnitOfWork>().To<NHibernateUnitOfWork>().InRequestScope();
            Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>().OpenSession()).InRequestScope();
            Bind<IGenericRepository>().To<GenericRepositoryImpl>().InRequestScope();
        }
    }
}
