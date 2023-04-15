using AgarioModels;
using Microsoft.Graphics.Canvas.UI.Xaml;
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

        private int worldHeight = 5000;
        private int worldWidth = 5000;
        private int screenHeight = 3000;
        private int screenWidth = 3000;
        public Dictionary<long, Food> foods { get; set; }
        public long userPlayerID;
        public Dictionary<long, Player> players { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Gray;
            canvas.FillRectangle(dirtyRect);

            foreach (Food food in foods.Values) 
            {
                canvas.FillColor = Color.FromInt(food.ARGBColor);
                food.radius = calculateRadius(food.Mass); // radius = square root of area / pi

                //In order to get the width and height of a circle, you must multiply the radius by 2.
                convert_from_world_to_screen(food.X, food.Y, food.radius*2, food.radius*2, out int screen_X, out int screen_Y, out int width, out int height);
                //The radius is half the width of a circle.
                int radius = width / 2;
                canvas.FillCircle(screen_X, screen_Y, radius); 
            }

            if (players is not null)
            {
                foreach (Player player in players.Values)
                {
                    canvas.FillColor = Color.FromInt(player.ARGBColor);
                    player.radius = calculateRadius(player.Mass);
                    //In order to get the width and height of a circle, you must multiply the radius by 2.
                    convert_from_world_to_screen(player.X, player.Y, player.radius * 2, player.radius * 2, out int screen_X, out int screen_Y, out int width, out int height);
                    //The radius is half the width of a circle.
                    int radius = width / 2;
                    canvas.FillCircle(screen_X, screen_Y, radius);
                }
            }
            
        }

        public float calculateRadius(float mass) 
        {
            return (float)Math.Sqrt(mass / Math.PI);
        }

        /// <summary>
        /// This code converts between world and screen coordinates.
        ///
        /// Assumption: The world is 3000 wide and 2000 high. WARNING: never use magic numbers like these in
        /// your code. Always replaced by named constants that "live" somewhere appropriate.
        ///
        /// Assumption: we are drawing across the entire GUI window. WARNING: you probably will not do this
        /// in your program... leave room for some info displays.
        ///
        /// Assumption: We are drawing the entire world on the GUI window. WARNING: you will need to "shrink"
        /// the area of the "world" that is shown. Think about how to do this and ask questions
        /// in lecture.
        ///
        /// Algorithm: See Lab Writeup
        ///
        /// Notice: You should be able to explain why the world is in floats and the screen is in ints!
        /// </summary>
        /// <param name="world_x"></param>
        /// <param name="world_y"></param>
        /// <param name="world_w"></param>
        /// <param name="world_h"></param>
        /// <param name="screen_x"></param>
        /// <param name="screen_y"></param>
        /// <param name="screen_w"></param>
        /// <param name="screen_h"></param>

        private void convert_from_world_to_screen(
        in float world_x, in float world_y, in float world_w, in float world_h,
        out int screen_x, out int screen_y, out int screen_w, out int screen_h)
        {
            screen_x = (int)(world_x / worldWidth * screenWidth);
            screen_y = (int)(world_y / worldHeight * screenHeight);
            screen_w = (int)(world_w / worldWidth * screenWidth);
            screen_h = (int)(world_h / worldHeight * screenHeight);
        }


    }
}
