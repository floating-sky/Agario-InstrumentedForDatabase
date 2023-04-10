using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AgarioModels
{
    public class GameObject
    {

        public float X { get; set; } //TODO: Instructions say not to have setters but serializer wont work without them?
        public float Y { get; set; }
        
        public int ARGBColor { get; set; }

        public long ID { get; set; }

        public float Mass { get; set; }

        public Vector2 location { get; set; }


        public GameObject()
        {
            
        }
}

    
}
