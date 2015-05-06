using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACCDataStore.Entity;

namespace ACCDataStore.Web.Areas.Nationality.ViewModels.IndexNationality
{
    public class IndexViewModel
    {
        public IList<Nationality2012> ListNationality2012 { get; set; }
    }
}