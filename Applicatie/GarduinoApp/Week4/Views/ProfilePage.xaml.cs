using GarduinoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week4;
using Week4.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarduinoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        protected override bool OnBackButtonPressed() => true;

        DatabaseManager databasemanager;

        public ProfilePage()
        {
            InitializeComponent();

            databasemanager = new DatabaseManager();

            NavigationPage.SetHasNavigationBar(this, false);

            List<Profiles> profiles = databasemanager.GetProfiles();

            ProfilesList.ItemsSource = profiles;
        }

        private void SelectProfile(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new MasterPage());
        }

        private void AddProfile(object sender, EventArgs e)
        {

        }
    }
}