using System;
using System.Globalization;
using Xamarin.Forms;

namespace GarduinoApp.Models.Weather
{
    public class LongToDateTimeConverter : IValueConverter

    {
        // Initialisation of the Variable.
        DateTime _time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        /// <summary>
        /// Converts the incoming Long data into a properly formatted time.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long dateTime = (long)value;
            return $"{_time.AddSeconds(dateTime).ToString()} UTC";
        }

        /// <summary>
        /// Not used, placeholder for the future (may we ever need to return something or use the old date/time to call the API again)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
