using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculatorTests.Linq
{
	static class AssertExtensions
	{
		public static AssertValue<T> It<T>(this Assert assert, T value)
		{
			return new AssertValue<T>(value);
		}

		public static AssertValues<T> They<T>(this Assert assert, IEnumerable<T> values)
		{
			return new AssertValues<T>(values);
		}

		public static AssertAction It(this Assert assert, Action action)
		{
			return new AssertAction(action);
		}

		public static AssertAction It<TResult>(this Assert assert, Func<TResult> func)
		{
			return new AssertAction(() => func());
		}

		public static Assert And(this Assert assert)
		{
			return assert;
		}
	}
}
