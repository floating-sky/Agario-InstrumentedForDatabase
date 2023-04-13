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
        public long userPlayerID;
        public Player[] players { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Gray;
            canvas.FillRectangle(dirtyRect);

            foreach (Food food in foods) 
            {
                canvas.FillColor = Color.FromInt(food.ARGBColor);
                food.radius = calculateRadius(food.Mass); // radius = square root of area / pi
                canvas.FillCircle(food.X, food.Y, food.radius); 
            }

            if (players is not null)
            {
                foreach (Player player in players)
                {
                    canvas.FillColor = Color.FromInt(player.ARGBColor);
                    player.radius = calculateRadius(player.Mass); // radius = square root of area / pi
                    canvas.FillCircle(player.X, player.Y, player.radius);
                }
            }
        }

        public float calculateRadius(float mass) 
        {
            return (float)Math.Sqrt(mass / Math.PI);
        }



    }
}
