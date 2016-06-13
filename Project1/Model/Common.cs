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

namespace DisertationProject.Model
{
    /// <summary>
    /// Class for common methods
    /// </summary>
    public class Common
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

        /// <summary>
        /// Custom exception
        /// </summary>
        public class Problem : Exception
        {
            public Problem(string message) : base(message) { }
        }
    }
}