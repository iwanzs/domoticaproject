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
using SQLite;

namespace GarduinoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProfile : ContentPage
    {
        DatabaseManager databasemanager;

        public AddProfile()
        {
            InitializeComponent();

            databasemanager = new DatabaseManager();
        }

        private void CreateProfile(object sender, EventArgs e)
        {
            if (Name.Text == null | Threshold.Text == null | IP.Text == null | Port.Text == null | ArduinoPinNumber == null)
            {
                Error.Text = "Please enter all the information";
                return;
            }
            else
            {
                databasemanager.AddProfile(Name.Text, Convert.ToInt32(Threshold.Text), IP.Text, Port.Text, Convert.ToInt32(ArduinoPinNumber.Text));
                Navigation.PushAsync(new ProfilePage());
            }
        }
    }
}