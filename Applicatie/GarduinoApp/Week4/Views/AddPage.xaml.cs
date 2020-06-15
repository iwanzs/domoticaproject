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
    public partial class AddPage : ContentPage
    {
        string TappedSelected = "Series";

        DatabaseManager dbManager = new DatabaseManager();
        public AddPage()
        {
            InitializeComponent();
        }
        private void SelectMovieClicked(object sender, EventArgs e)
        {
            TappedSelected = "Movie";
            SeriesButton.Opacity = 0.5;
            MovieButton.Opacity = 1;
            YearEnded.IsVisible = false;
            NumberOfEpisodes.IsVisible = false;
            NumberOfSeasons.IsVisible = false;
            YearEndedLabel.IsVisible = false;
            NumberOfEpisodesLabel.IsVisible = false;
            NumberOfSeasonsLabel.IsVisible = false;
        }

        private void SelectSeriesClicked(object sender, EventArgs e)
        {
            TappedSelected = "Series";
            SeriesButton.Opacity = 1;
            MovieButton.Opacity = 0.5;
            YearEnded.IsVisible = true;
            NumberOfEpisodes.IsVisible = true;
            NumberOfSeasons.IsVisible = true;
            YearEndedLabel.IsVisible = true;
            NumberOfEpisodesLabel.IsVisible = true;
            NumberOfSeasonsLabel.IsVisible = true;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            if (TappedSelected == "Series")
            {
                dbManager.AddSeries(TitleInput.Text, DescriptionInput.Text, 2010, 2020, 5, 5);
            }
            else if (TappedSelected == "Movie")
            {
                dbManager.AddMovie(TitleInput.Text, DescriptionInput.Text, Convert.ToInt32(YearInput.Text));
            }
        }
    }
}