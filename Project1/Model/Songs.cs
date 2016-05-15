using SQLite;

namespace DisertationProject.Data.Models
{
    public class Songs
    {
        public class Person
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            public string SongName { get; set; }

            public string ArtistName { get; set; }

            public byte DataInformation { get; set; }

            public override string ToString()
            {
                return string.Format("[Person: Id={0}, SongName={1}, ArtistName={2}]", Id, SongName, ArtistName);
            }
        }
    }
}