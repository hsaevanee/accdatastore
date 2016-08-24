using ACCDataStore.Entity.DatahubProfile;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatahubDataSummaryGenerator
{
    class SummaryGenerator
    {
        private static ISessionFactory CreateSessionFactory()
        {
            //var connectionString = "server=localhost;user id=root;password=toor;persist security info=True;database=simd";
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(
                        c => c.FromConnectionStringWithKey("ConnectionString")
                    )
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ACCDataStore.Entity.BaseEntity>())
                .BuildSessionFactory();
        }
        static void Main(string[] args)
        {
            var sessionFactory = CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    var str = session.Get<DataZoneObj>("S01000001").Reference_Parent;
                    // insert magic code
                    IList datahubStudentData = session.CreateCriteria<DatahubDataObj>().List();
                    Console.WriteLine(str);
                }
            }
            Console.WriteLine("rdy");
            Console.ReadKey();
        }
    }
}
