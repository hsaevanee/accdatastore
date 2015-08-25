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
            // origianal with single database
            //Bind<INHibernateHelper>().To<NHibernateHelper>().InSingletonScope();
            //Bind<ISessionFactory>().ToMethod(context => context.Kernel.Get<INHibernateHelper>().CreateSessionFactory()).InSingletonScope();
            //Bind<IUnitOfWork>().To<NHibernateUnitOfWork>().InRequestScope();
            //Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>().OpenSession()).InRequestScope();
            //Bind<IGenericRepository>().To<GenericRepositoryImpl>().InRequestScope();

            Bind<INHibernateHelper>().To<NHibernateHelper>().InSingletonScope();
            Bind<ISessionFactory>().ToMethod(context => context.Kernel.Get<INHibernateHelper>().CreateSessionFactory()).InSingletonScope().Named("Database1st");
            Bind<ISessionFactory>().ToMethod(context => context.Kernel.Get<INHibernateHelper>().CreateSessionFactory2nd()).InSingletonScope().Named("Database2nd");
            Bind<IUnitOfWork>().To<NHibernateUnitOfWork>().InRequestScope();
            Bind<IUnitOfWork2nd>().To<NHibernateUnitOfWork2nd>().InRequestScope();
            Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>("Database1st").OpenSession()).WhenInjectedInto<GenericRepositoryImpl>().InRequestScope().Named("Session1st");
            Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>("Database2nd").OpenSession()).WhenInjectedInto<GenericRepository2ndImpl>().InRequestScope().Named("Session2nd");
            
            Bind<IUnitOfWork>().To<NHibernateUnitOfWork>().WithConstructorArgument("session", c => c.Kernel.Get<ISession>("Session1st")); // not working right now
            Bind<IUnitOfWork2nd>().To<NHibernateUnitOfWork2nd>().WithConstructorArgument("session", c => c.Kernel.Get<ISession>("Session2nd")); // not working right now
            
            Bind<IGenericRepository>().To<GenericRepositoryImpl>().InRequestScope();
            Bind<IGenericRepository2nd>().To<GenericRepository2ndImpl>().InRequestScope();
        }
    }
}
