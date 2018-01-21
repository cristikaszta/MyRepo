using DisertationProject.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Data contoller class
    /// </summary>
    public class DataController
    {
        private static DataController _dataController;

        public static DataController Instance
        {
            get
            {
                if (_dataController == null)
                    _dataController = new DataController();
                return _dataController;
            }
        }

        /// <summary>
        /// SQL connection
        /// </summary>
        private SqlConnection _connection;

        /// <summary>
        /// Sql data _reader
        /// </summary>
        //private SqlDataReader _reader;

        /// <summary>
        /// Constructor
        /// </summary>
        private DataController()
        {
            EstablishConnection(Globals.ConnectionString);
        }

        /// <summary>
        /// Establish connection
        /// </summary>
        private void EstablishConnection(string connectionString)
        {
            try
            {
                _connection = new SqlConnection(connectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {

            };
        }

        /// <summary>
        /// Method to get all songs from the table
        /// </summary>
        public GenericResponse<List<Song>> GetSongs()
        {
            var songList = new List<Song>();
            var response = new GenericResponse<List<Song>>
            {
                Status = GenericStatus.Success,
                Message = "Get songs success"
            };
            try
            {
                var _command = new SqlCommand("SELECT Id, Name, Artist, Source, Playlist FROM dbo.Songs", _connection);
                var _reader = _command.ExecuteReader();
                if (_reader.HasRows)
                {
                    // Read advances to the next row.
                    while (_reader.Read())
                    {
                        // To avoid unexpected bugs access columns by name.
                        songList.Add(new Song()
                        {
                            Id = _reader.GetInt32(_reader.GetOrdinal("Id")),
                            Name = _reader.GetString(_reader.GetOrdinal("Name")),
                            Artist = _reader.GetString(_reader.GetOrdinal("Artist")),
                            Source = Globals.FileStorage + _reader.GetString(_reader.GetOrdinal("Source"))
                        });
                    }
                }
                response.Result = songList;
            }
            catch (Exception ex)
            {
                response.Status = GenericStatus.Failed;
                response.Message = $"Get songs failed : {ex.Message}";
            };
            return response;
        }

        /// <summary>
        /// Method for connection close
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
