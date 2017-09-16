using Android.App;
using Android.Views;
using Android.Widget;
using DisertationProject.Model;
using System.Collections.Generic;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Custom song list adapter class
    /// Inherits BaseAdapter class
    /// </summary>
    public class SongListAdapter : BaseAdapter<Song>
    {
        private List<Song> items;
        private Activity context;

        public SongListAdapter(Activity context, List<Song> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override Song this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            }
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = string.Format("{0} - {1}", item.Name, item.Artist);

            return convertView;
        }
    }
}