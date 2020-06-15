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
            MovieList.ItemsSource = dbManager.GetAllMovies();
            SerieList.ItemsSource = dbManager.GetAllSeries();
        }

        private void MovieList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Movie)e.SelectedItem;
            int user = Configuration.UserID;
            dbManager.AddFavoriteMovie(user, item.MovieID);

        }

        private void SerieList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Series)e.SelectedItem;
            int user = Configuration.UserID;
            dbManager.AddFavoriteSeries(user, item.SeriesID);
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            UpdateLists();
        }
    }
}
