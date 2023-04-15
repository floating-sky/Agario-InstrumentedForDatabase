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
    /// <summary>
    /// Author:    Julia Thomas
    /// Partner:   C Wyatt Bruchhauser
    /// Date:      4-16-2022
    /// Course:    CS 3500, University of Utah, School of Computing
    /// Copyright: CS 3500 and C Wyatt Bruchhauser - This work may not 
    ///            be copied for use in Academic Coursework.
    ///
    /// I, C Wyatt Bruchhauser and Julia Thomas, certify that I wrote this code from scratch and
    /// did not copy it in part or whole from another source.  All 
    /// references used in the completion of the assignments are cited 
    /// in my README file.
    ///
    /// File Contents
    /// This file represents the view of the world the player will see. The player will always be at the center of the screen
    /// and as they gain more mass the camera will pan out more and more to let the see the world better.
    /// </summary>
    internal class WorldDrawable : IDrawable
    {
        //The dimensions of the world we are playing the game in.
        private int worldHeight = 5000;
        private int worldWidth = 5000;

        //The dimensions of the area of the world the player is able to see.
        private int cameraHeight = 2000; //TODO:Set to some value based on the player's radius.
        private int cameraWidth = 2000;

        //The dimensions of the player's physical screen.
        private int screenHeight = 800; //TODO: Set to whatever the current value of the screen.
        private int screenWidth = 800;

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

        private void convert_from_world_to_screen(
        in float worldX, in float worldY, in float worldW, in float worldH,
        out int screenX, out int screenY, out int screenW, out int screenH)
        {
            //Put the center of the screen wherever the player is.
            float screenCenterX = players[userPlayerID].X;
            float screenCenterY = players[userPlayerID].Y;

            //find where the (0,0) coordinate is of our camera view.
            float cameraLocationX = (float)(screenCenterX - 0.5*cameraWidth);
            float cameraLocationY = (float)(screenCenterY - 0.5*cameraHeight);

            //find the offset of whatever object we want to draw in relation to the camera.
            float objectPanX = worldX - cameraLocationX;
            float objectPanY = worldY - cameraLocationY;

            if (objectPanX <0 || objectPanX > cameraWidth || objectPanY <0 || objectPanY > cameraHeight)
            {
                Console.WriteLine("Not drawn");
            }
            //divide the objects offset by the camerawidth to get the percentage of the screen
            //it is taking up then multiply by the dimensions of the users actual screen.
            screenX =  (int)(objectPanX / cameraWidth * screenWidth);
            screenY = (int)(objectPanY / cameraWidth * screenWidth);

            //Do the same operation above with the width and the height.
            screenW = (int)(worldW / cameraWidth * screenWidth);
            screenH = (int)(worldH / cameraHeight * screenHeight);
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

        private void convert_from_world_to_screen2(
        in float world_x, in float world_y, in float world_w, in float world_h,
        out int screen_x, out int screen_y, out int screen_w, out int screen_h)
        {
            screen_x = (int)(world_x / worldWidth * cameraWidth);
            screen_y = (int)(world_y / worldHeight * cameraHeight);
            screen_w = (int)(world_w / worldWidth * cameraWidth);
            screen_h = (int)(world_h / worldHeight * cameraHeight);
        }


    }
}
