using AgarioModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace ClientGUI
{
    internal class WorldDrawable : IDrawable
    {
        public Food[] foods { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Gray;
            canvas.FillRectangle(dirtyRect);

            foreach (Food food in foods) 
            {
                canvas.FillColor = Color.FromInt(food.ARGBColor);
                food.radius = (float)Math.Sqrt(food.Mass / Math.PI); // radius = square root of area / pi
                canvas.FillCircle(food.X, food.Y, food.radius); 
            }
        }

    }
}
