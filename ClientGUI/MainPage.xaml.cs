using Microsoft.Extensions.Logging;
using Windows.Networking.Vpn;

namespace ClientGUI
{
    public partial class MainPage : ContentPage
    {
        private readonly ILogger<MainPage> _logger;

        public MainPage(ILogger<MainPage> logger) 
        {
            _logger = logger;
            InitializeComponent();
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

        void PanUpdated(object sender, PanUpdatedEventArgs e) { }
    }
}