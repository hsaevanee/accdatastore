﻿using FluentNHibernate.Cfg;
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
                    sConnectionString = sDbName;
                    configuration = Fluently
                        .Configure()
                        .Database(JetDriverConfiguration
                            .Standard
                            .ConnectionString(c => c.DatabaseFile(sConnectionString))
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
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.Mapping.MySQL.LA100SchoolsMap>())
                    .BuildConfiguration();
                    break;
            }

            return configuration.BuildSessionFactory();
        }

        public ISessionFactory CreateSessionFactory2nd()
        {
            var sDbUser = System.Configuration.ConfigurationManager.AppSettings["dbUser_2nd"];
            var sDbPassword = System.Configuration.ConfigurationManager.AppSettings["dbPassword_2nd"];
            var sDbName = System.Configuration.ConfigurationManager.AppSettings["dbName_2nd"];
            var sDbHost = System.Configuration.ConfigurationManager.AppSettings["dbHost_2nd"];
            var sDbType = System.Configuration.ConfigurationManager.AppSettings["dbType_2nd"];

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
                    sConnectionString = sDbName;
                    configuration = Fluently
                        .Configure()
                        .Database(JetDriverConfiguration
                            .Standard
                            .ConnectionString(c => c.DatabaseFile(sConnectionString))
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
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.Mapping.MySQL.LA100SchoolsMap>())
                    .BuildConfiguration();
                    break;
            }

            return configuration.BuildSessionFactory();
        }
    }
}
