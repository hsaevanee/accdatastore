﻿using ACCDataStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.InsightProfile.ViewModels.BenchmarkMeasure
{
    public class LeaverDestination : CriteriaObjViewModel
    {
        public int centrecode { get; set; }
        public string centrename { get; set; }
        public int year { get; set; }
        public Gender gender { get; set; }
        public Year academicyear { get; set; }
        public double sum0 { get; set; }
        public double sum1 { get; set; }
        public double sum2 { get; set; }

        public double Percentage
        {
            get 
            {
                return (sum1 * 100) / (sum1 + sum2+sum0);                
            }
        }
    }
}