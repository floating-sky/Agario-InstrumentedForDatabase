using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
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

        public MainPage(ILogger<MainPage> logger) 
        {
            _logger = logger;
            InitializeComponent();
            worldView= new WorldDrawable();
            initialized = false;
            userName = PlayerNameBox.Text;
            serverName = ServerNameBox.Text;

        }

        void PlayerNameBoxChanged(object sender, EventArgs e)
        {
            userName= PlayerNameBox.Text;
        }

        void ServerNameBoxChanged(Object sender, EventArgs e)
        {
            serverName = ServerNameBox.Text;
        }

        void ConnectToServerButtonClicked(object sender, EventArgs e) 
        {
            WelcomeScreen.IsVisible = false;
            GameScreen.IsVisible = true;
        }

        void PointerChanged(object sender, PointerEventArgs e) 
        {

        }

        void OnTap(object sender, TappedEventArgs args)
        {

        }

        void PanUpdated(object sender, PanUpdatedEventArgs e) 
        { 
        
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

        private void InitializeGameLogic()
        {
            PlaySurface.Drawable = worldView;
        }


    }
}