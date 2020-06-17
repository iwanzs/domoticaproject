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

        public PlotModel Graph { get; set; }
        public LineSeries lineSeries { get; set; }

        public HistoryPage()
        {
            InitializeComponent();

            CreateGraph();
            AddPoint(30, 20);
            AddPoint(40, 25);
        }

        public void CreateGraph()
        {
            Graph = new PlotModel { Title = "a graph"};
            Graph.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" });
            Graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature(℃)" });

            lineSeries = new LineSeries();
            lineSeries.Points.Add(new DataPoint(0, 20));
            lineSeries.Points.Add(new DataPoint(10, 22));
            lineSeries.Points.Add(new DataPoint(20, 15));
            Graph.Series.Add(lineSeries);
            LineGraph.Model = Graph;
        }

        public void AddPoint(int time, float temp)
        {
            Graph.Series.Remove(lineSeries);
            lineSeries.Points.Add(new DataPoint(time, temp));
            Graph.Series.Add(lineSeries);
        }

    }
}