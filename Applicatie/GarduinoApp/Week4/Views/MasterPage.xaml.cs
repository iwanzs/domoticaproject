using GarduinoApp.Models;
using GarduinoApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week4.MenuItems;
using Week4.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Week4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {

        List<MasterPageItem> MasterList = new List<MasterPageItem>();

        DatabaseManager databasemanager;

        public MasterPage()
        {
            InitializeComponent();

            databasemanager = new DatabaseManager();

            MenuPage.BackgroundColor = Color.FromHex("A6E3F6");

            Profiles currentProfile = databasemanager.GetProfileInformation();

            ProfileName.Text = currentProfile.Name;
            ProfileName.TextColor = Color.FromHex("2670B5");

            MasterList.Add(new MasterPageItem { Title = "Home", Icon = "HomeIcon.png", TargetType = typeof(MainPage) });
            MasterList.Add(new MasterPageItem { Title = "Weather", Icon = "WeatherIcon.png", TargetType = typeof(CurrentWeatherPage) });
            MasterList.Add(new MasterPageItem { Title = "History", Icon = "HistoryIcon.png", TargetType = typeof(HistoryPage) });
            MasterList.Add(new MasterPageItem { Title = "Profiles", Icon = "ProfileIcon.png", TargetType = typeof(ProfilePage) });
            MasterList.Add(new MasterPageItem { Title = "Logout", Icon = "LogoutIcon.png", TargetType = typeof(LoginPage) });
            NavigationDrawerList.ItemsSource = MasterList;
            NavigationPage.SetHasNavigationBar(this, false);
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(CurrentWeatherPage)));
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;

            if(page == typeof(LoginPage))
            {
                Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }
            else if(page == typeof(ProfilePage))
            {
                Navigation.PushModalAsync(new NavigationPage(new ProfilePage()));
            }

            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}