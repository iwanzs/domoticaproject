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

        public RegisterPage()
        {
            InitializeComponent();

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
    }
}