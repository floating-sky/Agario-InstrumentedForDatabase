﻿using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Windows.Graphics.Printing3D;
using Windows.Networking.Vpn;

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
///    This class represents the GUI that our client will see when they start up the game. There are two pages,
///    the first is our welcome page where the user will be able to set their name and connect to their server.
///    The second is the game screen where players will be able to play the game. The screens are toggled
///    on and off using the isVisible property.
/// </summary>
namespace ClientGUI
{
    public partial class MainPage : ContentPage
    {
        private readonly ILogger<MainPage> _logger;
        private WorldDrawable worldView;
        private bool initialized;
        private String userName;
        private string serverName;
        private int port;
        Networking client;
        private Point mousePosition;
        private Point graphicsViewTopLeft; //Stores the relative coordinates of the graphics view at (0,0)
        public MainPage(ILogger<MainPage> logger) 
        {
            _logger = logger;
            InitializeComponent();
            worldView= new WorldDrawable();
            initialized = false;
            userName = PlayerNameBox.Text;
            serverName = ServerNameBox.Text;
            port = 11000;
            graphicsViewTopLeft = new Point(560,0);
        }

        void PlayerNameBoxChanged(object sender, EventArgs e)
        {
            userName = PlayerNameBox.Text;
        }

        void ServerNameBoxChanged(Object sender, EventArgs e)
        {
            serverName = ServerNameBox.Text;
        }

        void ConnectToServerButtonClicked(object sender, EventArgs e) 
        {
            client = new Networking(_logger, OnConnect, OnDisconnect, OnMessage, '\n');

            try { 
                client.Connect(serverName, port); }
            catch (Exception) {
                DebugMessage.Text = "Could not connect to server";
                return;
            }

            client.AwaitMessagesAsync();
            client.Send(String.Format(Protocols.CMD_Start_Game, userName));
        }

        public void OnConnect(Networking channel) 
        {
            WelcomeScreen.IsVisible = false;
            GameScreen.IsVisible = true;

            InitializeGameLogic();
        }

        private void InitializeGameLogic()
        {
            PlaySurface.Drawable = worldView;
        }

        public void OnDisconnect(Networking channel) 
        {
        
        }

        public void OnMessage(Networking channel, string message) 
        {
            ReceiveFood(message);
            ReceivePlayerID(message);
            ReceiveAllPlayers(message);
            RecieveHeartbeat(message);
            RecieveFoodEaten(message);
            RecieveDeadPlayers(message);
        }

        private void ReceiveFood(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Food))
            {
                AgarioModels.Food[] foodsList = JsonSerializer.Deserialize<Food[]>(message.Replace(AgarioModels.Protocols.CMD_Food, ""))
                ?? throw new Exception("Invalid JSON");

               
                foreach (Food food in foodsList)
                {
                    worldView.foods.Add(food.ID, food);
                }  
            }
        }

        private void ReceivePlayerID(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Player_Object))
            {
                long playerID = JsonSerializer.Deserialize<long>(message.Replace(AgarioModels.Protocols.CMD_Player_Object, ""));

                worldView.userPlayerID = playerID;
            }
        }

        private void ReceiveAllPlayers(string message) 
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Update_Players)) 
            {
                AgarioModels.Player[] playersList = JsonSerializer.Deserialize<Player[]>(message.Replace(AgarioModels.Protocols.CMD_Update_Players, ""))
                ?? throw new Exception("Invalid JSON");

                Dictionary<long, Player> players = new Dictionary<long, Player>();
                foreach (Player player in playersList)
                    players.Add(player.ID, player);

                worldView.players = players;
            }
        }

        private void RecieveHeartbeat(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_HeartBeat))
            {
                int heartBeatCount = JsonSerializer.Deserialize<int>(message.Replace(AgarioModels.Protocols.CMD_HeartBeat, ""));
                //?? throw new Exception("Invalid JSON");

                worldView.convert_from_screen_to_world((float)(mousePosition.X -graphicsViewTopLeft.X), (float)(mousePosition.Y - graphicsViewTopLeft.Y), out int worldMouseX, out int worldMouseY);

                
                if (worldView.userPlayerID != 0) //DEBUG
                PlayDebugMessage.Text = $"mouse {mousePosition.X - graphicsViewTopLeft.X} , {mousePosition.X - graphicsViewTopLeft.Y} \nworld {worldMouseX} , {worldMouseY} \nrelative {worldView.players[worldView.userPlayerID].X - worldMouseX} , {worldView.players[worldView.userPlayerID].Y - worldMouseY}";
                
                client.Send(String.Format(Protocols.CMD_Move, worldMouseX, worldMouseY)); //Convert posX and posY into world coordinates.
                PlaySurface.Invalidate();
            }
        }

        private void RecieveFoodEaten(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Eaten_Food))
            {
                List<long> foodsEaten = JsonSerializer.Deserialize<List<long>>(message.Replace(AgarioModels.Protocols.CMD_Eaten_Food, ""));

                foreach (long foodID in foodsEaten)
                {
                    worldView.foods.Remove(foodID);
                }
            }
            
        }

        private void RecieveDeadPlayers(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Dead_Players))
            {
                int[] deadPlayers = JsonSerializer.Deserialize<int[]>(message.Replace(AgarioModels.Protocols.CMD_Dead_Players, ""));

                foreach (long playerID in deadPlayers)
                {
                    worldView.foods.Remove(playerID);
                }
            }

        }
        /// <summary>
        /// Tracks the movement of the mouse; called with each mouse movement
        /// </summary>
        void PointerChanged(object sender, PointerEventArgs e)
        {
            mousePosition = (Point)e.GetPosition(this);
        }

        /// <summary>
        /// Runs when mouse2 is pressed on phone screen is tapped
        /// </summary>
        void OnTap(object sender, TappedEventArgs args)
        {
        }

        /// <summary>
        /// Called when phone user moves finger across screen (not needed)
        /// </summary>
        void PanUpdated(object sender, PanUpdatedEventArgs e) 
        { 
            //TODO: Remove if not useful
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            Debug.WriteLine($"OnSizeAllocated {width} {height}");

            if (!initialized)
            {
                initialized = true;
                InitializeGameLogic();
            }
        }


    }
}