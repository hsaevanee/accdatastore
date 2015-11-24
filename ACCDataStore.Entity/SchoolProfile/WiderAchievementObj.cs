﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class WiderAchievementObj: BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual string schoolname { get; set; }
        public virtual string age_range { get; set; }
        public virtual string awardname { get; set; }
        public virtual string gender { get; set; }
        public virtual int award2013 { get; set; }
        public virtual int award2014 { get; set; }
        public virtual int award2015 { get; set; }

    }
}
