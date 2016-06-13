namespace DisertationProject.Model
{
    public class Song
    {
        /// <summary>
        /// Id of the song
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the song
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Source of the song
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Artist name
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Song type: H, S or N
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Returns the full song name and source
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} : {1} - {2}", Type, Artist, Name);
        }

    }
}