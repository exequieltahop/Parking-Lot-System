using Parking_Lot_System.Services;
using System.Net.Http;
using Newtonsoft.Json;
using Parking_Lot_System.Classes;

namespace Parking_Lot_System;

public partial class HomePage : ContentPage
{
    private CheckStorageForSessionAuth checker = new();
    private Timer? timer;

    public HomePage()
    {
        InitializeComponent();
        // Start the timer to get slot status
        GetSlotStatusByTimer("slot_1");
        GetSlotStatusByTimer("slot_2");
        GetSlotStatusByTimer("slot_3");
        GetSlotStatusByTimer("slot_4");
    }

    private async void SignOut(object sender, EventArgs e)
    {
        bool logout_status = checker.LogOut("username", "password");

        if (logout_status == true)
        {
            //await Shell.Current.GoToAsync("//SignIn");
            await DisplayAlert("alert", "ok", "okay");
        }
        else 
        {
            await DisplayAlert("alert", "!ok", "okay");
        }
    }

    // Get display slot status async func
    private void GetSlotStatusByTimer(string slotName)
    {
        // Create a timer that triggers every second
        timer = new Timer(async _ => await DisplaySlotStatus(slotName), null, 0, 1000);
    }

    // Display parking slot status
    private async Task DisplaySlotStatus(string slot_name)
    {
        string url = "https://vehicle-parking-guide-system-default-rtdb.asia-southeast1.firebasedatabase.app/";

        try
        {
            using (HttpClient http_client = new())
            {
                var response = await http_client.GetAsync($"{url}/slots/{slot_name}.json");

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                string response_string = await response.Content.ReadAsStringAsync();
                Slot? slot = JsonConvert.DeserializeObject<Slot>(response_string);

                // Check if slot is not null and Status has a value
                if (slot?.Status.HasValue == true)
                {
                    // Ensure that UI updates happen on the main thread
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        switch (slot_name)
                        {
                            case "slot_1":
                                if (slot.Status == 1)
                                {
                                    img_car_1.IsVisible = false; // Slot available
                                    img_p_1.IsVisible = true;
                                }
                                else
                                {
                                    img_car_1.IsVisible = true; // Slot available
                                    img_p_1.IsVisible = false;
                                }
                                break;
                            case "slot_2":
                                if (slot.Status == 1)
                                {
                                    img_car_2.IsVisible = false; // Slot available
                                    img_p_2.IsVisible = true;
                                }
                                else
                                {
                                    img_car_2.IsVisible = true; // Slot available
                                    img_p_2.IsVisible = false;
                                }
                                break;
                            case "slot_3":
                                if (slot.Status == 1)
                                {
                                    img_car_3.IsVisible = false; // Slot available
                                    img_p_3.IsVisible = true;
                                }
                                else
                                {
                                    img_car_3.IsVisible = true; // Slot available
                                    img_p_3.IsVisible = false;
                                }
                                break;
                            case "slot_4":
                                if (slot.Status == 1)
                                {
                                    img_car_4.IsVisible = false; // Slot available
                                    img_p_4.IsVisible = true;
                                }
                                else
                                {
                                    img_car_4.IsVisible = true; // Slot available
                                    img_p_4.IsVisible = false;
                                }
                                break;
                            default:
                                break;
                        }

                    });
                }
                else
                {
                    // Handle case where slot is null or Status is not set
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        DisplayAlert("Error", "Slot information is not available.", "Okay");
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                DisplayAlert("Error", ex.Message, "Okay");
            });
        }
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        timer?.Dispose(); // Stop the timer when the page is no longer visible
    }

    // sample button
    private async void ClickMe(object sender, EventArgs e) 
    {
        bool stat = Preferences.ContainsKey("username");

        if (stat == true) 
        {
            await DisplayAlert("Alert", "1", "OK");
        }
        else 
        {
            await DisplayAlert("Alert", "0", "OK");
        }
    }
}
