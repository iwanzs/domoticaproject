using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace GarduinoApp.Models
{
    public class Connection
    {
        public int ConnectionID;
        public Socket ConnectionSocket;
        public Connection(int ID, Socket socket)
        {
            ConnectionID = ID;
            ConnectionSocket = socket;
        }
    }
}
