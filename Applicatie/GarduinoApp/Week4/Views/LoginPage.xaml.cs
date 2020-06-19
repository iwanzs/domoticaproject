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

        protected override bool OnBackButtonPressed() => true;

        public LoginPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            LoginButton.BorderColor = Color.FromHex("6b4031");
            LoginButton.TextColor = Color.FromHex("543226");
            RegisterText.TextColor = Color.FromHex("362019");

            databasemanager = new DatabaseManager();
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Username.Text) | string.IsNullOrEmpty(Password.Text))
            {
                Error.Text = "Please enter all credentials";
                return;
            }
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