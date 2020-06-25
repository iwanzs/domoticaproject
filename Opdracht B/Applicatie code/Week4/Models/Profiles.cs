using SQLite;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Week4;

namespace GarduinoApp.Models
{
    class Profiles
    {
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int ID { get; set; }
        [NotNull]
        public int UserID { get; set; }
        [NotNull]
        public string Name { get; set; }
        [NotNull]
        public string Location { get; set; }
        [Ignore]
        public string FirstLetter => Name[0].ToString().ToUpper();
        [NotNull]
        public int Threshold { get; set; }
        [NotNull]
        public string IP { get; set; }
        [NotNull]
        public string Port { get; set; }
        [NotNull]
        public int ArduinoPinNumber { get; set; }
        [NotNull]
        public int AutoEnabled { get; set; }

        private Socket socket;

        public void SetSocket(Connection connection)
        {
            try
            {
                socket = connection.ConnectionSocket;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Hoi dit is de socket exception: " + e);
            }
            // tom volgende keer normale exceptions opschrijven... - iwan
            catch (NullReferenceException f)
            {
                Console.WriteLine("Hoi dit is de nullreferenceexception: " + f);
            }
        }

        // Connect to socket ip/prt (simple sockets)
        public Socket ConnectSocket(string ip, string port)
        {
            if (CheckValidIpAddress(ip) && CheckValidPort(port))
            {
                if (socket == null)
                {
                    try  // to connect to the server (Arduino).
                    {
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        Task t = Task.Run(() =>
                        {
                            socket.Connect(new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port)));
                        });
                        t.Wait(500);
                        return socket;
                    }
                    catch
                    {
                        if (socket != null)
                        {
                            socket.Close();
                            socket = null;
                        }
                        return socket;

                    }
                }
                else // disconnect socket
                {
                    socket.Close();
                    socket = null;
                    return socket;
                }
            }
            return socket;
        }

        public bool IsConnected()
        {
            if (socket != null)
                return socket.Connected;

            return false;
        }

        public bool ToggleProfile()
        {
            if (socket == null) return false;
            try
            {
                //Send command to server
                socket.Send(Encoding.ASCII.GetBytes("t"));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetResponse(string cmd)
        {
            byte[] buffer = new byte[4]; // response is always 4 bytes
            int bytesRead = 0;
            string result = "---";

            if (cmd != "a" && cmd != "s" && cmd != "w")
            {
                return result;
            }

            if (socket != null)
            {
                //Send command to server
                socket.Send(Encoding.ASCII.GetBytes(cmd));

                try //Get response from server
                {
                    //Store received bytes (always 4 bytes, ends with \n)
                    bytesRead = socket.Receive(buffer);  // If no data is available for reading, the Receive method will block until data is available,
                    //Read available bytes.              // socket.Available gets the amount of data that has been received from the network and is available to be read
                    while (socket.Available > 0) bytesRead = socket.Receive(buffer);
                    if (bytesRead == 4)
                        result = Encoding.ASCII.GetString(buffer, 0, bytesRead - 1); // skip \n
                    else result = "err";
                }
                catch (Exception exception)
                {
                    result = exception.ToString();
                    if (socket != null)
                    {
                        socket.Close();
                        socket = null;
                    }
                }
            }
            return result;
        }

        //Check if the entered IP address is valid.
        private bool CheckValidIpAddress(string ip)
        {
            if (ip != "")
            {
                //Check user input against regex (check if IP address is not empty).
                Regex regex = new Regex("\\b((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\\.|$)){4}\\b");
                Match match = regex.Match(ip);
                return match.Success;
            }
            else return false;
        }

        //Check if the entered port is valid.
        private bool CheckValidPort(string port)
        {
            //Check if a value is entered.
            if (port != "")
            {
                Regex regex = new Regex("[0-9]+");
                Match match = regex.Match(port);

                if (match.Success)
                {
                    int portAsInteger = Int32.Parse(port);
                    //Check if port is in range.
                    return ((portAsInteger >= 0) && (portAsInteger <= 65535));
                }
                else return false;
            }
            else return false;
        }

        public void CheckWeather(Socket sock)
        {
            Console.WriteLine("CheckWeather()");
            if (AutoEnabled != 1) return;

            string weather = "";
            string deviceState = "";
            byte[] buffer = new byte[4]; // response is always 4 bytes
            int bytesRead = 0;
            int intWeather = 0;

            Task ttry = Task.Run(() =>
            {
                try
                {
                    //create dbManager for inserting weather data
                    DatabaseManager databasemanager = new DatabaseManager();

                    sock.Send(ArduinoPinNumber == 999
                        ? Encoding.ASCII.GetBytes("w")
                        : Encoding.ASCII.GetBytes("a")
                    );
                    Console.WriteLine("sock.Receive");
                    bytesRead = sock.Receive(buffer);
                    while (sock.Available > 0) bytesRead = sock.Receive(buffer);
                    if (bytesRead == 4)
                        weather = Encoding.ASCII.GetString(buffer, 0, bytesRead - 1);

                    if (weather.Substring(0, 2) == "00")
                    {
                        weather = weather.Substring(2, weather.Length - 2);
                    }
                    else if (weather.Substring(0, 2) != "00" && weather.Substring(0, 1) == "0")
                    {
                        weather = weather.Substring(1, weather.Length - 1);
                    }
                    intWeather = Convert.ToInt32(weather);

                    //insert data into database
                    databasemanager.AddResult(UserID, intWeather, DateTime.Now.ToString());
                }
                catch (Exception exception)
                {
                    Console.WriteLine("De exception bij task 1 is: " + exception);
                }
            });
            ttry.Wait(100);

            Task tstry = Task.Run(() =>
            {
                try
                {
                    sock.Send(Encoding.ASCII.GetBytes("s"));
                    bytesRead = sock.Receive(buffer);
                    while (sock.Available > 0) bytesRead = sock.Receive(buffer);
                    if (bytesRead == 4)
                        deviceState = Encoding.ASCII.GetString(buffer, 0, bytesRead - 1);

                }
                catch (Exception exception)
                {
                    Console.WriteLine("De exception bij task 2 is " + exception);
                }
            });
            tstry.Wait(200);

            //Console.WriteLine("int.Parse");
            //If the weather is higher than the Threshold and the device is off ToggleProfile()

            if (intWeather > Threshold)
            {
                //Console.WriteLine("weather) >>>> Threshold");
                if (deviceState != " ON")
                {
                    try
                    {
                        //Send command to server
                        sock.Send(Encoding.ASCII.GetBytes("t"));
                    }
                    catch(SocketException r)
                    {
                        Console.WriteLine("Socket exception " + r);
                    }
                }
            }
            //else check if device in on and if so ToggleProfile()
            else if (intWeather < Threshold)
            {
                if (deviceState == " ON")
                {
                    try
                    {
                        //Send command to server
                        sock.Send(Encoding.ASCII.GetBytes("t"));
                    }
                    catch(SocketException s)
                    {
                        Console.WriteLine("Socket exception " + s);
                    }
                }
            }
        }
    }
}
