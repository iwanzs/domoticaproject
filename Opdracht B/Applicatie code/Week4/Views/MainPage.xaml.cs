using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Text;
using GarduinoApp.Models;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Week4;

namespace GarduinoApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        DatabaseManager databasemanager = new DatabaseManager();
        Profiles currentProfile;
        private Connection connection;

        public MainPage()
        {
            InitializeComponent();

            currentProfile = databasemanager.GetProfileInformation(Configuration.ProfileID);

            connection = Configuration.GetConnection(currentProfile.ID);
            currentProfile.SetSocket(connection);

            lblDevice.Text = currentProfile.Name;
            lblDevice.TextColor = Color.FromHex("2670B5");

            if (currentProfile.IsConnected())
            {
                btnConnectionState.BackgroundColor = Color.Green;

                if (currentProfile.GetResponse("s") == " ON")
                {
                    swhOnOff.Toggled -= SwhOnOff_OnToggled;
                    swhOnOff.IsToggled = true;
                    swhOnOff.Toggled += SwhOnOff_OnToggled;
                }
            }



            if (currentProfile.AutoEnabled == 1)
            {
                swhAuto.IsToggled = true;
                swhOnOff.IsEnabled = false;
            }

            lblTempCurrent.Text = currentProfile.Threshold.ToString();
            sldrTemp.Value = currentProfile.Threshold;
        }

        private void SwhAuto_OnToggled(object sender, ToggledEventArgs e)
        {
            if (swhAuto.IsToggled)
            {
                swhOnOff.IsEnabled = false;
                databasemanager.SetAutoEnabled(1, currentProfile.ID);
            }
            else
            {
                swhOnOff.IsEnabled = true;
                databasemanager.SetAutoEnabled(0, currentProfile.ID);
            }
        }

        private void SwhOnOff_OnToggled(object sender, ToggledEventArgs e)
        {
            if (!currentProfile.ToggleProfile())
            {
                swhOnOff.Toggled -= SwhOnOff_OnToggled;
                swhOnOff.IsToggled = false;
                swhOnOff.Toggled += SwhOnOff_OnToggled;
            }
        }

        private void SldrTemp_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblTempCurrent.Text = Convert.ToInt32(sldrTemp.Value).ToString();
            databasemanager.SetThreshold((int)sldrTemp.Value, currentProfile.ID);
        }

    }
}
