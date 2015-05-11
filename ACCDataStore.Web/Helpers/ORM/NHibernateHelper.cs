using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace ACCDataStore.Helpers.ORM
{
    public class NHibernateHelper : INHibernateHelper
    {
        public ISessionFactory CreateSessionFactory()
        {
            var sDbUser = System.Configuration.ConfigurationManager.AppSettings["dbUser"];
            var sDbPassword = System.Configuration.ConfigurationManager.AppSettings["dbPassword"];
            var sDbName = System.Configuration.ConfigurationManager.AppSettings["dbName"];
            var sDbHost = System.Configuration.ConfigurationManager.AppSettings["dbHost"];
            var sDbType = System.Configuration.ConfigurationManager.AppSettings["dbType"];

            string sConnectionString;
            global::NHibernate.Cfg.Configuration configuration;

            switch (sDbType)
            {
                case "2":
                    sConnectionString = @"Server=" + sDbHost + ";Initial Catalog=" + sDbName + ";User Id=" + sDbUser + ";Password=" + sDbPassword;
                    configuration = Fluently
                        .Configure()
                        .Database(MsSqlConfiguration
                            .MsSql2012
                            .ConnectionString(sConnectionString)
                            .ShowSql
                        )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.Mapping.Nationality2012Map>())
                    .BuildConfiguration();
                    break;
                case "3":
                    sConnectionString = @"Server=" + sDbHost + ";Database=" + sDbName + ";User ID=" + sDbUser + ";Password=" + sDbPassword + ";";
                    configuration = Fluently
                        .Configure()
                        .Database(PostgreSQLConfiguration           
                            .Standard
                            .ConnectionString(sConnectionString)
                            .ShowSql
                        )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.Mapping.Nationality2012Map>())
                    .BuildConfiguration();
                    break;
                case "4":
                    sConnectionString = @"C:\Users\s01hs4\Documents\Database2.accdb";
                    configuration = Fluently
                        .Configure()
                        .Database(JetDriverConfiguration
                            .Standard
                            .ConnectionString(sConnectionString)
                            .ShowSql
                        )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.Mapping.MSAccess.StudentSIMDMap>())
                    .BuildConfiguration();
                    break;
                default:
                    sConnectionString = @"Server=" + sDbHost + ";Database=" + sDbName + ";User ID=" + sDbUser + ";Password=" + sDbPassword + ";";
                    configuration = Fluently
                        .Configure()
                        .Database(MySQLConfiguration
                            .Standard
                            .ConnectionString(sConnectionString)
                            .ShowSql
                        )
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.Mapping.Nationality2012Map>())
                    .BuildConfiguration();
                    break;
            }

            return configuration.BuildSessionFactory();
        }
    }
}
