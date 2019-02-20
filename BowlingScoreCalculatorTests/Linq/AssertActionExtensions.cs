using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BowlingScoreCalculatorTests.Linq
{
	static class AssertActionExtensions
	{
		public static Assert Throws<TException>(this AssertAction action) where TException : Exception
		{
			Assert.ThrowsException<TException>(action.Action);
			return Assert.That;
		}

		public static Assert NoThrows<TException>(this AssertAction action) where TException : Exception
		{
			try
			{
				action.Action();
			}
			catch (TException)
			{
				Assert.Fail();
			}

			return Assert.That;
		}

		public static Assert NoThrows(this AssertAction action)
		{
			try
			{
				action.Action();
			}
			catch (Exception)
			{
				Assert.Fail();
			}

			return Assert.That;
		}
	}
}
