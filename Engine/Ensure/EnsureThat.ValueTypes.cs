using System;

namespace RedOwl.Core
{
	public partial class EnsureThat
	{
		public void IsNotDefault<T>(T param) where T : struct
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (default(T).Equals(param))
			{
				throw new ArgumentException(EnsureExceptionMessages.ValueTypes_IsNotDefault_Failed, ParamName);
			}
		}
	}
}
