using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Windows.Graphics.Printing3D;
using Windows.Networking.Vpn;

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

        public MainPage(ILogger<MainPage> logger) 
        {
            _logger = logger;
            InitializeComponent();
            worldView= new WorldDrawable();
            initialized = false;
            userName = PlayerNameBox.Text;
            serverName = ServerNameBox.Text;
            port = 11000;

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
        }

        private void ReceiveFood(string message)
        {
            if (message.StartsWith(AgarioModels.Protocols.CMD_Food))
            {
                AgarioModels.Food[] foodsList = JsonSerializer.Deserialize<Food[]>(message.Replace(AgarioModels.Protocols.CMD_Food, ""))
                ?? throw new Exception("Invalid JSON");

                Dictionary<long, Food> foods = new Dictionary<long, Food>();
                foreach (Food food in foodsList)
                    foods.Add(food.ID, food);

                worldView.foods = foods;
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

                PlaySurface.Invalidate();
            }
        }

        /// <summary>
        /// Tracks the movement of the mouse; called with each mouse movement
        /// </summary>
        void PointerChanged(object sender, PointerEventArgs e)
        {
            Point position = (Point)e.GetPosition(this);
            
            
            client.Send(String.Format(Protocols.CMD_Move, position.X, position.Y));
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