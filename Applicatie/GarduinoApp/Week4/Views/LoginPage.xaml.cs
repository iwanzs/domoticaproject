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
        DatabaseManager dbManager = new DatabaseManager();

        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginClicked(object sender, EventArgs e)
        {
            if (UsernameInput.Text != "" && PasswordInput.Text != "")
            {
                if (dbManager.DoesAccountExist(UsernameInput.Text, PasswordInput.Text))
                {
                    Configuration.Username = UsernameInput.Text;
                    Configuration.UserID = dbManager.GetUser().UserID;
                    Navigation.PushAsync(new MasterPage());
                }
            }
        }

        private void RegisterClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }
    }
}