﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmSpaceModels;
using Newtonsoft.Json;

namespace AmSpaceTools.Infrastructure.Providers
{
    public class AmSpaceEnvironmentsProvider : IAmSpaceEnvironmentsProvider
    {
        private IEnumerable<AmSpaceEnvironment> _environments;
        public IEnumerable<AmSpaceEnvironment> Environments
        {
            get
            {
                if (_environments == null)
                {
                    var currentPath = AppDomain.CurrentDomain.BaseDirectory;
                    _environments = JsonConvert.DeserializeObject<List<AmSpaceEnvironment>>(File.ReadAllText($"{currentPath}\\Environments.json"));
                }
                return _environments;
            }
        }
    }
}
