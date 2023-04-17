using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Windows.Graphics.Printing3D;
using Windows.Media.Streaming.Adaptive;
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
            worldView = new WorldDrawable();
            userName = PlayerNameBox.Text;
            serverName = ServerNameBox.Text;
            port = 11000;
            graphicsViewTopLeft = new Point(560,0);
        }

        /// <summary>
        /// Saves what is in the player name entry box into a variable when enter is pressed on it.
        /// </summary>
        void PlayerNameBoxChanged(object sender, EventArgs e)
        {
            userName = PlayerNameBox.Text;
        }

        /// <summary>
        /// Saves what is in the server name entry box into a variable when enter is pressed on it.
        /// </summary>
        void ServerNameBoxChanged(Object sender, EventArgs e)
        {
            serverName = ServerNameBox.Text;
        }

        /// <summary>
        /// When the connect to server connect button is clicked, this method tries to connect the client to the server,
        /// await messages from the server, and sends a message to the server that the game has started.
        /// </summary>
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

        /// <summary>
        /// Switches from the welcome screen to the game view when the client connects to the server.
        /// </summary>
        /// <param name="channel">The networking object representing what is being connected to</param>
        public void OnConnect(Networking channel) 
        {
            WelcomeScreen.IsVisible = false;
            GameScreen.IsVisible = true;

            PlaySurface.Drawable = worldView;
        }

        /// <summary>
        /// Displays an error message to the user when the client disconnects from the server.
        /// </summary>
        /// <param name="channel">The networking object representing what is being disconnected from</param>
        public void OnDisconnect(Networking channel) 
        {
            PlayDebugMessage.Text = "Player disconnected from server";
        }

        /// <summary>
        /// When the client receives a message, this method checks if it matches any of the protocols.
        /// </summary>
        /// <param name="channel">The networking object representing where the message is coming from</param>
        /// <param name="message">The message that was sent</param>
        public void OnMessage(Networking channel, string message) 
        {
            ReceiveDeadPlayers(message);
            ReceivePlayerID(message);
            ReceiveAllPlayers(message);
            ReceiveHeartbeat(message);
            ReceiveFood(message);
            ReceiveFoodEaten(message); 
        }

        /// <summary>
        /// If the message contains the receive food protocol, it gets deserialized and its contents get put into the foods dictionary.
        /// </summary>
        /// <param name="message">The message being checked for if it matches the protocol</param>
        /// <exception cref="Exception">Thrown if the JSON is improperly formatted</exception>
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

        /// <summary>
        /// If the message contains the playerID protocol, the ID is saved into a variable.
        /// </summary>
        /// <param name="message">The message being checked for if it matches the protocol</param>
        private void ReceivePlayerID(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Player_Object))
            {
                long playerID = JsonSerializer.Deserialize<long>(message.Replace(AgarioModels.Protocols.CMD_Player_Object, ""));

                worldView.userPlayerID = playerID;
            }
        }

        /// <summary>
        /// If the message contains the receive all players protocol, its deserialized contents gets put into the players list
        /// </summary>
        /// <param name="message">The message being checked for if it matches the protocol</param>
        /// <exception cref="Exception">Thrown if the JSON is improperly formatted</exception>
        private void ReceiveAllPlayers(string message) 
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Update_Players)) 
            {
                AgarioModels.Player[] playersList = JsonSerializer.Deserialize<Player[]>(message.Replace(AgarioModels.Protocols.CMD_Update_Players, ""))
                ?? throw new Exception("Invalid JSON");

                Dictionary<long, Player> players = new Dictionary<long, Player>();
                foreach (Player player in playersList)
                {
                    players.Add(player.ID, player);
                }
                worldView.players = players;
            }
        }

        /// <summary>
        /// If the message contains the heartbeat protocol, this method deserializes it and puts the heartbeat count into a variable. It
        /// then displays the most current game statistics to the GUI, sends the move protocol to the server, and refocuses on the split
        /// button so that space bar will always split the player if they meet the right size threshold.
        /// </summary>
        /// <param name="message">The message being checked for if it matches the protocol</param>
        private void ReceiveHeartbeat(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_HeartBeat))
            {
                int heartbeatCount = JsonSerializer.Deserialize<int>(message.Replace(AgarioModels.Protocols.CMD_HeartBeat, ""));

                worldView.convert_from_screen_to_world((float)(mousePosition.X -graphicsViewTopLeft.X), (float)(mousePosition.Y - graphicsViewTopLeft.Y), out int worldMouseX, out int worldMouseY);
                
                if (worldView.userPlayerID != 0 && worldView.players.ContainsKey(worldView.userPlayerID))
                {
                    GameStatistics.Text = $"Mass: {worldView.players[worldView.userPlayerID].Mass}" +
                        $"\nCoordinates: {(int)worldView.players[worldView.userPlayerID].X}, {(int)worldView.players[worldView.userPlayerID].Y}" +
                        $"\nMouse Position: {(int)(mousePosition.X - graphicsViewTopLeft.X)}, {(int)(mousePosition.Y - graphicsViewTopLeft.Y)}" +
                        $"\nHeartbeat: {heartbeatCount}";

                    PlaySurface.Invalidate();
                }

                client.Send(String.Format(Protocols.CMD_Move, worldMouseX, worldMouseY)); //Convert posX and posY into world coordinates.
                SplitButton.Focus();
            }
        }

        /// <summary>
        /// If the message contains the food eaten protocol, the eaten food is removed from the food list.
        /// </summary>
        /// <param name="message">The message being checked for if it matches the protocol</param>
        private void ReceiveFoodEaten(string message)
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

        /// <summary>
        /// If the message contains the dead players protocol, the dead players are removed from the players list. If one of the dead
        /// players is the user, then we show a game over message.
        /// </summary>
        /// <param name="message">The message being checked for if it matches the protocol</param>
        private void ReceiveDeadPlayers(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Dead_Players))
            {
                int[] deadPlayers = JsonSerializer.Deserialize<int[]>(message.Replace(AgarioModels.Protocols.CMD_Dead_Players, ""));

                foreach (long playerID in deadPlayers)
                {
                   
                    if (worldView.userPlayerID == playerID)
                    {
                        DeathMessage();
                    }
                    worldView.players.Remove(playerID);
                }
            }
        }

        /// <summary>
        /// Displays a message saying that the player has died. The user can then choose to keep playing or exit the game.
        /// </summary>
        private async void DeathMessage()
        {
            bool keepPlaying = await DisplayAlert("You died!", $"Final Mass: {worldView.players[worldView.userPlayerID].Mass}", "Restart Game", "Quit");
            if (keepPlaying)
            {
                client.Send(String.Format(Protocols.CMD_Start_Game, userName));
            }
            else
            {
                Application.Current?.CloseWindow(Application.Current.MainPage.Window);
            }
        }

        /// <summary>
        /// Tracks the movement of the mouse; called with each mouse movement.
        /// </summary>
        void PointerChanged(object sender, PointerEventArgs e)
        {
            mousePosition = (Point)e.GetPosition(this);
        }

        /// <summary>
        /// Runs when the split button is clicked. Gets the mouse coordinates and splits into that direction by sending the split protocol
        /// to the server.
        /// </summary>
        private void SplitButtonClicked(object sender, EventArgs e)
        {
            worldView.convert_from_screen_to_world((float)(mousePosition.X - graphicsViewTopLeft.X), (float)(mousePosition.Y - graphicsViewTopLeft.Y), out int worldMouseX, out int worldMouseY);
            client.Send(String.Format(Protocols.CMD_Split, (int)(worldMouseX), (int)(worldMouseY)));
        }
    }
}