using System;
using Microsoft.Maui.Graphics;

namespace Parking_Lot_System.Classes
{
    public class CircularProgressBarDrawable : IDrawable
    {
        public float Progress { get; set; } = 0; // Value between 0 and 100
        public float Thickness { get; set; } = 20; // Thickness of the arc
        public Color BackgroundColor { get; set; } = Colors.Red; // Background color of the circle
        public Color ProgressColor { get; set; } = Colors.Blue; // Color of the progress arc
        public Color TextColor { get; set; } = Colors.Black; // Color of the progress text

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Ensure progress is within bounds
            Progress = Math.Clamp(Progress, 0, 100);

            // Calculate the diameter and radius based on the drawable area
            float diameter = Math.Min(dirtyRect.Width, dirtyRect.Height) - Thickness;
            float radius = diameter / 2;

            // Center coordinates
            float centerX = dirtyRect.Center.X;
            float centerY = dirtyRect.Center.Y;

            // Draw the background circle
            canvas.StrokeColor = BackgroundColor;
            canvas.StrokeSize = Thickness;
            canvas.DrawEllipse(centerX - radius, centerY - radius, diameter, diameter);

            // Calculate the sweep angle based on progress
            float sweepAngle = 360 * (Progress / 100);

            // Draw the progress arc only if Progress > 0
            if (Progress > 0)
            {
                canvas.StrokeColor = ProgressColor;
                canvas.StrokeSize = Thickness;
                canvas.DrawArc(centerX - radius, centerY - radius, diameter, diameter, 270, sweepAngle, true, false);
            }

            // Draw progress text in the center
            canvas.FontSize = diameter / 5; // Scale font size to fit within the circle
            canvas.FontColor = TextColor;
            canvas.DrawString($"{Progress}%", centerX, centerY, diameter, diameter / 2, HorizontalAlignment.Center, VerticalAlignment.Center, TextFlow.ClipBounds, 0f);
        }
    }
}
