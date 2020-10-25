using System;

namespace RedOwl.Engine
{
	public partial class EnsureThat
	{
		public void IsNotNull<T>(T? value) where T : struct
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
