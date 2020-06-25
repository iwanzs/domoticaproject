using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GarduinoApp;
using Week4.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyEntry), typeof(CustomEntryRendererAndroid))]
namespace Week4.Droid
{
    public class CustomEntryRendererAndroid : EntryRenderer
    {
        public CustomEntryRendererAndroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if(Control != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}