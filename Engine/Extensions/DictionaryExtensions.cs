using System.Collections;
using System.Collections.Generic;

namespace RedOwl.Engine
{
	public static class DictionaryUtility
	{
		public static void Merge(this IDictionary destination, IDictionary source)
		{
			IDictionaryEnumerator sourceEnumerator = source.GetEnumerator();

			while (sourceEnumerator.MoveNext())
			{
				if (sourceEnumerator.Key != null && !destination.Contains(sourceEnumerator.Key))
				{
					destination.Add(sourceEnumerator.Key, sourceEnumerator.Value);
				}
			}
		}

		public static void Merge(this IDictionary destination, params IDictionary[] sources)
		{
			foreach (IDictionary source in sources)
			{
				destination.Merge(source);
			}
		}
		
		public static T SafeGet<T, TK>(this IDictionary<TK, T> self, TK key) => self.TryGetValue(key, out T value) == false ? default : value;

	}
}