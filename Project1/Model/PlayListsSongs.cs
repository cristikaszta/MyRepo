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

namespace Project1.Data.Models
{
    public class PlayListsSongs
    {
        public int SongsId { get; set; }

        public int PlayListsId { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: SongsId={0}, PlayListsId={1}", SongsId, PlayListsId);
        }
    }
}