using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RedOwl.Engine
{
	public partial class EnsureThat
	{
		public void HasItems<T>(T value) where T : class, ICollection
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			IsNotNull(value);

			if (value.Count < 1)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_HasItemsFailed, ParamName);
			}
		}

		public void HasItems<T>(ICollection<T> value)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			IsNotNull(value);

			if (value.Count < 1)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_HasItemsFailed, ParamName);
			}
		}

		public void HasItems<T>(T[] value)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			IsNotNull(value);

			if (value.Length < 1)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_HasItemsFailed, ParamName);
			}
		}

		public void HasNoNullItem<T>(T value) where T : class, IEnumerable
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			IsNotNull(value);

			foreach (var item in value)
			{
				if (item == null)
				{
					throw new ArgumentException(EnsureExceptionMessages.Collections_HasNoNullItemFailed, ParamName);
				}
			}
		}

		public void HasItems<T>(IList<T> value) => HasItems(value as ICollection<T>);

		public void HasItems<TKey, TValue>(IDictionary<TKey, TValue> value)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			IsNotNull(value);

			if (value.Count < 1)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_HasItemsFailed, ParamName);
			}
		}

		public void SizeIs<T>(T[] value, int expected)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Length != expected)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_SizeIs_Failed.Inject(expected, value.Length), ParamName);
			}
		}

		public void SizeIs<T>(T[] value, long expected)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Length != expected)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_SizeIs_Failed.Inject(expected, value.Length), ParamName);
			}
		}

		public void SizeIs<T>(T value, int expected) where T : ICollection
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Count != expected)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_SizeIs_Failed.Inject(expected, value.Count), ParamName);
			}
		}

		public void SizeIs<T>(T value, long expected) where T : ICollection
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Count != expected)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_SizeIs_Failed.Inject(expected, value.Count), ParamName);
			}
		}

		public void SizeIs<T>(ICollection<T> value, int expected)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Count != expected)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_SizeIs_Failed.Inject(expected, value.Count), ParamName);
			}
		}

		public void SizeIs<T>(ICollection<T> value, long expected)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Count != expected)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_SizeIs_Failed.Inject(expected, value.Count), ParamName);
			}
		}

		public void SizeIs<T>(IList<T> value, int expected) => SizeIs(value as ICollection<T>, expected);

		public void SizeIs<T>(IList<T> value, long expected) => SizeIs(value as ICollection<T>, expected);

		public void SizeIs<TKey, TValue>(IDictionary<TKey, TValue> value, int expected)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Count != expected)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_SizeIs_Failed.Inject(expected, value.Count), ParamName);
			}
		}

		public void SizeIs<TKey, TValue>(IDictionary<TKey, TValue> value, long expected)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Count != expected)
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_SizeIs_Failed.Inject(expected, value.Count), ParamName);
			}
		}

		public void IsKeyOf<TKey, TValue>(IDictionary<TKey, TValue> value, TKey expectedKey, string keyLabel = null)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!value.ContainsKey(expectedKey))
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_ContainsKey_Failed.Inject(expectedKey, keyLabel ?? ParamName.Prettify()), ParamName);
			}
		}

		public void Any<T>(IList<T> value, Func<T, bool> predicate) => Any(value as ICollection<T>, predicate);

		public void Any<T>(ICollection<T> value, Func<T, bool> predicate)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!value.Any(predicate))
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_Any_Failed, ParamName);
			}
		}

		public void Any<T>(T[] value, Func<T, bool> predicate)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!value.Any(predicate))
			{
				throw new ArgumentException(EnsureExceptionMessages.Collections_Any_Failed, ParamName);
			}
		}
	}
}
