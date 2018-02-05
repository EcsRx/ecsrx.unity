using System;
using System.Collections.Generic;

namespace EasyLayout
{
	/// <summary>
	/// Extensions.
	/// </summary>
	public static class EasyLayoutExtensions
	{
		/// <summary>
		/// Sums the float.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="list">List.</param>
		/// <param name="calculate">Calculate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static float SumFloat<T>(this List<T> list, Func<T,float> calculate)
		{
			var result = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				result += calculate(list[i]);
			}
			return result;
		}

		/// <summary>
		/// Convert the specified input with converter.
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

