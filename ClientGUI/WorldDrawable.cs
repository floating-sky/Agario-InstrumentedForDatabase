using AgarioModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    internal class WorldDrawable : IDrawable
    {
        public Food[] foods { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Blue;
            canvas.FillRectangle(dirtyRect);

            foreach (Food food in foods) 
            {
                canvas.FillColor = Colors.Red;
                canvas.FillCircle(food.X, food.Y, food.Mass);
            }
        }

    }
}
