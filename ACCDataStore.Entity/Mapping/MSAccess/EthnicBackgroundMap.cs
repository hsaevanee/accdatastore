using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class EthnicBackgroundMap : ClassMap<EthnicBackgroundObj>
    {
        public EthnicBackgroundMap() {
            Table("Lu_EthnicBackground"); //from Scotxed_15 database
            Id(x => x.id);
            Map(x => x.code);
            Map(x => x.name);
            Map(x => x.scotXedCode);  
        }
    }
}
