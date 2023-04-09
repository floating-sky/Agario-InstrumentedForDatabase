using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AgarioModels
{
    internal class GameObject
    {
        public long ID { get; set; }

        public float X { get; }
        public float Y { get; }

        public Vector2 location { get; set; }
        

        public int ARGBColor { get; set; }

        public float Mass { get; set; }

        public GameObject()
        {
            
        }
}

    
}
