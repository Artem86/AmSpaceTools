﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmSpaceModels
{
    public class AmSpaceEnvironment : IAmSpaceEnvironment
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string BaseAddress { get; set; }
        public string GrantPermissionType { get; set; }
    }
}
