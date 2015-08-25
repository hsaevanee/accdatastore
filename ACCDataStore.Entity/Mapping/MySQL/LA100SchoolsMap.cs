using ACCDataStore.Entity.MySQL;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MySQL
{
    public class LA100SchoolsMap : ClassMap<LA100Schools>
    {
        public LA100SchoolsMap()
        {
            Id(x => x.ID);
            Map(x => x.SeedCode);
            Map(x => x.SchoolName);
        }
    }
}
