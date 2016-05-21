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
using DisertationProject.Data.Models;

namespace DisertationProject.Model
{
    /// <summary>
    /// Playlist class
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Repeat flag
        /// </summary>
        private bool _repeatFlag;

        /// <summary>
        /// The total items in the playlist
        /// </summary>
        private int _totalItems;

        /// <summary>
        /// The current position in the playlist
        /// </summary>
        private int _currentPosition;

        /// <summary>
        /// The tracklist
        /// </summary>
        private List<Song> _trackList;

        /// <summary>
        /// Suffle playlist flag
        /// </summary>
        private bool _suffleFlag;

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            _suffleFlag = false;
            _currentPosition = 0;
            _repeatFlag = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Playlist()
        {
            Init();
            _trackList = new List<Song>();
            _totalItems = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="trackList">The tracklist</param>
        public Playlist(List<Song> trackList)
        {
            Init();
            _trackList = trackList;
            _totalItems = trackList.Count;
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Add(Song item)
        {
            _trackList.Add(item);
            _totalItems++;
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Add(List<Song> items)
        {
            _trackList.AddRange(items);
            _totalItems += items.Count();
        }

        /// <summary>
        /// Add items to playlist
        /// </summary>
        public void Remove(Song item)
        {
            if (_trackList.Any())
            {
                _trackList.Remove(item);
                _totalItems--;
            }
        }

        /// <summary>
        /// Get current item from the playlist
        /// </summary>
        /// <returns></returns>
        public Song GetCurrentItem()
        {
            return _trackList[_currentPosition];
        }

        /// <summary>
        /// Increment current position
        /// </summary>
        public void IncrementPosition()
        {
            if (_currentPosition < _totalItems - 1)
                _currentPosition++;
        }

        /// <summary>
        /// Decrement current position
        /// </summary>
        public void DecrementPosition()
        {
            if (_currentPosition > 0)
                _currentPosition--;
        }

        /// <summary>
        /// Set suffle on
        /// </summary>
        public void SetSuffleOn()
        {
            _suffleFlag = true;
        }

        /// <summary>
        /// Set suffle off
        /// </summary>
        public void SetSuffleOff()
        {
            _suffleFlag = false;
        }

        /// <summary>
        /// Set repeat on
        /// </summary>
        public void SetRepeatOn()
        {
            _repeatFlag = true;
        }

        /// <summary>
        /// Set repeat off
        /// </summary>
        public void SetRepeatOff()
        {
            _repeatFlag = false;
        }

        /// <summary>
        /// Set position to zero (first position)
        /// </summary>
        public void ResetPosition()
        {
            _currentPosition = 0;
        }

        /// <summary>
        /// Set position to last
        /// </summary>
        public void SetPositionToEnd()
        {
            _currentPosition = _totalItems - 1;
        }

        /// <summary>
        /// Check if playlist is at the end
        /// </summary>
        /// <returns>True of playlist is at the end and false otherwise</returns>
        public bool IsAtEnd()
        {
            var result = false;
            if (_currentPosition == _totalItems - 1)
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
            if (_currentPosition == 0)
                result = true;
            return result;
        }

        /// <summary>
        /// Check if repeat flag is on
        /// </summary>
        /// <returns>Return true if repeat flag is on and false otherwise</returns>
        public bool IsRepeatOn()
        {
            var result = false;
            if (_repeatFlag)
                result = true;
            return result;
        }

    }
}