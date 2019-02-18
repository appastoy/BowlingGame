
namespace BowlingScoreCalculatorTests.Linq
{
	public interface IAssertValue
	{
		object Value { get; }
		bool AssertInverse { get; }
	}

	public struct AssertValue<T> : IAssertValue
	{
		public readonly T Value;

		object IAssertValue.Value => Value;

		public bool AssertInverse { get; }
		public AssertValue<T> Is => this;
		public AssertValue<T> Not => new AssertValue<T>(Value, !AssertInverse);

		public AssertValue(T value, bool assertInverse = false)
		{
			Value = value;
			AssertInverse = assertInverse;
		}
	}
}
