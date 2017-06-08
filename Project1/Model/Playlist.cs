
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DisertationProject.Model;

namespace DisertationProject.Model
{
    /// <summary>
    /// Playlist class
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Playlist is
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Playlist name
        /// </summary>
        public string PlayListName { get; set; }

        /// <summary>
        /// Repeat flag
        /// </summary>
        private bool repeatFlag;

        /// <summary>
        /// The total items in the playlist
        /// </summary>
        private int totalItems;

        /// <summary>
        /// The current position in the playlist
        /// </summary>
        private int currentPosition;

        /// <summary>
        /// The song list
        /// </summary>
        public List<Song> SongList;

        /// <summary>
        /// Suffle playlist flag
        /// </summary>
        private bool suffleFlag;

        /// <summary>
        /// Suffle property
        /// </summary>
        public Globals.State Shuffle { get; set; }

        /// <summary>
        /// Repeat property
        /// </summary>
        public Globals.State Repeat { get; set; }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            currentPosition = 0;
            Shuffle = Globals.State.Off;
            Repeat = Globals.State.Off;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Playlist()
        {
            Initialize();
            SongList = new List<Song>();
            totalItems = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <_parameter name="trackList">The tracklist</_parameter>
        public Playlist(List<Song> trackList)
        {
            Initialize();
            SongList = trackList;
            totalItems = trackList.Count;
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Add(Song item)
        {
            SongList.Add(item);
            totalItems++;
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Add(List<Song> items)
        {
            SongList.AddRange(items);
            totalItems += items.Count();
        }

        /// <summary>
        /// Remove items from playlist
        /// </summary>
        public void Remove(Song item)
        {
            if (SongList.Any())
            {
                SongList.Remove(item);
                totalItems--;
            }
        }

        /// <summary>
        /// Get current song from the playlist
        /// </summary>
        /// <returns>Current song</returns>
        public Song GetCurrentSong()
        {
            return SongList[currentPosition];
        }

        /// <summary>
        /// Increment current position
        /// </summary>
        public void IncrementPosition()
        {
            if (currentPosition < totalItems - 1)
                currentPosition++;
        }

        /// <summary>
        /// Decrement current position
        /// </summary>
        public void DecrementPosition()
        {
            if (currentPosition > 0)
                currentPosition--;
        }

        /// <summary>
        /// Set position to zero (first position)
        /// </summary>
        public void ResetPosition()
        {
            currentPosition = 0;
        }

        /// <summary>
        /// Set position to last
        /// </summary>
        public void SetPositionToEnd()
        {
            currentPosition = totalItems - 1;
        }

        /// <summary>
        /// Check if playlist is at the end
        /// </summary>
        /// <returns>True of playlist is at the end and false otherwise</returns>
        public bool IsAtEnd()
        {
            var result = false;
            if (currentPosition == totalItems - 1)
                result = true;
            return result;
        }

        /// <summary>
        /// Check if playlist is at the beggining
        /// </summary>
        /// <returns>True of playlist is at the beggining and false otherwise</returns>
        public bool IsAtBeggining()
        {
            var result = false;
            if (currentPosition == 0)
                result = true;
            return result;
        }

        /// <summary>
        /// Check if repeat flag is on
        /// </summary>
        /// <returns>Return true if repeat flag is on and false otherwise</returns>
        public bool IsRepeatEnabled()
        {
            var result = false;
            if (repeatFlag)
                result = true;
            return result;
        }

        /// <summary>
        /// Check if suffle flag is on
        /// </summary>
        /// <returns>Return true if suffle flag is on and false otherwise</returns>
        public bool IsSuffleEnabled()
        {
            var result = false;
            if (suffleFlag)
                result = true;
            return result;
        }
    }
}