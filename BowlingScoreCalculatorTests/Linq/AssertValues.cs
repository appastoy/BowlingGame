using System.Collections;
using System.Collections.Generic;

namespace BowlingScoreCalculatorTests.Linq
{
	public interface IAssertValues
	{
		IEnumerable Values { get; }
		bool AssertInverse { get; }
	}

	public struct AssertValues<T> : IAssertValues, IAssertValue
	{
		public readonly IEnumerable<T> Values;

		object IAssertValue.Value => Values;
		IEnumerable IAssertValues.Values => Values;

		public bool AssertInverse { get; }
		public AssertValues<T> Are => this;
		public AssertValues<T> Not => new AssertValues<T>(Values, !AssertInverse);

		public AssertValues(IEnumerable<T> values, bool assertInverse = false)
		{
			Values = values;
			AssertInverse = assertInverse;
		}
	}
}
