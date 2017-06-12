
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
        /// Suffle property
        /// </summary>
        public Globals.State Shuffle { get; set; }

        /// <summary>
        /// Repeat property
        /// </summary>
        public Globals.State Repeat { get; set; }

        /// <summary>
        /// Check if posiion is at end
        /// </summary>
        public bool IsAtEnd
        {
            get
            {
                if (currentPosition == totalItems - 1)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Check if position is at beggining
        /// </summary>
        public bool IsAtBeggining
        {
            get
            {
                if (currentPosition == 0)
                    return true;
                return false;
            }

        }

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
    }
}