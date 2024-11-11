using System.Net.Http;
using Newtonsoft.Json;
using Parking_Lot_System.Classes;
using Parking_Lot_System.Services;

namespace Parking_Lot_System
{
    public partial class MainPage : ContentPage
    {
        private CheckStorageForSessionAuth checker = new();

        private void Sample(object sender, EventArgs e) 
        {
            string username = EntryUsername.Text;

            Preferences.Set("username", username);
        }

        private async void CheckUsernamePreference(object sender, EventArgs e)
        {
            bool hasKey = Preferences.ContainsKey("username");

            if (hasKey == true)
            {
                await DisplayAlert("Alert", "1", "Okay");
            }
            else 
            {
                await DisplayAlert("Alert", "0", "Okay");
            }
        }

        public MainPage() // Inject singleton
        {
            InitializeComponent();

            AppShell _AppShell = (AppShell)Shell.Current;
        }

        // Show password
        private void ShowPassWord(object sender, CheckedChangedEventArgs e)
        {
            EntryPassword.IsPassword = !e.Value;
        }

        // Display sign up form
        public async void SignUpForm(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//SignUp");
        }

        // Sign in
        private async void SignIn(object sender, EventArgs e)
        {
            string username = EntryUsername.Text;
            string password = EntryPassword.Text;
            string url = $"https://vehicle-parking-guide-system-default-rtdb.asia-southeast1.firebasedatabase.app/users/{username}.json";

            BtnSignIn.IsEnabled = false;

            try
            {
                // Validate entries
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("Username or Password Is Empty!");
                }

                if (password.Length < 8)
                {
                    throw new Exception("Password must be at least 8 characters long!");
                }

                // HTTP client for request
                HttpClient httpClient = new HttpClient();

                // Perform GET request
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Parse JSON response
                string responseResult = await response.Content.ReadAsStringAsync();
                User? user = JsonConvert.DeserializeObject<User>(responseResult);

                // Validate user credentials
                if (string.IsNullOrWhiteSpace(user?.Username))
                {
                    throw new Exception("Invalid Username!");
                }

                if (password != user?.Password)
                {
                    throw new Exception("Wrong Password!");
                }

                // save preferences for login status 
                bool LoginStatus = checker.LogIn(username, password);

                if (LoginStatus == false)
                {
                    throw new Exception("Failed to log in please try again!");
                }
                // add a checking code if user from appshell userloginstatus
                await DisplayAlert("Success", "Successfully Logged In", "OK");
                BtnSignIn.IsEnabled = true;
                await Shell.Current.GoToAsync("//Home");
            }
            catch (Exception ex)
            {
                BtnSignIn.IsEnabled = true;
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

    }
}
