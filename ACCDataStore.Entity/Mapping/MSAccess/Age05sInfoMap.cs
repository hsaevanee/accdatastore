using ACCDataStore.Entity.PupilsPopulation;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class Age05sInfoMap : ClassMap<PupilsInfoObj>
    {
        public Age05sInfoMap() {
            Table("Age05sinfo");
            Id(x => x.id).Column("Id");
            Map(x => x.dbmonth);
            Map(x => x.dbyear);
            Map(x => x.gender);
            Map(x => x.post_in);
            Map(x => x.post_out);
            Map(x => x.postcode);

        
        }
    }
}
