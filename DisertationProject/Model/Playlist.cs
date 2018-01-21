
using System.Collections.Generic;
using System.Linq;

namespace DisertationProject.Model
{
    /// <summary>
    /// PlayList class
    /// </summary>
    public class PlayList
    {
        /// <summary>
        /// Playlist is
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Playlist name
        /// </summary>
        public string Name { get; set; }

        private int _totalItems;

        private int _position;

        /// <summary>
        /// The current position in the playlist
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// The song list
        /// </summary>
        public List<Song> SongList { get; set; }

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
                if (_position == _totalItems - 1)
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
                if (_position == 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            _position = 0;
            Shuffle = ToggleState.Off;
            Repeat = ToggleState.Off;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayList()
        {
            Initialize();
            SongList = new List<Song>();
            _totalItems = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <_parameter name="trackList">The tracklist</_parameter>
        public PlayList(List<Song> songList)
        {
            Initialize();
            SongList = songList;
            _totalItems = songList.Count;
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Add(Song item)
        {
            SongList.Add(item);
            _totalItems++;
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Add(List<Song> items)
        {
            SongList.AddRange(items);
            _totalItems += items.Count();
        }

        /// <summary>
        /// Remove items from playlist
        /// </summary>
        public void Remove(Song item)
        {
            if (SongList.Any())
            {
                SongList.Remove(item);
                _totalItems--;
            }
        }

        /// <summary>
        /// Get current song from the playlist
        /// </summary>
        /// <returns>Current song</returns>
        public Song GetCurrentSong()
        {
            return SongList[_position];
        }

        /// <summary>
        /// Increment current position
        /// </summary>
        public void IncrementPosition()
        {
            if (_position < _totalItems - 1)
                _position++;
        }

        /// <summary>
        /// Decrement current position
        /// </summary>
        public void DecrementPosition()
        {
            if (_position > 0)
                _position--;
        }

        /// <summary>
        /// Set position to zero (first position)
        /// </summary>
        public void ResetPosition()
        {
            _position = 0;
        }

        /// <summary>
        /// Set position to last
        /// </summary>
        public void SetPositionToEnd()
        {
            _position = _totalItems > 1 ? _totalItems - 1 : 0;
        }
    }
}