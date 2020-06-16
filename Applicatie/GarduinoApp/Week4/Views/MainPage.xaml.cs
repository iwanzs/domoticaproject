using GarduinoApp.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Week4.Models;
using Xamarin.Forms;

namespace Week4.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        protected override bool OnBackButtonPressed() => true;

        DatabaseManager databasemanager;

        public MainPage()
        {
            InitializeComponent();
//            UpdateLists();
//            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
//            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#0080FF");

            //            List<Devices> profiles = databasemanager.GetProfiles();
            //
            //            DevicesList.ItemsSource = profiles;


            //SetValue(NavigationPage.HasNavigationBarProperty, false);

        }

        private void UpdateLists()
        {
            // Update of iets
        }

        private void UpdateClicked(object sender, System.EventArgs e)
        {
            UpdateLists();
        }

        private void HistoryClicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new HistoryPage());
        }
    }
}
