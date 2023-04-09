using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowardAgarioStepOne
{
    internal class WorldDrawable : IDrawable
    {
        public WorldModel model;
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillCircle(model.circlePosition.X, model.circlePosition.Y, model.circleRadius);
        }
    }
}
