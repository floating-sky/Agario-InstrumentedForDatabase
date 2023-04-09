using Microsoft.Maui.Animations;
using System.Diagnostics;
using System.Timers;

namespace TowardAgarioStepOne
{
    public partial class MainPage : ContentPage
    {
        static WorldDrawable world = new();
        bool initialized = false;
        
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        ///    Called when the window is resized.  
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
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
            System.Timers.Timer timer = new System.Timers.Timer();
            PlaySurface.Drawable = world;
            world.model = new();
            //Window.Width = 500;
            timer.Elapsed += new ElapsedEventHandler(GameStep);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void GameStep(object source, ElapsedEventArgs e)
        {
            world.model.AdvanceGameOneStep();
            PlaySurface.Invalidate();
            //circleCenter.Text += world.model.circlePosition;
            //direction.Text += world.model.direction;
        }
    }


}