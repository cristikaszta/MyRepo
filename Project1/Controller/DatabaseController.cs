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
using System.Data.SqlClient;
using DisertationProject.Model;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Controller class used to manipulate data from the database and use it in the application
    /// </summary>
    public class DatabaseController
    {
        /// <summary>
        /// The SQL connection object
        /// </summary>
        private SqlConnection _connection;

        /// <summary>
        /// The data reader object
        /// </summary>
        private SqlDataReader _reader;

        /// <summary>
        /// The SQL command object
        /// </summary>
        private SqlCommand _command;

        /// <summary>
        /// The SQL parameter object
        /// </summary>
        private SqlParameter _parameter;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseController()
        {
            Initialize();
            _parameter = new SqlParameter();
            _command = _connection.CreateCommand();

        }

        /// <summary>
        /// Initialization method
        /// Must be called at the beggining
        /// </summary>
        public void Initialize()
        {
            //string connsqlstring = string.Format("Server=tcp:ourserver.database.windows.net,1433;Data Source=ourserver.database.windows.net;Initial Catalog=ourdatabase;Persist Security Info=False;User ID=lanister;Password=tyrion0!;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;");
            try
            {
                _connection = new SqlConnection(Globals.TheConnectionString);
                _connection.Open();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public void GetSong(int playListId)
        public List<Song> GetSongs()
        {
            var songList = new List<Song>();
            //_command.CommandText = "SELECT * FROM Songs WHERE Id = @playListId";
            _command.CommandText = "SELECT * FROM Songs";

            //_parameter.ParameterName = "@playListId";
            //_parameter.Value = playListId;

            //_command.Parameters.Add(_parameter);
            _reader = _command.ExecuteReader();

            if (_reader.HasRows)
            {
                while (_reader.Read())
                {
                    var song = new Song();
                    song.Id = _reader.GetInt32(_reader.GetOrdinal("Id"));
                    song.Name = _reader.GetString(_reader.GetOrdinal("Name"));
                    song.Source = _reader.GetString(_reader.GetOrdinal("Source"));
                    song.Artist = _reader.GetString(_reader.GetOrdinal("Artist"));
                    song.Type = _reader.GetString(_reader.GetOrdinal("Type"));
                    songList.Add(song);
                }
            }
            _reader.Close();
            CloseConnection();
            return songList;
        }

        /// <summary>
        /// Close the open connection
        /// </summary>
        public void CloseConnection()
        {
            if (_connection != null)
                _connection.Close();
        }
    }
}