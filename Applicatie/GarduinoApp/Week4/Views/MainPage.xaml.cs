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

        DatabaseManager dbManager = new DatabaseManager();

        public MainPage()
        {
            InitializeComponent();
            UpdateLists();
        }

        private void UpdateLists()
        {
            // Update of iets
        }

        private void UpdateClicked(object sender, System.EventArgs e)
        {
            UpdateLists();
        }
    }
}
