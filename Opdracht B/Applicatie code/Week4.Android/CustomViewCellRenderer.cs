using Android.Content;
using Android.Views;
using Week4.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Android.Graphics.Drawables;
using Week4;
using Week4.Views;
using Week4.MenuItems;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace Week4.Droid
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        private Android.Views.View _cellCore;
        private Drawable _unselectedBackground;
        private bool _selected;
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            _cellCore = base.GetCellCore(item, convertView, parent, context);
            _selected = false;
            _unselectedBackground = _cellCore.Background;
            return _cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnCellPropertyChanged(sender, e);
            if (e.PropertyName == "IsSelected")
            {
                _selected = !_selected;
                if (_selected)
                {
                    var extendedViewCell = sender as CustomViewCell;
                    _cellCore.SetBackgroundColor(extendedViewCell.SelectedItemBackgroundColor.ToAndroid());
                }
                else
                {
                    _cellCore.SetBackground(_unselectedBackground);
                }
            }
        }
    }
}