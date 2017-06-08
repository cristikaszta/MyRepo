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

        /// <summary>
        /// SQL connection
        /// </summary>
        private SqlConnection connection;

        /// <summary>
        /// Sql command
        /// </summary>
        private SqlCommand command;

        /// <summary>
        /// Sql data _reader
        /// </summary>
        private SqlDataReader reader;

        /// <summary>
        /// Song list
        /// </summary>
        private List<Song> songList;

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
            songList = new List<Song>();
            command = new SqlCommand();
            EstablishConnection(Globals.ConnectionString);
        }

        /// <summary>
        /// Establish connection
        /// </summary>
        private void EstablishConnection(string connectionString)
        {
            //SqlConnection sqlconn;
            //string connsqlstring = string.Format("Server=tcp:ourserver.database.windows.net,1433;Data Source=ourserver.database.windows.net;Initial Catalog=ourdatabase;Persist Security Info=False;User ID=lanister;Password=tyrion0!;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;");

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                command = new SqlCommand("SELECT Id, Name, Artist, Source, Type FROM dbo.Songs", connection);
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
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        // To avoid unexpected bugs access columns by name.
                        song.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        song.Name = reader.GetString(reader.GetOrdinal("Name"));
                        song.Artist = reader.GetString(reader.GetOrdinal("Artist"));
                        song.Source = reader.GetString(reader.GetOrdinal("Source"));
                        songList.Add(song);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for connection close
        /// </summary>
        private void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            };
        }

    }
}
