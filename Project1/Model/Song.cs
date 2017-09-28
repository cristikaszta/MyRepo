namespace DisertationProject.Model
{
    /// <summary>
    /// Song class
    /// </summary>
    public class Song
    {
        /// <summary>
        /// Id of the song
        /// </summary>
        public long Id { get; set; }

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
        /// Emotion
        /// </summary>
        public Emotion Emotion { get; set; }

    }
}