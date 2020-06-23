using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Text;
using GarduinoApp.Models;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Week4;

namespace GarduinoApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        static DatabaseManager databasemanager = new DatabaseManager();
        Profiles currentProfile = databasemanager.GetProfileInformation(Configuration.ProfileID);
        private Connection connection;

        public MainPage()
        {
            InitializeComponent();
            connection = Configuration.GetConnection(currentProfile.ID);
            currentProfile.SetSocket(connection);

            lblDevice.Text = currentProfile.Name;
            lblDevice.TextColor = Color.FromHex("2670B5");

            if (currentProfile.IsConnected())
            {
                btnConnectionState.BackgroundColor = Color.Green;
            }

            Task t = Task.Run(() => {

                if (currentProfile.GetResponse("s") == " ON")
                {
                    swhOnOff.Toggled -= SwhOnOff_OnToggled;
                    swhOnOff.IsToggled = true;
                    swhOnOff.Toggled += SwhOnOff_OnToggled;
                }

            });
            t.Wait(2000);

            if (currentProfile.AutoEnabled == 1)
            {
                swhAuto.IsToggled = true;
                swhOnOff.IsEnabled = false;
            }
            
            lblTempCurrent.Text = currentProfile.Threshold.ToString();
            sldrTemp.Value = currentProfile.Threshold;
        }

        private void SwhAuto_OnToggled(object sender, ToggledEventArgs e)
        {
            if (swhAuto.IsToggled)
            {
                swhOnOff.IsEnabled = false;
                databasemanager.SetAutoEnabled(1, currentProfile.ID);
            }
            else
            {
                swhOnOff.IsEnabled = true;
                databasemanager.SetAutoEnabled(0, currentProfile.ID);
            }
        }

        private void SwhOnOff_OnToggled(object sender, ToggledEventArgs e)
        {
            if (!currentProfile.ToggleProfile())
            {
                swhOnOff.Toggled -= SwhOnOff_OnToggled;
                swhOnOff.IsToggled = false;
                swhOnOff.Toggled += SwhOnOff_OnToggled;
            }
        }

        private void SldrTemp_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblTempCurrent.Text = Convert.ToInt32(sldrTemp.Value).ToString();
            databasemanager.SetThreshold((int)sldrTemp.Value, 1);
        }


//        // Connect to socket ip/prt (simple sockets)
//        public void ConnectSocket(string ip, string prt)
//        {
////            RunOnUiThread(() =>
////            {
//                // create new socket
//                if (socket == null)
//                {
//                    try  // to connect to the server (Arduino).
//                    {
//                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//                        socket.Connect(new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(prt)));
//                    }
//                    catch (Exception exception)
//                    {
//                        if (socket != null)
//                        {
//                            socket.Close();
//                            socket = null;
//                        }
//
//                    }
//                }
//                else // disconnect socket
//                {
//                    socket.Close();
//                    socket = null;
//                }
////            });
//        }
//
//        public string getResponse(string cmd)
//        {
//            byte[] buffer = new byte[4]; // response is always 4 bytes
//            int bytesRead = 0;
//            string result = "---";
//
//            if (socket != null)
//            {
//                //Send command to server
//                socket.Send(Encoding.ASCII.GetBytes(cmd));
//
//                try //Get response from server
//                {
//                    //Store received bytes (always 4 bytes, ends with \n)
//                    bytesRead = socket.Receive(buffer);  // If no data is available for reading, the Receive method will block until data is available,
//                    //Read available bytes.              // socket.Available gets the amount of data that has been received from the network and is available to be read
//                    while (socket.Available > 0) bytesRead = socket.Receive(buffer);
//                    if (bytesRead == 4)
//                        result = Encoding.ASCII.GetString(buffer, 0, bytesRead - 1); // skip \n
//                    else result = "err";
//                }
//                catch (Exception exception)
//                {
//                    result = exception.ToString();
//                    if (socket != null)
//                    {
//                        socket.Close();
//                        socket = null;
//                    }
//                }
//            }
//            return result;
//        }
//
//
//
//        //Check if the entered IP address is valid.
//        private bool CheckValidIpAddress(string ip)
//        {
//            if (ip != "")
//            {
//                //Check user input against regex (check if IP address is not empty).
//                Regex regex = new Regex("\\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\\.|$)){4}\\b");
//                Match match = regex.Match(ip);
//                return match.Success;
//            }
//            else return false;
//        }
//
//        //Check if the entered port is valid.
//        private bool CheckValidPort(string port)
//        {
//            //Check if a value is entered.
//            if (port != "")
//            {
//                Regex regex = new Regex("[0-9]+");
//                Match match = regex.Match(port);
//
//                if (match.Success)
//                {
//                    int portAsInteger = Int32.Parse(port);
//                    //Check if port is in range.
//                    return ((portAsInteger >= 0) && (portAsInteger <= 65535));
//                }
//                else return false;
//            }
//            else return false;
//        }
    }
}
