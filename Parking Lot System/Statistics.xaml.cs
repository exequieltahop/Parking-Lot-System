using Parking_Lot_System.Classes;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Maui.Layouts;
using System.ComponentModel;
using System.Collections.Generic;

namespace Parking_Lot_System;

public partial class Statistics : ContentPage
{
    public Statistics()
    {
        InitializeComponent();

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await getStatisticsData();
    }

    // get total parking
    private async Task getStatisticsData()
    {
        string url = "https://vehicle-parking-guide-system-default-rtdb.asia-southeast1.firebasedatabase.app/parking_logs.json";
        try
        {
            using (HttpClient client = new())
            {
                var response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var parseJsonDataIntoString = await response.Content.ReadAsStringAsync();

                var logs = JsonConvert.DeserializeObject<Dictionary<string, ParkingLogs>>(parseJsonDataIntoString);

                int count = 0;
                int count_park_today = 0;

                int loop_month_validator = 1;
                List<string> months = new List<string>();

                if (logs != null && logs.Count > 0)
                {
                    // loop the data for total parkings
                    foreach (var log in logs)
                    {
                        if (log.Value.Status == "in")
                        {
                            count++;
                        }

                        TimeZoneInfo manilaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");

                        // get date in the firebase and format it 
                        string raw_datetime = log.Value.Timestamp;
                        DateTime newDateTime = DateTime.Parse(raw_datetime);
                        string dateString = newDateTime.ToString("yyyy-MM-dd");


                        // Get the current date in the Manila time zone
                        DateTime currentManilaTime = TimeZoneInfo.ConvertTime(DateTime.Now, manilaTimeZone);
                        string currentDateString = currentManilaTime.ToString("yyyy-MM-dd");

                        // Convert the parsed DateTime to Manila time
                        DateTime manilaDateTime = TimeZoneInfo.ConvertTime(currentManilaTime, manilaTimeZone);
                        string currentDateTime = manilaDateTime.ToString("yyyy-MM-dd");

                        if (dateString == currentDateTime && log.Value.Status == "in")
                        {
                            count_park_today++;
                        }

                        // get month digit only
                        string date_string_month_part = newDateTime.ToString("yyyy-MM");

                        // Manage month data
                        if (!months.Contains(date_string_month_part)) 
                        {
                            months.Add(date_string_month_part);
                        }

                        loop_month_validator++;
                    }
                }
                // get count per month
                List<int> newMonthTotalPark = GetTotalParkPerMonth(months, logs);


                // add the value into the content
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    total_parking.Text = count.ToString();
                    total_parkings_today.Text = count_park_today.ToString();
                    AddTableData(months, newMonthTotalPark);
                });

            }
        }
        catch (Exception e)
        {
            await DisplayAlert("Error: ", e.Message, "Okay");
        }
    }

    // get total park per month
    private List<int> GetTotalParkPerMonth(List<string> months, Dictionary<String, ParkingLogs> logs)
    {
        List<int> total_parking_per_month = new List<int>();

        foreach (string month in months)
        {
            int count_total = 0;
            foreach (var log in logs)
            {
                string rawStringDate = log.Value.Timestamp;
                DateTime DateInstance = DateTime.Parse(rawStringDate);
                string monthDateOnly = DateInstance.ToString("yyyy-MM");

                if (monthDateOnly == month && log.Value.Status == "in")
                {
                    count_total++;
                }
            }

            total_parking_per_month.Add(count_total);
        }

        return total_parking_per_month;
    }

    // Add the month and counts into the table
    private void AddTableData(List<string> months, List<int> total_count_per_month)
    {
        int count = months.Count;
        int count_row_definition = 1;

        for (int i = 0; i < count; i++)
        {
            // new DateTime
            DateTime date_month = DateTime.Parse(months[i]);
            // format into yyyy, MMM or 2024, MAR
            string formatedDate = date_month.ToString("yyyy, MMM");

            // Create the frame for the month
            var frame_month = new Frame
            {
                CornerRadius = 0,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.FromArgb("#FFFFFF")
            };

            // Create the label for the month
            var label_month = new Label
            {
                Text = formatedDate,
                FontSize = 17,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromArgb("#000000")
            };

            // Add the label to the frame
            frame_month.Content = label_month;

            // Create the frame for the total count per month
            var frame_total_count = new Frame
            {
                CornerRadius = 0,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.FromArgb("#FFFFFF")
            };

            // Create the label for the total count
            var label_total_count = new Label
            {
                Text = total_count_per_month[i].ToString(), 
                FontSize = 17,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromArgb("#000000")
            };

            // Add the label to the frame
            frame_total_count.Content = label_total_count;

            // Set row and column in the parent grid
            table_monthly_park_list.SetRow(frame_month, count_row_definition);
            table_monthly_park_list.SetRow(frame_total_count, count_row_definition);
            table_monthly_park_list.SetColumn(frame_month, 0);
            table_monthly_park_list.SetColumn(frame_total_count, 1);

            // Add a new RowDefinition
            table_monthly_park_list.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            // Add the frames to the grid
            table_monthly_park_list.Children.Add(frame_month);
            table_monthly_park_list.Children.Add(frame_total_count);

            count_row_definition++;
        }
    }

}