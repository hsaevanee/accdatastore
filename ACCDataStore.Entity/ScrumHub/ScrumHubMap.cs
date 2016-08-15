using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Entity.ScrumHub
{
    public class ScrumHubMap: ClassMap<ScrumTable>
    {
        public ScrumHubMap() 
        {
            Table("scrumtable");
            Id(x => x.id);
            Map(x => x.firstname);
            Map(x => x.lastname);
            Map(x => x.age);

        }
    }
   
}