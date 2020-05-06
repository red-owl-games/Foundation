using System;

namespace RedOwl.Core
{
	public partial class EnsureThat
	{
		public void Is<T>(T param, T expected) where T : struct, IComparable<T>
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!param.IsEq(expected))
			{
				throw new ArgumentException(EnsureExceptionMessages.Comp_Is_Failed.Inject(param, expected), ParamName);
			}
		}

		public void IsNot<T>(T param, T expected) where T : struct, IComparable<T>
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (param.IsEq(expected))
			{
				throw new ArgumentException(EnsureExceptionMessages.Comp_IsNot_Failed.Inject(param, expected), ParamName);
			}
		}

		public void IsLt<T>(T param, T limit) where T : struct, IComparable<T>
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!param.IsLt(limit))
			{
				throw new ArgumentException(EnsureExceptionMessages.Comp_IsNotLt.Inject(param, limit), ParamName);
			}
		}

		public void IsLte<T>(T param, T limit) where T : struct, IComparable<T>
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (param.IsGt(limit))
			{
				throw new ArgumentException(EnsureExceptionMessages.Comp_IsNotLte.Inject(param, limit), ParamName);
			}
		}

		public void IsGt<T>(T param, T limit) where T : struct, IComparable<T>
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (!param.IsGt(limit))
			{
				throw new ArgumentException(EnsureExceptionMessages.Comp_IsNotGt.Inject(param, limit), ParamName);
			}
		}

		public void IsGte<T>(T param, T limit) where T : struct, IComparable<T>
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (param.IsLt(limit))
			{
				throw new ArgumentException(EnsureExceptionMessages.Comp_IsNotGte.Inject(param, limit), ParamName);
			}
		}

		public void IsInRange<T>(T param, T min, T max) where T : struct, IComparable<T>
		{
			if (!Ensure.IsActive)
			{
				return;
			}

			if (param.IsLt(min))
			{
				throw new ArgumentException(EnsureExceptionMessages.Comp_IsNotInRange_ToLow.Inject(param, min), ParamName);
			}

			if (param.IsGt(max))
			{
				throw new ArgumentException(EnsureExceptionMessages.Comp_IsNotInRange_ToHigh.Inject(param, max), ParamName);
			}
		}
	}
}
