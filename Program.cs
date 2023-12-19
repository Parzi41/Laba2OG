using System;
using System.IO;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace Laba2
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            string buff = File.ReadAllText("DS2.txt");
            string[] point = buff.Split(' ', '\n', '\r');

            point = point.Where(o => o != "").ToArray();

            List<ScatterPoint> points = new List<ScatterPoint> { };

            for(int i = 0; i < point.Length; i++) 
            {
                if (i + 1 < point.Length)
                {
                    points.Add(new ScatterPoint(Convert.ToInt32(point[i]), Convert.ToInt32(point[i + 1])));
                }
            }

            var plotModel = new PlotModel { Title = "Scatter Plot" };
            var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };

            scatterSeries.Points.AddRange(points);

            plotModel.Series.Add(scatterSeries);

            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Y Axis" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X Axis" });

            var plotView = new PlotView
            {
                Model = plotModel,
                Width = 960,
                Height = 540
            };

            var fileName = "scatter_plot.png";
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                Thread thread = new Thread(() =>
                {
                    var pngExporter = new OxyPlot.Wpf.PngExporter { Width = 960, Height = 540 };
                    pngExporter.Export(plotModel, stream);
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }

            Console.WriteLine($"Plot saved to {fileName}");

            Console.ReadLine();
        }
    }
}