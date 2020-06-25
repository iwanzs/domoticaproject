using GarduinoApp.Models;
using GarduinoApp.Views;
using System;
using Week4.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Week4
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 15000;
            timer.Elapsed += new BackgroundTimer().timer_Elapsed;
            timer.Start();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
