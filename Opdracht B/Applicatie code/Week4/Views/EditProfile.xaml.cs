using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week4;
using Week4.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System;
using Xamarin.Forms.Internals;
using GarduinoApp.Models;
using System.Net.Sockets;

namespace GarduinoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfile : ContentPage
    {
        protected override bool OnBackButtonPressed() => true;

        DatabaseManager databasemanager;

        int profileID;

        public EditProfile()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            NavigationPage.SetHasBackButton(this, false);

            databasemanager = new DatabaseManager();

            Profiles currentProfile = databasemanager.GetProfileInformation(Configuration.ProfileID);
            profileID = currentProfile.ID;

            Name.Text = currentProfile.Name;
            DevLocation.Text = currentProfile.Location;
            Threshold.Text = (currentProfile.Threshold).ToString();
            IP.Text = (currentProfile.IP).ToString();
            Port.Text = (currentProfile.Port).ToString();
            ArduinoPinNumber.Text = (currentProfile.ArduinoPinNumber).ToString();
        }

        private void ChangeProfile(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(Name.Text) | string.IsNullOrEmpty(DevLocation.Text) | string.IsNullOrEmpty(Threshold.Text) | string.IsNullOrEmpty(IP.Text) | string.IsNullOrEmpty(Port.Text))
            {
                Error.Text = "Please enter all the information";
                return;
            }
            if (string.IsNullOrEmpty(ArduinoPinNumber.Text))
            {
                databasemanager.EditProfile(profileID, Name.Text, DevLocation.Text, Convert.ToInt32(Threshold.Text), IP.Text, Port.Text, 999);
            }
            else
            {
                databasemanager.EditProfile(profileID, Name.Text, DevLocation.Text, Convert.ToInt32(Threshold.Text), IP.Text, Port.Text, Convert.ToInt32(ArduinoPinNumber.Text));
            }

            Profiles profile = databasemanager.GetProfileInformation(profileID);
            Configuration.RemoveConnection(profile.ID);
            Socket socketAdd = profile.ConnectSocket(profile.IP, profile.Port);
            Configuration.AddConnection(profile.ID, socketAdd);

            Navigation.PushAsync(new ProfilePage());
        }

        private void CancelProfile(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}