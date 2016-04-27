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
    public class Songs
    {
        public class Person
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            public string SongName { get; set; }

            public string ArtistName { get; set; }

            public byte Data { get; set; }

            public override string ToString()
            {
                return string.Format("[Person: Id={0}, SongName={1}, ArtistName={2}]", Id, SongName, ArtistName);
            }
        }
    }
}