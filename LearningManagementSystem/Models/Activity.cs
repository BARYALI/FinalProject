﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningManagementSystem.Models
{
    public class Activity
    {

        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }


       
        public Module Module { get; set; }
        public int ModuleId { get; set; }



    }
}