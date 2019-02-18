
namespace BowlingScoreCalculator
{
	public class Roll
	{
		public readonly int PinScore;
		public readonly Roll Next;

		public Roll(int pinFall, Roll next = null)
		{
			PinScore = pinFall;
			Next = next;
		}
	}
}
