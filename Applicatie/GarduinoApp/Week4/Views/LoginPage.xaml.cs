using GarduinoApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Week4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        DatabaseManager databasemanager;

        public LoginPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            databasemanager = new DatabaseManager();
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            if (Username.Text == null | Password.Text == null)
                return;
            else if (databasemanager.DoesAccountExist(Username.Text, Password.Text) == false)
            {
                Error.Text = "This account does not exist";
                return;
            }
            else if (databasemanager.DoesAccountExist(Username.Text, Password.Text) == true)
            {
                Configuration.Username = Username.Text;
                Configuration.UserID = databasemanager.GetUser().UserID;
                Navigation.PushAsync(new ProfilePage());
            }
        }

        private void RegisterClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }
    }
}