using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BowlingScoreCalculatorTests.Linq
{
	static class AssertExtensions
	{
		public static AssertValue<T> Value<T>(this Assert assert, T value)
		{
			return new AssertValue<T>(value);
		}

		public static AssertValues<T> Values<T>(this Assert assert, IEnumerable<T> value)
		{
			return new AssertValues<T>(value);
		}

		public static Action Action(this Assert assert, Action action)
		{
			return action;
		}

		public static Action Action<T>(this Assert assert, Func<T> func)
		{
			return () => func();
		}

		public static Assert True(this AssertValue<bool> value)
		{
			if (value.AssertInverse)
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
			if (value.AssertInverse)
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
			if (value.AssertInverse)
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
			if (value.AssertInverse)
			{
				Assert.IsFalse(value.Values.Count() == 0);
			}
			else
			{
				Assert.IsTrue(value.Values.Count() == 0);
			}
			
			return Assert.That;
		}

		public static Assert Throws<TException>(this Action action) where TException : Exception
		{
			Assert.ThrowsException<TException>(action);
			return Assert.That;
		}

		public static Assert And(this Assert assert)
		{
			return assert;
		}
	}
}
