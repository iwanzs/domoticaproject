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
        DatabaseManager dbManager = new DatabaseManager();
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void RegisterClicked(object sender, EventArgs e)
        {
            if (UsernameInput.Text != "" && PasswordInput.Text != "")
            {
                if (!dbManager.DoesAccountExist(UsernameInput.Text, PasswordInput.Text))
                {
                    dbManager.AddUser(UsernameInput.Text, PasswordInput.Text);
                    Configuration.Username = UsernameInput.Text;
                    Configuration.UserID = dbManager.GetUser().UserID;
                    Navigation.PushAsync(new MasterPage());
                }
            }
        }
    }
}