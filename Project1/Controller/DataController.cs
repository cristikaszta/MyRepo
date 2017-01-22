using DisertationProject.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DisertationProject.Controller
{
    public class DataController
    {

        /// <summary>
        /// SQL connection
        /// </summary>
        private SqlConnection _connection;

        /// <summary>
        /// Sql command
        /// </summary>
        private SqlCommand _command;

        /// <summary>
        /// Sql data _reader
        /// </summary>
        private SqlDataReader _reader;

        /// <summary>
        /// Song list
        /// </summary>
        private List<Song> _songList;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataController()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            _songList = new List<Song>();
            _command = new SqlCommand();
            EstablishConnection();
        }


        /// <summary>
        /// Establish connection
        /// </summary>
        private void EstablishConnection()
        {
            try
            {
                _connection = new SqlConnection(Globals.ConnectionString);
                _connection.Open();
                _command = new SqlCommand("SELECT Id, Name, Artist, Source, Type FROM dbo.Songs", _connection);
            }
            catch (Exception ex)
            {
                throw ex;
            };
        }

        /// <summary>
        /// Method to get all songs from the table
        /// </summary>
        public void GetSongs()
        {
            var song = new Song();
            try
            {
                //_command
                _reader = _command.ExecuteReader();
                if (_reader.HasRows)
                {
                    // Read advances to the next row.
                    while (_reader.Read())
                    {
                        // To avoid unexpected bugs access columns by name.
                        song.Id = _reader.GetInt32(_reader.GetOrdinal("Id"));
                        song.Name = _reader.GetString(_reader.GetOrdinal("Name"));
                        song.Artist = _reader.GetString(_reader.GetOrdinal("Artist"));
                        song.Source = _reader.GetString(_reader.GetOrdinal("Source"));
                        _songList.Add(song);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Close connection
        /// </summary>
        private void CloseConnection()
        {
            try
            {
                _connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            };
        }

    }
}
