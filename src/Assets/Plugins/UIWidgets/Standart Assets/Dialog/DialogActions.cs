using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace UIWidgets {

	/// <summary>
	/// Dialog actions.
	/// Key - button name.
	/// Value - action on click.
	/// </summary>
	public class DialogActions : IDictionary<string,Func<bool>> {
		List<string> keys = new List<string>();
		List<Func<bool>> values = new List<Func<bool>>();
		List<KeyValuePair<string,Func<bool>>> elements = new List<KeyValuePair<string,Func<bool>>>();

		/// <summary>
		/// Gets the number of elements contained in the dictionary.
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get {
				return elements.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public bool IsReadOnly {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets or sets the element with the specified key.
		/// </summary>
		/// <returns>The element with the specified key.</returns>
		/// <param name="key">The key of the element to get or set.</param>
		public Func<bool> this[string key] {
			get {
				var index = keys.IndexOf(key);
				return elements[index].Value;
			}
			set {
				var index = keys.IndexOf(key);
				elements[index] = new KeyValuePair<string, Func<bool>>(key, value);
			}
		}

		/// <summary>
		/// Gets an ICollection<string> containing the keys of the dictionary.
		/// </summary>
		/// <value>The keys.</value>
		public ICollection<string> Keys {
			get {
				return elements.Convert<KeyValuePair<string,Func<bool>>,string>(GetKey);
			}
		}

		string GetKey(KeyValuePair<string,Func<bool>> item)
		{
			return item.Key;
		}

		/// <summary>
		/// Gets an ICollection<Func<bool>> containing the values in the dictionary.
		/// </summary>
		/// <value>The values.</value>
		public ICollection<Func<bool>> Values {
			get {
				return elements.Convert<KeyValuePair<string,Func<bool>>,Func<bool>>(GetValue);
			}
		}

		Func<bool> GetValue(KeyValuePair<string, Func<bool>> item)
		{
			return item.Value;
		}

		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add(KeyValuePair<string,Func<bool>> item)
		{
			Add(item.Key, item.Value);
		}

		/// <summary>
		/// Add the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void Add(string key, Func<bool> value)
		{
			if (key==null)
			{
				throw new ArgumentNullException("key", "Key is null.");
			}
			if (ContainsKey(key))
			{
				throw new ArgumentException(string.Format("An element with the same key ({0}) already exists.", key));
			}
			keys.Add(key);
			values.Add(value);
			elements.Add(new KeyValuePair<string, Func<bool>>(key, value));
		}

		/// <summary>
		/// Removes all items.
		/// </summary>
		public void Clear()
		{
			keys.Clear();
			values.Clear();
			elements.Clear();
		}

		/// <summary>
		/// Determines whether contains a specific value.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Contains(KeyValuePair<string,Func<bool>> item)
		{
			return elements.Contains(item);
		}

		/// <summary>
		/// Determines whether the IDictionary<TKey,TValue> contains an element with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
		public bool ContainsKey(string key)
		{
			if (key==null)
			{
				throw new ArgumentNullException("key", "Key is null.");
			}

			return keys.Contains(key);
		}

		/// <summary>
		/// Copies the elements of the KeyValuePair<string,Func<bool>> to an Array, starting at a particular Array index.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="arrayIndex">Array index.</param>
		public void CopyTo(KeyValuePair<string,Func<bool>>[] array, int arrayIndex)
		{
			elements.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		public IEnumerator<KeyValuePair<string,Func<bool>>> GetEnumerator()
		{
			return elements.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return elements.GetEnumerator();
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the dictionary.
		/// </summary>
		/// <param name="item">Item.</param>
		public bool Remove(KeyValuePair<string,Func<bool>> item)
		{
			if (!elements.Contains(item))
			{
				return false;
			}
			var index = elements.IndexOf(item);
			keys.RemoveAt(index);
			values.RemoveAt(index);
			elements.RemoveAt(index);

			return true;
		}

		/// <summary>
		/// Removes the element with the specified key from the dictionary.
		/// </summary>
		/// <param name="key">Key.</param>
		public bool Remove(string key)
		{
			if (key==null)
			{
				throw new ArgumentNullException("key", "Key is null.");
			}

			if (!ContainsKey(key))
			{
				return false;
			}
			var index = keys.IndexOf(key);
			keys.RemoveAt(index);
			values.RemoveAt(index);
			elements.RemoveAt(index);

			return true;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <returns><c>true</c>, if get value was tryed, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public bool TryGetValue(string key, out Func<bool> value)
		{
			if (key==null)
			{
				throw new ArgumentNullException("key", "Key is null.");
			}

			if (!ContainsKey(key))
			{
				value = default(Func<bool>);
				return false;
			}

			value = values[keys.IndexOf(key)];
			return true;
		}
	}
}