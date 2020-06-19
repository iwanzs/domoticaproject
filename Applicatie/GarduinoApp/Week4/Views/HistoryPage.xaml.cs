using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarduinoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage, INotifyPropertyChanged
    {
        //max amount of points in a graph
        private int MAXPOINTS = 10;

        /// <summary>
        /// 0 = airtemp
        /// 
        /// default is 0
        /// </summary>
        public int TYPEOFSENSOR = 0;

        public TimeSpan STARTTIMESPAN = TimeSpan.Zero;
        public TimeSpan PERIODTIMESPAN = TimeSpan.FromMinutes(5);

        public PlotModel Graph { get; set; }
        public LineSeries lineSeries { get; set; }

        public HistoryPage()
        {
            InitializeComponent();

            CreateGraph();
            //mock data

            //for testing
            var rnd = new Random();

            AddPoint(DateTime.Now, rnd.Next(0, 40));

            var timer = new System.Threading.Timer((e) =>
            {
                AddPoint(DateTime.Now, rnd.Next(0,40));
            }, null, STARTTIMESPAN, PERIODTIMESPAN);

            //var timer = new System.Threading.Timer((e) =>
            //{
            //    AddPoint(new DateTime(), passed value);
            //}, null, STARTTIMESPAN, PERIODTIMESPAN);
        }

        public void CreateGraph()
        {
            switch (TYPEOFSENSOR)
            {
                case 0:
                    Graph = new PlotModel { Title = "Temp of air" };
                    //Graph.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" });
                    Graph.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm dd-MM-yyyy", Title = "Time" });
                    Graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature(℃)" });

                    lineSeries = new LineSeries();
                    Graph.Series.Add(lineSeries);
                    LineGraph.Model = Graph;
                    break;

                default:
                    break;
            }
        }

        public void AddPoint(DateTime time, float temp)
        {
            switch (TYPEOFSENSOR)
            {
                case 0:
                    Graph.Series.Remove(lineSeries);
                    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), temp));
                    Graph.Series.Add(lineSeries);
                    break;


                default:
                    break;
            }

            //removes a point if the max is hit
            if (lineSeries.Points.Count >= MAXPOINTS)
            {
                RemovePoint();
            }
        }

        public void RemovePoint()
        {
            lineSeries.Points.RemoveAt(0);
        }

    }
}