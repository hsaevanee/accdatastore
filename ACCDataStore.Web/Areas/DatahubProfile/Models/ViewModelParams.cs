using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class ViewModelParams
    {
        public List<string> school { get; set; }
        public List<string> neighbourhood { get; set; }
        public string councilName { get; set; }
    }
}