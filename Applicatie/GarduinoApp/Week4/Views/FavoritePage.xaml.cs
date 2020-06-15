using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week4.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Week4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritePage : ContentPage
    {
        public FavoritePage()
        {
            InitializeComponent();
            UpdateLists();
        }

        DatabaseManager dbManager = new DatabaseManager();

        private void UpdateLists()
        {
            MovieList.ItemsSource = dbManager.GetAllMoviesByUser();
            SerieList.ItemsSource = dbManager.GetAllSeriesByUser();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            UpdateLists();
        }

        private void MovieList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Movie)e.SelectedItem;
            dbManager.DeleteFavoriteMovie(Configuration.UserID, item.MovieID);
            UpdateLists();
        }

        private void SerieList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (Series)e.SelectedItem;
            dbManager.DeleteFavoriteSeries(Configuration.UserID, item.SeriesID);
            UpdateLists();
        }
    }
}