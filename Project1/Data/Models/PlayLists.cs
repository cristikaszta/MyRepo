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
using SQLite;

namespace Project1.Data.Models
{
    public class PlayLists
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string PlayListName { get; set; }

        public string Info { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: Id={0}, PlayListName={1}, Info={2}]", Id, PlayListName, Info);
        }
    }
}