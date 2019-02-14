using BowlingScoreCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BowlingScoreCalculator.Tests
{
	[TestClass()]
	public class RuleCheckerTests
	{
		[TestMethod()]
		public void IsLastFrameTest()
		{
			Assert.IsTrue(RuleChecker.IsLastFrame(RuleChecker.LastFrameIndex));
			Assert.IsFalse(RuleChecker.IsLastFrame(RuleChecker.LastFrameIndex + 1));
			Assert.IsFalse(RuleChecker.IsLastFrame(0));
			Assert.IsFalse(RuleChecker.IsLastFrame(-1));
		}

		[TestMethod()]
		public void IsFrameLastRollTest()
		{
			Assert.IsFalse(RuleChecker.IsFrameLastRoll(-1));
			Assert.IsFalse(RuleChecker.IsFrameLastRoll(0));
			Assert.IsFalse(RuleChecker.IsFrameLastRoll(RuleChecker.FrameLastRollIndex + 1));
			Assert.IsTrue(RuleChecker.IsFrameLastRoll(RuleChecker.FrameLastRollIndex));
		}

		[TestMethod()]
		public void IsStrikeTest()
		{
			Assert.IsFalse(RuleChecker.IsStrike(0, int.MaxValue));
			Assert.IsFalse(RuleChecker.IsStrike(0, -1));
			Assert.IsFalse(RuleChecker.IsStrike(-1, 0));
			Assert.IsFalse(RuleChecker.IsStrike(-1, RuleChecker.MaxPinCount));
			Assert.IsTrue(RuleChecker.IsStrike(0,  RuleChecker.MaxPinCount));
		}

		[TestMethod()]
		public void IsFrameEndedTest()
		{
			var inProgress = new Roll[] { new Roll(5, null) };
			var open = new Roll[] { new Roll(5, null), new Roll(4, null) };
			var spare = new Roll[] { new Roll(5, null), new Roll(5, null) };
			var strike = new Roll[] { new Roll(RuleChecker.MaxPinCount, null), new Roll(0, null) };
			Assert.IsFalse(RuleChecker.IsFrameEnded(0, inProgress));
			Assert.IsTrue(RuleChecker.IsFrameEnded(0, open));
			Assert.IsTrue(RuleChecker.IsFrameEnded(0, spare));
			Assert.IsTrue(RuleChecker.IsFrameEnded(0, strike));

			Assert.IsTrue(RuleChecker.IsFrameEnded(RuleChecker.LastFrameIndex, open));
			Assert.IsFalse(RuleChecker.IsFrameEnded(RuleChecker.LastFrameIndex, spare));
			Assert.IsFalse(RuleChecker.IsFrameEnded(RuleChecker.LastFrameIndex, strike));

			var spareInLast = new Roll[] { new Roll(5, null), new Roll(5, null), new Roll(5, null) };
			var strikeInLast = new Roll[] { new Roll(RuleChecker.MaxPinCount, null), new Roll(RuleChecker.MaxPinCount, null), new Roll(3, null) };
			Assert.IsFalse(RuleChecker.IsFrameEnded(RuleChecker.LastFrameIndex, spareInLast));
			Assert.IsFalse(RuleChecker.IsFrameEnded(RuleChecker.LastFrameIndex, strikeInLast));
		}

		[TestMethod()]
		public void IsFrameStrikeTest()
		{
			Assert.Fail();
		}

		[TestMethod()]
		public void IsFrameSpareTest()
		{
			Assert.Fail();
		}

		[TestMethod()]
		public void IsFrameOpenTest()
		{
			Assert.Fail();
		}

		[TestMethod()]
		public void IsGameEndedTest()
		{
			Assert.Fail();
		}
	}
}