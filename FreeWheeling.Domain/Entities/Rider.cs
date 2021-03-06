﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeWheeling.Domain.Entities
{
    public class Rider
    {
        public int id { get; set; }
        public string userId { get; set; }
        [Required]
        public Ride Ride { get; set; }
        public string PercentKeen { get; set; }
        public DateTime LeaveTime { get; set; }
        public string Name { get; set; }
    }
}
