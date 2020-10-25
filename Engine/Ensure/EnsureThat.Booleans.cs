using System;

namespace RedOwl.Engine
{
	public partial class EnsureThat
	{
		public void IsTrue(bool value)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!value)
			{
				throw new ArgumentException(EnsureExceptionMessages.Booleans_IsTrueFailed, ParamName);
			}
		}

		public void IsFalse(bool value)
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (value)
			{
				throw new ArgumentException(EnsureExceptionMessages.Booleans_IsFalseFailed, ParamName);
			}
		}
	}
}
