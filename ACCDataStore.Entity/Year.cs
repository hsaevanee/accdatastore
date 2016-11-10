﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class Year: BaseEntity
    {
        private string _year;
        public string year {
            get {
                return this._year;
            }
            set {
                this._year = value;
                switch (value)
                {
                    case "2008":
                        this.academicyear = "2008/2009";
                        break;
                    case "2009":
                        this.academicyear = "2009/2010";
                        break;
                    case "2010":
                        this.academicyear = "2010/2011";
                        break;
                    case "2011":
                        this.academicyear = "2011/2012";
                        break;
                    case "2012":
                        this.academicyear = "2012/2013";
                        break;
                    case "2013":
                        this.academicyear = "2013/2014";
                        break;
                    case "2014":
                        this.academicyear = "2014/2015";
                        break;
                    case "2015":
                        this.academicyear = "2015/2016";
                        break;
                    case "2016":
                        this.academicyear = "2016/2017";
                        break;
                }
            }
        }
        public string academicyear { get; set; }
        //public string isSelected { get; set; }

        public Year(string year)
        {
            this.year = year;
        }

        public Year()
        {
        }
    }
}
