using System;

namespace RedOwl.Core
{
	public partial class EnsureThat
	{
		public void IsNull<T>(T value)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value != null)
			{
				throw new ArgumentNullException(ParamName, EnsureExceptionMessages.Common_IsNull_Failed);
			}
		}

		public void IsNotNull<T>(T value)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value == null)
			{
				throw new ArgumentNullException(ParamName, EnsureExceptionMessages.Common_IsNotNull_Failed);
			}
		}
	}
}
