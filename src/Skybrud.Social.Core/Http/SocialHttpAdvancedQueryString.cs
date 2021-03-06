using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web;
using Skybrud.Social.Interfaces.Http;

namespace Skybrud.Social.Http {

    /// <summary>
    /// Class representing an advanced query string. While a query string typically won't have items sharing the same
    /// key, there might be need for it in some cases. This class will allow multiple items with the same key. 
    /// </summary>
    public class SocialHttpAdvancedQueryString : IHttpQueryString {

        #region Private fields

        private List<KeyValuePair<string, string>> _query = new List<KeyValuePair<string, string>>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the amount of items stored in the query string.
        /// </summary>
        public int Count {
            get { return _query.Count; }
        }

        /// <summary>
        /// Gets whether the query string is empty.
        /// </summary>
        public bool IsEmpty {
            get { return _query.Count == 0; }
        }

        /// <summary>
        /// Gets an array of the keys of all items in the query string.
        /// </summary>
        public string[] Keys {
            get { return _query.Select(x => x.Key).Distinct().ToArray(); }
        }

        /// <summary>
        /// Gets an array of all items in the query string.
        /// </summary>
        public KeyValuePair<string, string>[] Items {
            get { return _query.ToArray(); }
        }

        /// <summary>
        /// Gets the value of the first item with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the item to match.</param>
        /// <returns>Returns the <see cref="string"/> value of the item, or <code>NULL</code> if not found.</returns>
        public string this[string key] {
            get { return GetString(key); }
        }

        /// <summary>
        /// Gets whether this implementation of <see cref="IHttpQueryString"/> supports duplicate keys.
        /// </summary>
        public bool SupportsDuplicateKeys {
            get { return true; }
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Adds an entry with the specified <code>key</code> and <code>value</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        public void Add(string key, object value) {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            _query.Add(new KeyValuePair<string, string>(key, Convert.ToString(value, CultureInfo.InvariantCulture)));

        }

        /// <summary>
        /// Sets the <code>value</code> of an entry with the specified <code>key</code>. If one or more entries with
        /// <code>key </code> already exist, these will be overwritten.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        public void Set(string key, object value) {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            _query = _query.Where(x => x.Key == key).ToList();
            _query.Add(new KeyValuePair<string, string>(key, Convert.ToString(value, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Gets a string representation of the query string.
        /// </summary>
        /// <returns>Returns the query string as an URL encoded string.</returns>
        public override string ToString() {
            return String.Join("&", from item in _query select HttpUtility.UrlEncode(item.Key) + "=" + HttpUtility.UrlEncode(item.Value));
        }

        /// <summary>
        /// Return whether the query string contains an entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns <code>true</code> if the query string contains the specified <code>key</code>, otherwise
        /// <code>false</code>.</returns>
        public bool ContainsKey(string key) {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");
            return _query.FirstOrDefault(x => x.Key == key).Key != null;
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> value of the first entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.String"/> value of the entry, or <code>null</code> if not found.</returns>
        public string GetString(string key) {
            return GetValue<string>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Int32"/> value of the first entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Int32"/> value of the entry, or <code>0</code> if not found.</returns>
        public int GetInt32(string key) {
            return GetValue<int>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Int64"/> value of the first entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Int64"/> value of the entry, or <code>0</code> if not found.</returns>
        public long GetInt64(string key) {
            return GetValue<long>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Boolean"/> value of the first entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Boolean"/> value of the entry, or <code>0</code> if not found.</returns>
        public bool GetBoolean(string key) {
            return GetValue<bool>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Double"/> value of the first entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Double"/> value of the entry, or <code>0</code> if not found.</returns>
        public double GetDouble(string key) {
            return GetValue<double>(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Single"/> value of the first entry with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <see cref="System.Single"/> value of the entry, or <code>0</code> if not found.</returns>
        public float GetFloat(string key) {
            return GetValue<float>(key);
        }

        /// <summary>
        /// Gets the <code>T</code> value of the first entry first with the specified <code>key</code>.
        /// </summary>
        /// <param name="key">The key of the entry.</param>
        /// <returns>Returns the <code>T</code> value of the entry, or the default value of <code>T</code> if not found.</returns>
        private T GetValue<T>(string key) {

            // Throw an exception if "key" is NULL
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");

            // Get the first entry with "key"
            KeyValuePair<string, string> pair = _query.FirstOrDefault(x => x.Key == key);

            // If either the key or the value is NULL, we return the default value for "T"
            return pair.Key == null || pair.Value == null ? default(T) : (T) Convert.ChangeType(pair.Value, typeof(T), CultureInfo.InvariantCulture);
        
        }

        #endregion

    }

}