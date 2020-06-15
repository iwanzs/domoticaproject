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


        public MasterPage()
        {
            InitializeComponent();
            AccountName.Text = Configuration.Username;
            MasterList.Add(new MasterPageItem { Title = "Movies & Series", Icon = "icon.jpg", TargetType = typeof(MainPage) });
            MasterList.Add(new MasterPageItem { Title = "Favorites", Icon = "icon.jpg", TargetType = typeof(FavoritePage) });
            MasterList.Add(new MasterPageItem { Title = "Add", Icon = "icon.jpg", TargetType = typeof(AddPage) });
            NavigationDrawerList.ItemsSource = MasterList;
            NavigationPage.SetHasNavigationBar(this, false);
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPage)));
        }

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }
    }
}