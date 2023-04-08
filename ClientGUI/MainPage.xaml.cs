using Microsoft.Extensions.Logging;

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

    }
}