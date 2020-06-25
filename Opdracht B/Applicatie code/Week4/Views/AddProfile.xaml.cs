﻿using GarduinoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week4;
using Week4.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Net.Sockets;

namespace GarduinoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProfile : ContentPage
    {
        DatabaseManager databasemanager;

        protected override bool OnBackButtonPressed() => true;

        public AddProfile()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);

            databasemanager = new DatabaseManager();
        }

        private void CreateProfile(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Name.Text) | string.IsNullOrEmpty(DevLocation.Text) | string.IsNullOrEmpty(Threshold.Text) | string.IsNullOrEmpty(IP.Text) | string.IsNullOrEmpty(Port.Text))
            {
                Error.Text = "Please enter all the information";
                return;
            }
            if (string.IsNullOrEmpty(ArduinoPinNumber.Text))
            {
                databasemanager.AddProfile(Name.Text, DevLocation.Text, Convert.ToInt32(Threshold.Text), IP.Text, Port.Text, 999);
            }
            else
            {
                databasemanager.AddProfile(Name.Text, DevLocation.Text, Convert.ToInt32(Threshold.Text), IP.Text, Port.Text, Convert.ToInt32(ArduinoPinNumber.Text));
            }

            List<Profiles> totalProfiles = databasemanager.GetProfiles();
            int lastProfileID = (totalProfiles.Count() - 1);
            Profiles profileInfo = totalProfiles[lastProfileID];
            Profiles profile = databasemanager.GetProfileInformation(profileInfo.ID);
            Socket socket = profile.ConnectSocket(profile.IP, profile.Port);
            Configuration.AddConnection(profile.ID, socket);

            Navigation.PushAsync(new ProfilePage());
        }

        private void CancelProfile(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}