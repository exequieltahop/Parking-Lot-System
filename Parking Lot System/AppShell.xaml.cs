//using Android.Net.Wifi.Aware;
using Microsoft.Maui.Controls;
using Parking_Lot_System.Services;

namespace Parking_Lot_System
{
    public partial class AppShell : Shell
    {
        private CheckStorageForSessionAuth AuthChecker = new();

        public AppShell()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Check user login status
            bool session_status = GetUserLogInStatusToken();

            if (session_status == true)
            {
                await NavigateToHomePage();
            }
            else
            {
                await DisplayAlert("Alert", "ok", "ok");
            }
        }

        // Alert method
        public async void alertt()
        {
            await DisplayAlert("Sample", "Sample Alert", "OK");
        }

        // Get user preferences for status
        private bool GetUserLogInStatusToken()
        {
            // Ensure that CheckAuth is correctly implemented and returns a valid boolean
            return AuthChecker.CheckAuth("username");
        }

        // Method to navigate to the home page
        private async Task NavigateToHomePage()
        {
            // Assuming "HomePage" is the route name defined in your Shell
            await Shell.Current.GoToAsync("//Home");
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            // Call your logout logic
            bool logout_status = AuthChecker.LogOut("username", "password");

            if (logout_status == true)
            {
                await Shell.Current.GoToAsync("//SignIn"); // Navigate back to Sign-In page
            }
            else
            {
                await DisplayAlert("Error", "Logout failed. Please try again.", "OK");
            }
        }

        // history navigate
        private async void HistoryClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//History");
        }

        // history navigate
        private async void HomeClicked(object sender, EventArgs e) 
        {
            await Shell.Current.GoToAsync("//Home");
        }

        // statistics navigate
        private async void OnStatisticsClicked(object sender, EventArgs e) 
        {
            await Shell.Current.GoToAsync("//Statistics");
        }
    }
}
