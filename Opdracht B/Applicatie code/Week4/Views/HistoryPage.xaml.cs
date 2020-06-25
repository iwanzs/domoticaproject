﻿using GarduinoApp.Models;
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
        DatabaseManager dbManager;
        Profiles currentProfile;

        //max amount of points in a graph
        private int MAXPOINTS = 100;

        /// <summary>
        /// 0 = sensor status, dont use this one
        /// 1 = air temp
        /// 2 = ...
        /// </summary>
        public int typeOfSensor = 1;

        public TimeSpan STARTTIMESPAN = TimeSpan.Zero;
        public TimeSpan PERIODTIMESPAN = TimeSpan.FromMinutes(15);

        public PlotModel statusGraph { get; set; }
        public PlotModel sensorGraph { get; set; }
        public LineSeries statusSeries { get; set; }
        public LineSeries sensorSeries { get; set; }

        public HistoryPage()
        {
            InitializeComponent();

            dbManager = new DatabaseManager();
            currentProfile = dbManager.GetProfileInformation(Configuration.ProfileID);

            //fetches all data from a certain period
            GetData();

            //status graph
            statusGraph = CreateGraph();

            //sensor graph
            sensorGraph = CreateGraph(typeOfSensor);

            var timer = new System.Threading.Timer((e) =>
            {
                //AddPoint(DateTime.Now, );
                UpdateGraphs();
            }, null, STARTTIMESPAN, PERIODTIMESPAN);
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

            switch (type)
            {
                case 0:
                    graph = new PlotModel { Title = "Device status" };
                    graph.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH dd-MM", Title = "Time", });
                    graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "1 = on 0 = off" });

                    graph.Series.Add(statusSeries);

                    //connect the front to the back
                    StatusGraph.Model = graph;

                    return graph;

                case 1:
                    graph = new PlotModel { Title = "Temp of air" };
                    graph.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH dd-MM", Title = "Time" });
                    graph.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature(°F)" });

                    graph.Series.Add(sensorSeries);

                    //connect the front to the back
                    SensorGraph.Model = graph;

                    return graph;


                default:
                    //not a existing sensortype value
                    return null;
            }
        }

        public void UpdateGraphs()
        {
            //get the latest data
            Results item = dbManager.GetLatestResult(currentProfile.ID);

            int value;

            if (item.Value != null)
            {
                value = Int32.Parse(item.Value);

                int deviceStatus = GetDeviceStatus(value);

                UpdateLineSeries(DateTime.Now, value, deviceStatus);
            }
        }

        /// <summary>
        /// creates and adds another datapoint to the graph
        /// </summary>
        /// <param name="time">time/date of the data</param>
        /// <param name="temp">fill this in to pass a temp or other value</param>
        /// <param name="devicestatus">status of the device on = 1 off = 0</param>
        public void UpdateLineSeries(DateTime time, float temp, int deviceStatus)
        {
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

            //adding to the status graph
            statusGraph.Series.Remove(statusSeries);
            statusSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(time), deviceStatus));
            statusGraph.Series.Add(statusSeries);
            statusGraph.InvalidatePlot(true);

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
            statusSeries = new LineSeries();
            sensorSeries = new LineSeries();

            //query for data and put it in variable
            List<Results> collection = dbManager.GetPeriodOfResults(currentProfile.ID);

            //foreach through it all and putting it in datapoints and adding them to the lineseries
            foreach (var item in collection)
            {
                //put the correct data in lineSeries so it is available to everything
                int status = GetDeviceStatus(Int32.Parse(item.Value));
                float value = float.Parse(item.Value);
                DateTime date = DateTime.Parse(item.Date);

                statusSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), status));
                sensorSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), value));

                if (sensorSeries.Points.Count >= MAXPOINTS)
                {
                    RemovePoint();
                }

            }

        }
        public int GetDeviceStatus(int value)
        {
            //See if the value is above the threshhold
            if (value >= currentProfile.Threshold)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}