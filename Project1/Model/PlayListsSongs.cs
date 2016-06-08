namespace DisertationProject.Model
{
    public class PlayListsSongs
    {
        public int SongsId { get; set; }

        public int PlayListsId { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: SongsId={0}, PlayListsId={1}", SongsId, PlayListsId);
        }
    }
}