using System;
using System.Collections.Generic;
using Emotion = DisertationProject.Model.Globals.Emotions;

namespace DisertationProject.Controller
{
    /// <summary>
    /// Helper class
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Method used to add items to dictionaries
        /// </summary>
        /// <typeparam name="T1">Dictionary key type parameter</typeparam>
        /// <typeparam name="T2">Dictionary value type parameter</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void addItemToDictionary<T1, T2>(IDictionary<T1, T2> dictionary, T1 key, Func<T1, T2> value)
        {
            dictionary.Add(key, value(key));
        }

        /// <summary>
        /// Method used to add items to dictionaries
        /// </summary>
        /// <typeparam name="T1">Dictionary key type parameter</typeparam>
        /// <typeparam name="T2">Dictionary value type parameter</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void addItemToDictionary<T1, T2>(IDictionary<T1, T2> dictionary, List<T1> key, Func<T1, T2> value)
        {
            foreach (var item in key) dictionary.Add(item, (T2)value(item));
        }

        /// <summary>
        /// Method used to add items to dictionaries
        /// </summary>
        /// <typeparam name="T1">Dictionary key type parameter</typeparam>
        /// <typeparam name="T2">Dictionary value type parameter</typeparam>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void addItemToDictionary<T1, T2>(IDictionary<T1, T2> dictionary, T1 key, T2 value)
        {
            dictionary.Add(key, value);
        }

        #region Converters

        /// <summary>
        /// Convert string to emotion enum
        /// </summary>
        /// <param name="str">String text</param>
        /// <returns>Emotion enum</returns>
        public static Emotion Convert(string str)
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

        /// <summary>
        /// Convert emotion enum to string text
        /// </summary>
        /// <param name="emotion">Emotion enum value</param>
        /// <returns>String text</returns>
        public static string Convert(Emotion emotion)
        {
            string result;
            switch (emotion)
            {
                case Emotion.Happy: result = "H"; break;
                case Emotion.Angry: result = "A"; break;
                case Emotion.Sad: result = "S"; break;
                case Emotion.Neutral: result = "N"; break;
                default: result = "N"; break;
            }
            return result;
        }

        #endregion
    }
}
