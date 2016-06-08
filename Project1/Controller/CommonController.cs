using System;
using System.Collections.Generic;
using Emotion = DisertationProject.Model.Globals.Emotions;

namespace DisertationProject.Controller
{
    public class CommonController
    {
        /// <summary>
        /// Method used to add items to dictionaries
        /// </summary>
        /// <typeparam name="T1">Dictionary key type parameter</typeparam>
        /// <typeparam name="T2">Dictionary value type parameter</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public void addItemToDictionary<T1, T2>(IDictionary<T1, T2> dictionary, T1 key, Func<T1, T2> value)
        {
            dictionary.Add(key, (T2)value(key));
        }

        /// <summary>
        /// Method used to add items to dictionaries
        /// </summary>
        /// <typeparam name="T1">Dictionary key type parameter</typeparam>
        /// <typeparam name="T2">Dictionary value type parameter</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public void addItemToDictionary<T1, T2>(IDictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            dictionary.Add(key, value);
        }

        #region Converters

        public Emotion ConvertEmotion(string str)
        {
            Emotion result;
            switch (str)
            {
                case "H": result = Emotion.Happy; break;
                case "A": result = Emotion.Angry; break;
                case "S": result = Emotion.Sad; break;
                case "N": result = Emotion.Neutral; break;
                default: result = Emotion.Neutral; break;
            }
            return result;
        }

        public string Convert(Emotion str)
        {
            string result;
            switch (str)
            {
                case Emotion.Happy: result = "H"; break;
                case Emotion.Angry: result = "A"; break;
                case Emotion.Sad: result = "S"; break;
                case Emotion.Neutral: result = "N"; break;
                default: result = "H"; break;
            }
            return result;
        }

        #endregion
    }
}