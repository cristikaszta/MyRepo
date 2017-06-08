using Android.App;
using Android.Database;
using Android.Widget;
using DisertationProject.Model;
using System;
using System.Collections.Generic;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Text controller class
    /// </summary>
    //[Activity(Label = "TextController")]
    public class TextController : ListActivity
    {
        /// <summary>
        /// The main activity
        /// </summary>
        private MainActivity mainActivity;

        /// <summary>
        /// Message displayed below buttons
        /// </summary>
        public TextContainer TextContainer { get; set; }

        /// <summary>
        /// List items
        /// </summary>
        public List<string> Items { get; set; }

        /// <summary>
        /// The list view
        /// </summary>
        private ListView listView;

        /// <summary>
        /// List adapter
        /// </summary>
        private ArrayAdapter<string> listAdapter;

        /// <summary>
        /// Song list adapter
        /// </summary>
        private SongListAdapter songListAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainActivity"></param>
        public TextController(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
            TextContainer = new TextContainer(this.mainActivity, Globals.ErrorTextBox);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainActivity"></param>
        public TextController(MainActivity mainActivity, List<string> SongList)
        {
            this.mainActivity = mainActivity;
            TextContainer = new TextContainer(this.mainActivity, Globals.ErrorTextBox);
            Items = SongList;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize()
        {
            //Items = new List<string> { "one", "two", "three" };
            listView = (ListView)mainActivity.FindViewById(Resource.Id.songListView);
            songListAdapter = new SongListAdapter(mainActivity, Items);
            //listAdapter = new ArrayAdapter<string>(mainActivity, Android.Resource.Layout.SimpleListItemSingleChoice, Items);
            listView.Adapter = songListAdapter;
            //listAdapter.RegisterDataSetObserver(new MyDataSetObserver());
            //listAdapter.Add("a");
        }

        public void RefreshList()
        {
            //Items.Add("C");
            //listAdapter.Clear();
            //listAdapter.Add("B");
            //listAdapter.NotifyDataSetChanged();
            //_listView.RefreshDrawableState();
        }

        /// <summary>
        /// Method used to show error message
        /// </summary>d
        /// <param name="text"></param>
        public void SetError(string text)
        {
            throw new NotImplementedException();
        }
    }

    public class MyDataSetObserver : DataSetObserver
    {
        public override void OnChanged()
        {
            base.OnChanged();
        }
    }
}