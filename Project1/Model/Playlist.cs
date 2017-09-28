
using System.Collections.Generic;
using System.Linq;

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
        public int Position { get; set; }

        /// <summary>
        /// The song list
        /// </summary>
        public List<Song> SongList;

        /// <summary>
        /// Suffle property
        /// </summary>
        public ToggleState Shuffle { get; set; }

        /// <summary>
        /// Repeat property
        /// </summary>
        public ToggleState Repeat { get; set; }

        /// <summary>
        /// Check if posiion is at end
        /// </summary>
        public bool IsAtEnd
        {
            get
            {
                if (Position == totalItems - 1)
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
                if (Position == 0)
                    return true;
                return false;
            }

        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            Position = 0;
            Shuffle = ToggleState.Off;
            Repeat = ToggleState.Off;
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
            return SongList[Position];
        }

        /// <summary>
        /// Increment current position
        /// </summary>
        public void IncrementPosition()
        {
            if (Position < totalItems - 1)
                Position++;
        }

        /// <summary>
        /// Decrement current position
        /// </summary>
        public void DecrementPosition()
        {
            if (Position > 0)
                Position--;
        }

        /// <summary>
        /// Set position to zero (first position)
        /// </summary>
        public void ResetPosition()
        {
            Position = 0;
        }

        /// <summary>
        /// Set position to last
        /// </summary>
        public void SetPositionToEnd()
        {
            Position = totalItems - 1;
        }
    }
}