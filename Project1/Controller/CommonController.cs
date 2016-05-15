using System;
using System.Collections.Generic;

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
    }
}