using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping
{
    public class Nationality2012Map : ClassMap<Nationality2012>
    {
        public Nationality2012Map()
        {
            Table("nationality2012");
            Id(x => x.ID).Column("id");
            Map(x => x.NationalIdentity).Column("national_identity");
            Map(x => x.Gender).Column("gender");
        }
    }
}
