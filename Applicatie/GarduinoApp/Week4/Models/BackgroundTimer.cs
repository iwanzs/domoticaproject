using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Week4;

namespace GarduinoApp.Models
{
    public class BackgroundTimer
    {
        DatabaseManager databasemanager = new DatabaseManager();

        public void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<Profiles> profiles = new List<Profiles>();
            try
            {
                profiles = databasemanager.GetProfiles();
            }
            catch
            {
            }

            var profilesAndConnections = profiles.Zip(Configuration.ConnectionsList, (p, c) => new { profile = p, Connection = c });
            foreach (var pc in profilesAndConnections)
            {
                pc.profile.CheckWeather(pc.Connection.ConnectionSocket);
            }

            //            foreach (var profile in profiles)
            //            {
            //                
            //            }
            //
            //            foreach (var connection in Configuration.ConnectionsList)
            //            {
            //                Profiles prof = new Profiles();
            //                prof.CheckWeather(connection.ConnectionSocket);
            //            }
        }
    }
}
