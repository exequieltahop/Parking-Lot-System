using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Parking_Lot_System.Services
{
    public class CheckStorageForSessionAuth
    {
        public bool CheckAuth(string preference_name) 
        {
            bool login_status = Preferences.ContainsKey(preference_name);

            return login_status;
        }

        // log in user in the system storage
        public bool LogIn(string username, string password) 
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("Must not assign empty value");
            }

            Preferences.Set(nameof(username), username);
            Preferences.Set(nameof(password), password);

            return true;
        }

        public bool LogOut(string preference_name, string preference_password)
        {
            Preferences.Remove(preference_name);
            Preferences.Remove(preference_password);

            // Check if preferences were successfully removed
            bool isRemoved = !Preferences.ContainsKey(preference_name) && !Preferences.ContainsKey(preference_password);
            Debug.WriteLine($"Preferences removed: {isRemoved}");

            return isRemoved;
        }

    }
}
