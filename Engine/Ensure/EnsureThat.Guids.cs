using System;

namespace RedOwl.Core
{
	public partial class EnsureThat
	{
		public void IsNotEmpty(Guid value)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value.Equals(Guid.Empty))
			{
				throw new ArgumentException(EnsureExceptionMessages.Guids_IsNotEmpty_Failed, ParamName);
			}
		}
	}
}
