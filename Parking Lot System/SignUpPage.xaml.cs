using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Parking_Lot_System.Classes;

namespace Parking_Lot_System
{
	public partial class SignUpPage : ContentPage
	{
		public SignUpPage()
		{
			InitializeComponent();
		}

        //show password
        private void ShowPassWord(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                EntryPassword.IsPassword = false;
            }
            else
            {
                EntryPassword.IsPassword = true;
            }

        }

        //Sign Up
        private async void SignUp(object sender, EventArgs e)
        {
            // disable btn
            ButtonSignUp.IsEnabled = false;

            string username = EntryUsername.Text;
            string password = EntryPassword.Text;

            try
            {
                // validate entries
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("Username or Password Is Empty!");
                }

                if (password.Length < 8)
                {
                    throw new Exception("Password Is Atleast Must be 8 Characters Long!");
                }

                // register account in firebase
                bool register_status = await SignUp(username, password);

                if (register_status == true) 
                {
                    // enable btn again
                    ButtonSignUp.IsEnabled = true;

                    //Display Alert Success
                    await DisplayAlert("Success Alert", "Successfully Register Account", "okay");
                }
                else 
                {
                    throw new Exception("Failed to register please try again!");
                }

            }
            catch (Exception Ex)
            {
                //enable again
                ButtonSignUp.IsEnabled = true;
                
                //alert errors or exceptions
                await DisplayAlert("Error", Ex.Message, "Okay");
            }
        }

        // display sign in form 
        private async void DisplaySignIn(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//SignIn");
        }

        // set checkbox show password active
        private void CheckBoxActive(object sender, EventArgs e) 
        {
            if (ShowPassCheckBox.IsChecked == true)
            {
                ShowPassCheckBox.IsChecked = false;
            }
            else { 
                ShowPassCheckBox.IsChecked = true;
            }
        }

        // sign up
        private async Task<bool> SignUp(string username, string password) 
        {
            //url database
            string url = $"https://vehicle-parking-guide-system-default-rtdb.asia-southeast1.firebasedatabase.app/users/";

            // validations of username and password
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) 
            {
                throw new Exception("Username or Password Must Not Be Empty!");
            }

            if (password.Length < 8) 
            {
                throw new Exception("Password Must Be Atleast 8 Characters Long");
            }

            bool IsUsernameUniqueResult = await IsUsernameUnique(username, url);

            // register account into firebase realtime database
            HttpClient client = new();

            // prepare json data and headers for content
            var data_raw = new { username = username, password = password };
            var data_serialize = JsonConvert.SerializeObject(data_raw);
            var content = new StringContent(data_serialize, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Clear();

            var response = await client.PutAsync($"{url}{username}.json", content);

            response.EnsureSuccessStatusCode();

            return true;
        }
        // check if username already exist
        private async Task<bool> IsUsernameUnique(string username, string url) 
        {
            HttpClient _HttpClient = new();

            var response = await _HttpClient.GetAsync($"{url}{username}.json");

            response.EnsureSuccessStatusCode();

            string response_string = await response.Content.ReadAsStringAsync();

            // assign new user 
            User? user = JsonConvert.DeserializeObject<User>(response_string);

            // Validate user credentials
            if (!string.IsNullOrWhiteSpace(user?.Username))
            {
                throw new Exception("Username Already Exist!");
            }

            return true;
        }
    }
}