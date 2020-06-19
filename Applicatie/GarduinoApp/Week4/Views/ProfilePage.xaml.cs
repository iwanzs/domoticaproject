using GarduinoApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ProfilePage : ContentPage, INotifyPropertyChanged
    {
        protected override bool OnBackButtonPressed() => true;

        DatabaseManager databasemanager;

        public ProfilePage()
        {
            InitializeComponent();

            databasemanager = new DatabaseManager();

            NavigationPage.SetHasBackButton(this, false);

            GetProfiles();
        }

        public void GetProfiles()
        {
            List<Profiles> profiles = databasemanager.GetProfiles();

            ProfilesList.ItemsSource = profiles;
        }

        private void SelectProfile(object sender, ItemTappedEventArgs e)
        {
            var myItem = (Profiles)e.Item;
            databasemanager.GetProfileInformation(myItem.ID);
            Configuration.ProfileID = myItem.ID;
            Navigation.PushAsync(new MasterPage());
        }

        private void AddProfile(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddProfile());
        }

        private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {
            var item = (SwipeItem)sender;
            var param = (Profiles)item.CommandParameter;
            databasemanager.DeleteProfile(param.ID, param.UserID);

            GetProfiles();
        }

        private void OnEditSwipeItemInvoked(object sender, EventArgs e)
        {
            var item = (SwipeItem)sender;
            var param = (Profiles)item.CommandParameter;
            databasemanager.GetProfileInformation(param.ID);
            Navigation.PushAsync(new EditProfile());
        }
    }
}