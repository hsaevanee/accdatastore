﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.OleDb;

namespace ACCDataStore.Web.Areas.SchoolRollForecast.Controllers
{
    public class IndexSchoolRollForecastController : Controller
    {
        // GET: SchoolRollForecast/IndexSchoolRollForecast
        public ActionResult Index()
        {
            return View("Home");



        }


        
        
    
    

    }
}
//put excel code in here
