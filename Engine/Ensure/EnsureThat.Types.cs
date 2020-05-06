using System;

namespace RedOwl.Core
{
	public partial class EnsureThat
	{
		public void IsOfType<T>(T param, Type expectedType)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!expectedType.IsInstanceOfType(param))
			{
				throw new ArgumentException(EnsureExceptionMessages.Types_IsOfType_Failed.Inject(expectedType.ToString(), param?.GetType().ToString() ?? "null"), ParamName);
			}
		}

		public void IsOfType(Type param, Type expectedType)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!expectedType.IsAssignableFrom(param))
			{
				throw new ArgumentException(EnsureExceptionMessages.Types_IsOfType_Failed.Inject(expectedType.ToString(), param.ToString()), ParamName);
			}
		}

		public void IsOfType<T>(object param) => IsOfType(param, typeof(T));

		public void IsOfType<T>(Type param) => IsOfType(param, typeof(T));
	}
}
