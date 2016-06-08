
namespace DisertationProject.Data.Models
{
    public class Song
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        public string Artist { get; set; }

        //added by Babu :)) =))) buna asta Cristi
        public string Type { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: Id={0}, Name={1}, Source={2},Artist={3},Type={4}]", Id, Name, Source, Artist, Type);
        }

    }
}