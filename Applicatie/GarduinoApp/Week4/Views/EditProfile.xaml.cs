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

            NavigationPage.SetHasBackButton(this, false);

            databasemanager = new DatabaseManager();

            Profiles currentProfile = databasemanager.GetProfileInformation();
            profileID = currentProfile.ID;

            Name.Text = currentProfile.Name;
            Threshold.Text = (currentProfile.Threshold).ToString();
            IP.Text = (currentProfile.IP).ToString();
            Port.Text = (currentProfile.Port).ToString();
            ArduinoPinNumber.Text = (currentProfile.ArduinoPinNumber).ToString();
        }

        private void ChangeProfile(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(Name.Text) | string.IsNullOrEmpty(Threshold.Text) | string.IsNullOrEmpty(IP.Text) | string.IsNullOrEmpty(Port.Text) | string.IsNullOrEmpty(ArduinoPinNumber.Text))
            {
                Error.Text = "Please enter all the information";
                return;
            }
            else
            {
                databasemanager.EditProfile(profileID, Name.Text, Convert.ToInt32(Threshold.Text), IP.Text, Port.Text, Convert.ToInt32(ArduinoPinNumber.Text));
                Navigation.PushAsync(new ProfilePage());
            }
        }

        private void CancelProfile(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}