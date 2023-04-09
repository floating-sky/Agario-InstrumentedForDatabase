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

        public readonly int width;
        public readonly int height;

        public HashSet<Player> playerSet;
        public HashSet<Food> foodSet;
        
        public World(ILogger logger) 
        {
            _logger = logger;
            playerSet = new HashSet<Player>();
            foodSet = new HashSet<Food>();
            width =  5000;
            height = 5000;
        }
    }
}
