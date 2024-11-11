using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls;
using System.Net.Http;
using Parking_Lot_System.Classes;
using Newtonsoft.Json;

namespace Parking_Lot_System;
public partial class History : ContentPage
{
    public History()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await PopulateFlexLayouts();
    }

    // add dynamically the elements base from the data in the realtime database
    private async Task PopulateFlexLayouts()
    {
        try
        {
            string database_url = "https://vehicle-parking-guide-system-default-rtdb.asia-southeast1.firebasedatabase.app/parking_logs.json";

            using HttpClient client = new HttpClient();

            var response = await client.GetAsync(database_url);
            response.EnsureSuccessStatusCode();

            string response_result = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a Dictionary
            var logs = JsonConvert.DeserializeObject<Dictionary<string, ParkingLogs>>(response_result);

            // Check if the dictionary is not null and has entries
            if (logs != null && logs.Count > 0)
            {
                // Iterate through each log entry
                foreach (var logEntry in logs)
                {
                    var parkingLog = logEntry.Value; // Get the ParkingLogs object

                    Frame frame = new Frame
                    {
                        BackgroundColor = Colors.MediumPurple,
                        Padding = new Thickness(10), // Optional: Add padding if needed
                        Margin = new Thickness(10)
                    };

                    // Create the main FlexLayout
                    FlexLayout flexLayout = new FlexLayout
                    {
                        Direction = FlexDirection.Column,
                        JustifyContent = FlexJustify.Center
                    };

                    // Create the first HorizontalStackLayout for the parking image and slot label
                    HorizontalStackLayout horizontalStackLayout = new HorizontalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Start // Align to start
                    };

                    // Create the Image for parking
                    Image parkingImage = new Image
                    {
                        Source = "parking.png",
                        Aspect = Aspect.AspectFill,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        WidthRequest = 30
                    };

                    // Create the Label for Slot
                    Label slotLabel = new Label
                    {
                        Text = parkingLog.Slot_no,
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White
                    };

                    // Add the Image and Slot Label to the HorizontalStackLayout
                    horizontalStackLayout.Children.Add(parkingImage);
                    horizontalStackLayout.Children.Add(slotLabel);

                    // Create the BoxView
                    BoxView boxView = new BoxView
                    {
                        HeightRequest = 1,
                        Color = Colors.White,
                        HorizontalOptions = LayoutOptions.Fill,
                        Margin = new Thickness(0, 5, 0, 5)
                    };

                    // Create the second HorizontalStackLayout for the timestamp
                    HorizontalStackLayout timestampLayout = new HorizontalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.End // Align to end
                    };

                    // Create the Image for the timestamp
                    Image timestampImage = new Image
                    {
                        Source = "timestamp.png",
                        WidthRequest = 20,
                        HeightRequest = 20,
                        Margin = new Thickness(0, 0, 5, 0),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    // Create the Label for the status and timestamp
                    Label timestampLabel = new Label
                    {
                        Text = "out | 2024-11-10 14:28:52",
                        TextColor = Colors.White,
                        FontAttributes = FontAttributes.Bold,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };

                    // Add the timestamp image and label to the timestamp layout
                    timestampLayout.Children.Add(timestampImage);
                    timestampLayout.Children.Add(timestampLabel);

                    // Add all child elements to the main FlexLayout
                    flexLayout.Children.Add(horizontalStackLayout);
                    flexLayout.Children.Add(boxView);
                    flexLayout.Children.Add(timestampLayout);

                    // Set the FlexLayout as the content of the Frame
                    frame.Content = flexLayout;

                    // Assuming 'container' is a layout (like StackLayout or Grid) where you want to add this frame
                    container.Children.Add(frame);
                }
            }
            else
            {
                await DisplayAlert("Alert", "No parking logs found.", "Okay");
            }
        }
        catch (Exception e)
        {
            await DisplayAlert("Error", e.Message, "Okay");
        }
    }

}
