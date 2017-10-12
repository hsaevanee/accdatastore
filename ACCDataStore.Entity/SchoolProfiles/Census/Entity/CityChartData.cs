﻿using ACCDataStore.Entity.RenderObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class CityChartData: ChartData
    {
        public BaseRenderObject ChartCfeP1Level { get; set; }
        public BaseRenderObject ChartCfeP4Level { get; set; }
        public BaseRenderObject ChartCfeP7Level { get; set; }
        public BaseRenderObject ChartCfeP1LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfeP4LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfeP7LevelbyQuintile { get; set; }
        public BaseRenderObject ChartFSMPrimary { get; set; }
        public BaseRenderObject ChartFSMSecondary { get; set; }
        public BaseRenderObject ChartFSMSpecial { get; set; }
        public BaseRenderObject ChartFSMCity { get; set; }
        public BaseRenderObject ChartCfe3LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfe4LevelbyQuintile { get; set; }
        public BaseRenderObject ChartCfe3Level { get; set; }
        public BaseRenderObject ChartCfe4Level { get; set; }
        public BaseRenderObject ChartP1TimelineCfEReading { get; set; }
        public BaseRenderObject ChartP1TimelineCfEWriting { get; set; }
        public BaseRenderObject ChartP1TimelineCfEELT { get; set; }
        public BaseRenderObject ChartP1TimelineCfENumeracy{ get; set; }
        public BaseRenderObject ChartP4TimelineCfEReading { get; set; }
        public BaseRenderObject ChartP4TimelineCfEWriting { get; set; }
        public BaseRenderObject ChartP4TimelineCfEELT { get; set; }
        public BaseRenderObject ChartP4TimelineCfENumeracy { get; set; }
        public BaseRenderObject ChartP7TimelineCfEReading { get; set; }
        public BaseRenderObject ChartP7TimelineCfEWriting { get; set; }
        public BaseRenderObject ChartP7TimelineCfEELT { get; set; }
        public BaseRenderObject ChartP7TimelineCfENumeracy { get; set; }
    }
}
