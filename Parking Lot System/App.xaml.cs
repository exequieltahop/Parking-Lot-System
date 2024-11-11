using Parking_Lot_System.Services;
using Microsoft.Maui.Controls; 
using Microsoft.Maui.Storage; 

namespace Parking_Lot_System
{
    public partial class App : Application
    {
        private readonly UserLoginStatus _userLoginStatus;

        public App(UserLoginStatus userLoginStatus)
        {
            _userLoginStatus = userLoginStatus ?? throw new ArgumentNullException(nameof(userLoginStatus));
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
