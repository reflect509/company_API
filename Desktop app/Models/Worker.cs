﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_app.Models
{
    public class Worker
    {
        public required string FullName { get; set; }
        public required string Phone { get; set; }
        public required string Office { get; set; }
        public required string Email { get; set; }
        public required string JobPosition { get; set; }
        public required string SubdepartmentName { get; set; }
    }
}
