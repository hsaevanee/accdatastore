using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore
{
    public class AllSchoolMap : ClassMap<AllCitySchools>
    {
        public AllSchoolMap()
        {
            Table("datahubdata_schools");
            Id(x => x.id).Column("id");
            Map(x => x.seedCode);
            Map(x => x.name);
            Map(x => x.referenceCouncil);
        }
    }
}
