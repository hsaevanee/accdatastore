using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping
{
    public class UsersMap : ClassMap<Users>
    {
        public UsersMap()
        {
            Id(x => x.UserID);
            Map(x => x.UserName);
            Map(x => x.Password);
            Map(x => x.FullName);
        }
    }
}
