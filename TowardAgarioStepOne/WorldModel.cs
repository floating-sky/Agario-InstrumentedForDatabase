using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TowardAgarioStepOne
{
    internal class WorldModel
    {

        public Vector2 circlePosition = new Vector2(100, 100);
        public int circleRadius = 10;
        public Vector2 direction = new Vector2(50, 25);

        public void AdvanceGameOneStep()
        {
            if (circlePosition.X <= 0 || circlePosition.X >= 800)
                direction.X *= -1;
            if (circlePosition.Y <= 0 || circlePosition.Y >= 800)
                direction.Y *= -1;
        }
    }
}
