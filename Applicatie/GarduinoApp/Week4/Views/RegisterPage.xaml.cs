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
    public partial class RegisterPage : ContentPage
    {
        DatabaseManager databasemanager;

        protected override bool OnBackButtonPressed() => true;

        public RegisterPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            SetColors();

            databasemanager = new DatabaseManager();
        }

        private void RegisterClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Username.Text) | string.IsNullOrEmpty(Password.Text) | string.IsNullOrEmpty(RepeatPassword.Text))
            {
                Error.Text = "Please enter all credentials";
                return;
            }
            else if (Password.Text != RepeatPassword.Text)
            {
                Error.Text = "The password does not match";
                return;
            }
            else if (databasemanager.AddUser(Username.Text, Password.Text) == false)
            {
                Error.Text = "This username already exists";
                return;
            }
            else if (databasemanager.AddUser(Username.Text, Password.Text) == true)
            {
                databasemanager.AddUser(Username.Text, Password.Text);
            }
            Navigation.PushAsync(new LoginPage());
        }

        public void SetColors()
        {
            RegisterButton.BorderColor = Color.FromHex("6b4031");
            RegisterButton.TextColor = Color.FromHex("543226");
            LoginText.TextColor = Color.FromRgb(220, 220, 220);
            Error.TextColor = Color.FromRgb(220, 220, 220);
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}