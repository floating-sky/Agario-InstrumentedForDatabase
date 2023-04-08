using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgarioModels
{
    internal class World
    {
        private readonly ILogger _logger;

        public World(ILogger logger) 
        {
            _logger = logger;
        }
    }
}
