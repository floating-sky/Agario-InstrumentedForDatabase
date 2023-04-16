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
        //The dimensions of the area of the world the player is able to see.
        private int cameraHeight = 2000; 
        private int cameraWidth = 2000;

        //The dimensions of the player's physical screen.
        private int screenHeight = 800; //TODO: Set to whatever the current value of the screen.
        private int screenWidth = 800;

        public Dictionary<long, Food> foods { get; set; } = new Dictionary<long, Food>();
        public long userPlayerID;
        public Dictionary<long, Player> players { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (userPlayerID == 0) 
            {
                return;
            }

            cameraWidth = (int)(30*calculateRadius(players[userPlayerID].Mass)); //Adjust the camera to relate to the size of the player as the game continues.
            cameraHeight = (int)(30*calculateRadius(players[userPlayerID].Mass));

            canvas.FillColor = Colors.LightGray;
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

                    canvas.FontColor = Colors.Black;
                    canvas.FontSize = 15;
                    canvas.DrawString(player.Name, screen_X, screen_Y - radius - 5, HorizontalAlignment.Center);
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

            //divide the objects offset by the camera width to get the percentage of the screen
            //it is taking up then multiply by the dimensions of the users actual screen.
            screenX =  (int)(objectPanX / cameraWidth * screenWidth);
            screenY = (int)(objectPanY / cameraWidth * screenWidth);

            //Do the same operation above with the width and the height.
            screenW = (int)(worldW / cameraWidth * screenWidth);
            screenH = (int)(worldH / cameraHeight * screenHeight);
        }

        public void convert_from_screen_to_world(in float screenX, in float screenY, out int worldX, out int worldY)
        {
            //If the player is not found, just return zeroes.
            if (userPlayerID == 0 || !players.ContainsKey(userPlayerID))
            {
                worldX = 0;
                worldY = 0;
                return;
            }

            //Gives the amount of pixels from the camera's (0,0) our object is at.
            float objectOffsetX = screenX / screenWidth * cameraWidth;
            float objectOffsetY = screenY / screenHeight * cameraHeight;

            //Put the center of the screen wherever the player is.
            float screenCenterX = players[userPlayerID].X;
            float screenCenterY = players[userPlayerID].Y;

            //find where the (0,0) coordinate is of our camera view.
            float cameraLocationX = (float)(screenCenterX - 0.5 * cameraWidth);
            float cameraLocationY = (float)(screenCenterY - 0.5 * cameraHeight);

            //Add the object's offset to the (0,0) of our camera.
            worldX = (int)(objectOffsetX + cameraLocationX);
            worldY = (int)(objectOffsetY + cameraLocationY);

        }
    }
}
