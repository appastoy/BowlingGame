using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculatorTests.Linq
{
	static class AssertValueExtensions
	{
		public static Assert True(this AssertValue<bool> value)
		{
			if (value.Inverse)
			{
				Assert.IsFalse(value.Value);
			}
			else
			{
				Assert.IsTrue(value.Value);
			}

			return Assert.That;
		}

		public static Assert False(this AssertValue<bool> value)
		{
			if (value.Inverse)
			{
				Assert.IsTrue(value.Value);
			}
			else
			{
				Assert.IsFalse(value.Value);
			}

			return Assert.That;
		}

		public static Assert Null(this IAssertValue value)
		{
			if (value.Inverse)
			{
				Assert.IsNotNull(value.Value);
			}
			else
			{
				Assert.IsNull(value.Value);
			}
			return Assert.That;
		}

		public static Assert Empty<T>(this AssertValues<T> value)
		{
			if (value.Inverse)
			{
				Assert.IsFalse(value.Values.Count() == 0);
			}
			else
			{
				Assert.IsTrue(value.Values.Count() == 0);
			}

			return Assert.That;
		}

		public static Assert EqualTo<T>(this AssertValue<T> value, T expected)
		{
			if (value.Inverse)
			{
				Assert.AreNotEqual(expected, value.Value);
			}
			else
			{
				Assert.AreEqual(expected, value.Value);
			}

			return Assert.That;
		}

		public static Assert EqualTo<T>(this AssertValues<T> values, T expected)
		{
			if (values.Inverse)
			{
				foreach (var value in values.Values)
				{
					Assert.AreNotEqual(expected, value);
				}
			}
			else
			{
				foreach (var value in values.Values)
				{
					Assert.AreEqual(expected, value);
				}
			}

			return Assert.That;
		}

		public static Assert True(this AssertValues<bool> values)
		{
			if (values.Inverse)
			{
				foreach (var value in values.Values)
				{
					Assert.IsFalse(value);
				}
			}
			else
			{
				foreach (var value in values.Values)
				{
					Assert.IsTrue(value);
				}
			}

			return Assert.That;
		}

		public static Assert False(this AssertValues<bool> values)
		{
			if (values.Inverse)
			{
				foreach (var value in values.Values)
				{
					Assert.IsTrue(value);
				}
			}
			else
			{
				foreach (var value in values.Values)
				{
					Assert.IsFalse(value);
				}
			}

			return Assert.That;
		}
	}
}
