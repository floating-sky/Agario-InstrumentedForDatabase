using System.Diagnostics;
using System.Timers;

namespace TowardAgarioStepOne
{
    public partial class MainPage : ContentPage
    {
        static WorldDrawable world;
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
            world = (WorldDrawable)PlaySurface.Drawable;
            world.model = new();
            //Window.Width = 500;
            System.Timers.Timer timer = new(2000);
            timer.Start();
            timer.Elapsed += GameStep;
        }

        private void GameStep(object sender, ElapsedEventArgs e)
        {
            world.model.AdvanceGameOneStep();
            PlaySurface.Invalidate();
            circleCenter.Text += world.model.circlePosition;
            direction.Text += world.model.direction;
        }
    }


}