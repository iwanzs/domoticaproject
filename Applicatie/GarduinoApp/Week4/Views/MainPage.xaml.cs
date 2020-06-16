using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GarduinoApp.Views;
using Week4.Models;
using Xamarin.Forms;

namespace Week4.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
//        protected override bool OnBackButtonPressed() => true;

        DatabaseManager databasemanager;

        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);


        }

    }
}
