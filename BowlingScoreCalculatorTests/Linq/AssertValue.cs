using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculatorTests.Linq
{
	public interface IAssertValue
	{
		object Value { get; }
		bool Inverse { get; }
	}

	public interface IAssertValues
	{
		IEnumerable Values { get; }
		bool Inverse { get; }
	}

	public struct AssertValue<T> : IAssertValue
	{
		public readonly T Value;

		object IAssertValue.Value => Value;

		public bool Inverse { get; }
		public AssertValue<T> Is => this;
		public AssertValue<T> Not => new AssertValue<T>(Value, !Inverse);

		public AssertValue(T value, bool assertInverse = false)
		{
			Value = value;
			Inverse = assertInverse;
		}
	}

	public struct AssertValues<T> : IAssertValues, IAssertValue
	{
		public readonly IEnumerable<T> Values;

		object IAssertValue.Value => Values;
		IEnumerable IAssertValues.Values => Values;

		public bool Inverse { get; }
		public AssertValues<T> Are => this;
		public AssertValues<T> All => this;
		public AssertValues<T> Not => new AssertValues<T>(Values, !Inverse);
		public AssertValue<int> Count => new AssertValue<int>(Values.Count());

		public AssertValues(IEnumerable<T> values, bool assertInverse = false)
		{
			Values = values;
			Inverse = assertInverse;
		}
	}
}
