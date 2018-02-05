using System;
using System.Collections.Generic;
using System.Reflection;

namespace UIWidgets {
	/// <summary>
	/// For each extensions.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Foreach with index.
		/// </summary>
		/// <param name="enumerable">Enumerable.</param>
		/// <param name="handler">Handler.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T,int> handler)
		{
			int i = 0;
			foreach (T item in enumerable)
			{
				handler(item, i);
				i++;
			}
		}
		
		/// <summary>
		/// Foreach.
		/// </summary>
		/// <param name="enumerable">Enumerable.</param>
		/// <param name="handler">Handler.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> handler)
		{
			foreach (T item in enumerable)
			{
				handler(item);
			}
		}
		
		/// <summary>
		/// Convert IEnumerable<T> to ObservableList<T>.
		/// </summary>
		/// <returns>The observable list.</returns>
		/// <param name="enumerable">Enumerable.</param>
		/// <param name="observeItems">Is need to observe items? If true ObservableList.OnChange will be raised on item OnChange or PropertyChanged.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static ObservableList<T> ToObservableList<T>(this IEnumerable<T> enumerable, bool observeItems = true)
		{
			return new ObservableList<T>(enumerable, observeItems);
		}

		/// <summary>
		/// Sums the float.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="list">List.</param>
		/// <param name="calculate">Calculate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static float SumFloat<T>(this IList<T> list, Func<T,float> calculate)
		{
			var result = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				result += calculate(list[i]);
			}
			return result;
		}

		/// <summary>
		/// Sums the float.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="list">List.</param>
		/// <param name="calculate">Calculate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static float SumFloat<T>(this ObservableList<T> list, Func<T,float> calculate)
		{
			var result = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				result += calculate(list[i]);
			}
			return result;
		}

		/// <summary>
		/// Convert the specified list with converter.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="converter">Converter.</param>
		/// <typeparam name="TInput">The 1st type parameter.</typeparam>
		/// <typeparam name="TOutput">The 2nd type parameter.</typeparam>
		static public List<TOutput> Convert<TInput,TOutput>(this List<TInput> input, Converter<TInput,TOutput> converter)
		{
			#if NETFX_CORE
			var output = new List<TOutput>(input.Count);
			for (int i = 0; i < input.Count; i++)
			{
				output.Add(converter(input[i]));
			}
			
			return output;
			#else
			return input.ConvertAll<TOutput>(converter);
			#endif
		}
		
		#if NETFX_CORE
		/// <summary>
		/// Determines if is assignable from the specified source from.
		/// </summary>
		/// <returns><c>true</c> if is assignable from the specified source from; otherwise, <c>false</c>.</returns>
		/// <param name="source">Source.</param>
		/// <param name="from">From.</param>
		static public bool IsAssignableFrom(this Type source, Type from)
		{
			return source.GetTypeInfo().IsAssignableFrom(from.GetTypeInfo());
		}

		/// <summary>
		/// Apply action for each item in list.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="action">Action.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		static public void ForEach<T>(this List<T> list, Action<T> action)
		{
			for (int i = 0; i < list.Count; i++)
			{
				action(list[i]);
			}
		}
		#endif
	}
}