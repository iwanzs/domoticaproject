using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week4;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarduinoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage, INotifyPropertyChanged
    {
        DatabaseManager dbmanager;

        //max amount of points in a graph
        private int MAXPOINTS = 20;

        /// <summary>
        /// 0 = sensor status, dont use this one
        /// 1 = air temp
        /// 2 = ...
        /// </summary>
        public int typeOfSensor = 1;

        public TimeSpan STARTTIMESPAN = TimeSpan.Zero;
        public TimeSpan PERIODTIMESPAN = TimeSpan.FromMinutes(1);

        //public PlotModel graph;

        public PlotModel statusGraph { get; set; }
        public PlotModel sensorGraph { get; set; }
        public LineSeries statusSeries { get; set; }
        public LineSeries sensorSeries { get; set; }

        public HistoryPage()
        {
            InitializeComponent();

            dbmanager = new DatabaseManager();

            //hardcoded way of saying what sensor it is
            //typeOfSensor = 1;

            //status graph
            statusGraph = CreateGraph();

            //sensor graph
            sensorGraph = CreateGraph(typeOfSensor);

            //for testing
            var rnd = new Random();

            //to add a new point to both graphs
            //AddPoint(DateTime.Now, rnd.Next(0, 40), true);

            //time loop with test data
            var timer = new System.Threading.Timer((e) =>
            {
                //to add a new point to both graphs
                AddPoint(DateTime.Now, rnd.Next(0,40), rnd.Next(0,2));
            }, null, STARTTIMESPAN, PERIODTIMESPAN);

            //var timer = new System.Threading.Timer((e) =>
            //{
            //    AddPoint(new DateTime(), passed value);
            //}, null, STARTTIMESPAN, PERIODTIMESPAN);
        }

        /// <summary>
        /// Creates a graph
        /// the default graph is the status graph (0)
        /// </summary>
        /// <param name="type">default 0, shows what type of graph needs to be made</param>
        /// <returns></returns>
        public PlotModel CreateGraph(int type = 0)
        {
            PlotModel graph;
            sensorSeries = new LineSeries();

            switch (type)
            {
                case 0:
                    statusSeries = new LineSeries();
                    
                    graph = new PlotModel { Title = "Device status" };
                    //Graph.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" });
                    graph.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm dd-MM", Title = "Time",  });
                    graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "1 = on 0 = off" });


                    //statusSeries = GetData();

                    graph.Series.Add(sensorSeries);

                    //connect the front to the back
                    StatusGraph.Model = graph;

                    return graph;

                case 1:
                    graph = new PlotModel { Title = "Temp of air" };
                    //Graph.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Time" });
                    graph.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH dd-MM-yyyy", Title = "Time" });
                    graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature(℃)" });

                    //sensorSeries = GetData();

                    graph.Series.Add(sensorSeries);

                    //connect the front to the back
                    SensorGraph.Model = graph;

                    return graph;


                default:
                    //not a existing sensortype value
                    return null;
            }
        }

        /// <summary>
        /// creates and adds another datapoint to the graph
        /// </summary>
        /// <param name="time">time/date of the data</param>
        /// <param name="temp">fill this in to pass a temp or other value</param>
        /// <param name="devicestatus">status of the device on = 1 off = 0</param>
        public void AddPoint(DateTime time, float temp, int devicestatus)
        {
            //adding to the status graph
            statusGraph.Series.Remove(statusSeries);
            statusSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), devicestatus));
            statusGraph.Series.Add(statusSeries);
            statusGraph.InvalidatePlot(true);

            //switch for all the kinds of sensors
            switch (typeOfSensor)
            {
                case 1:
                    sensorGraph.Series.Remove(sensorSeries);
                    sensorSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), temp));
                    sensorGraph.Series.Add(sensorSeries);
                    sensorGraph.InvalidatePlot(true);
                    break;

                default:
                    break;
            }

            //removes a point if the max is hit if this is true in the other graph a point has to be removed as well
            if (sensorSeries.Points.Count >= MAXPOINTS)
            {
                RemovePoint();
            }
        }

        /// <summary>
        /// Removes a datapoint from both graphs
        /// </summary>
        public void RemovePoint()
        {
            sensorSeries.Points.RemoveAt(0);
            statusSeries.Points.RemoveAt(0);
        }

        /// <summary>
        /// queries database for data that corresponds with what is needed then processes that and puts it in a LineSeries
        /// </summary>
        public void GetData()
        {
            //create a variable to go a month back
            var today = DateTime.Today;
            var month = new DateTime(today.Year, today.Month, 1);
            var first = month.AddMonths(-1);

            //query for data and put it in variable
            

            ////foreach through it all and putting it in datapoints and adding them to the lineseries
            //foreach (var item in collection)
            //{

            //    //put the correct data in lineSeries so it is available to everything

            //}

        }

    }
}