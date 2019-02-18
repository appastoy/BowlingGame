using BowlingScoreCalculatorTests.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace BowlingScoreCalculator.Tests
{
	[TestClass()]
	public class RuleCheckerTests
	{
		readonly Roll[] nullRolls = null;
		readonly Roll[] emptyRolls = new Roll[0];
		readonly Roll[] inProgress = new Roll[] { new Roll(5) };
		readonly Roll[] open = new Roll[] { new Roll(5), new Roll(4) };
		readonly Roll[] spare = new Roll[] { new Roll(5), new Roll(5) };
		readonly Roll[] strike = new Roll[] { new Roll(Rule.MaxPinCount) };
		readonly Roll[] spareWithBonus = new Roll[] { new Roll(5), new Roll(5), new Roll(5) };
		readonly Roll[] strikeWithBonus1 = new Roll[] { new Roll(Rule.MaxPinCount), new Roll(5) };
		readonly Roll[] strikeWithBonus2 = new Roll[] { new Roll(Rule.MaxPinCount), new Roll(5), new Roll(3) };
		readonly Roll[] longRolls = new Roll[] { new Roll(1), new Roll(2), new Roll(3), new Roll(4), new Roll(5), new Roll(6) };

		[TestMethod()]
		public void IsLastFrameTest()
		{
			Assert.That.Value(Rule.IsLastFrame(-1)).Is.False();
			Assert.That.Value(Rule.IsLastFrame(0)).Is.False();
			Assert.That.Value(Rule.IsLastFrame(Rule.LastFrameIndex + 1)).Is.False();
			Assert.That.Value(Rule.IsLastFrame(Rule.LastFrameIndex)).Is.True();
		}

		[TestMethod()]
		public void IsFrameLastRollTest()
		{
			Assert.That.Value(Rule.IsFrameLastRoll(-1)).Is.False();
			Assert.That.Value(Rule.IsFrameLastRoll(0)).Is.False();
			Assert.That.Value(Rule.IsFrameLastRoll(Rule.FrameLastRollIndex + 1)).Is.False();
			Assert.That.Value(Rule.IsFrameLastRoll(Rule.FrameLastRollIndex)).Is.True();
		}

		[TestMethod()]
		public void IsStrikeTest()
		{
			Assert.That.Value(Rule.IsStrike(0, int.MaxValue)).Is.False();
			Assert.That.Value(Rule.IsStrike(0, -1)).Is.False();
			Assert.That.Value(Rule.IsStrike(-1, 0)).Is.False();
			Assert.That.Value(Rule.IsStrike(-1, Rule.MaxPinCount)).Is.False();
			Assert.That.Value(Rule.IsStrike(0,  Rule.MaxPinCount)).Is.True();
		}

		[TestMethod()]
		public void IsFrameEndedTest()
		{
			Assert.That.Action(() => Rule.IsFrameEnded(0, nullRolls)).Throws<ArgumentNullException>();
			Assert.That.Action(() => Rule.IsFrameEnded(0, emptyRolls)).Throws<ArgumentException>();
			Assert.That.Action(() => Rule.IsFrameEnded(0, longRolls)).Throws<ArgumentOutOfRangeException>();
			Assert.That.Value(Rule.IsFrameEnded(0, inProgress)).Is.False();
			Assert.That.Value(Rule.IsFrameEnded(0, open)).Is.True();
			Assert.That.Value(Rule.IsFrameEnded(0, spare)).Is.True();
			Assert.That.Value(Rule.IsFrameEnded(0, strike)).Is.True();
			Assert.That.Value(Rule.IsFrameEnded(Rule.LastFrameIndex, open)).Is.True();
			Assert.That.Value(Rule.IsFrameEnded(Rule.LastFrameIndex, spare)).Is.False();
			Assert.That.Value(Rule.IsFrameEnded(Rule.LastFrameIndex, strike)).Is.False();
			Assert.That.Value(Rule.IsFrameEnded(Rule.LastFrameIndex, spareWithBonus)).Is.True();
			Assert.That.Value(Rule.IsFrameEnded(Rule.LastFrameIndex, strikeWithBonus1)).Is.False();
			Assert.That.Value(Rule.IsFrameEnded(Rule.LastFrameIndex, strikeWithBonus2)).Is.True();
		}

		[TestMethod()]
		public void IsFrameStrikeTest()
		{
			Assert.That.Action(() => Rule.IsFrameStrike(nullRolls)).Throws<ArgumentNullException>();
			Assert.That.Action(() => Rule.IsFrameStrike(emptyRolls)).Throws<ArgumentException>();
			Assert.That.Value(Rule.IsFrameStrike(inProgress)).Is.False();
			Assert.That.Value(Rule.IsFrameStrike(open)).Is.False();
			Assert.That.Value(Rule.IsFrameStrike(spare)).Is.False();
			Assert.That.Value(Rule.IsFrameStrike(strike)).Is.True();
			Assert.That.Value(Rule.IsFrameStrike(spareWithBonus)).Is.False();
			Assert.That.Value(Rule.IsFrameStrike(strikeWithBonus1)).Is.True();
			Assert.That.Value(Rule.IsFrameStrike(strikeWithBonus2)).Is.True();
		}

		[TestMethod()]
		public void IsFrameSpareTest()
		{
			Assert.That.Action(() => Rule.IsFrameSpare(nullRolls)).Throws<ArgumentNullException>();
			Assert.That.Action(() => Rule.IsFrameSpare(emptyRolls)).Throws<ArgumentException>();
			Assert.That.Value(Rule.IsFrameSpare(inProgress)).Is.False();
			Assert.That.Value(Rule.IsFrameSpare(open)).Is.False();
			Assert.That.Value(Rule.IsFrameSpare(spare)).Is.True();
			Assert.That.Value(Rule.IsFrameSpare(strike)).Is.False();
			Assert.That.Value(Rule.IsFrameSpare(spareWithBonus)).Is.True();
			Assert.That.Value(Rule.IsFrameSpare(strikeWithBonus1)).Is.False();
			Assert.That.Value(Rule.IsFrameSpare(strikeWithBonus2)).Is.False();
		}

		[TestMethod()]
		public void IsFrameOpenTest()
		{
			Assert.That.Action(() => Rule.IsFrameOpen(nullRolls)).Throws<ArgumentNullException>();
			Assert.That.Action(() => Rule.IsFrameOpen(emptyRolls)).Throws<ArgumentException>();
			Assert.That.Value(Rule.IsFrameOpen(inProgress)).Is.False();
			Assert.That.Value(Rule.IsFrameOpen(open)).Is.True();
			Assert.That.Value(Rule.IsFrameOpen(spare)).Is.False();
			Assert.That.Value(Rule.IsFrameOpen(strike)).Is.False();
			Assert.That.Value(Rule.IsFrameOpen(spareWithBonus)).Is.False();
			Assert.That.Value(Rule.IsFrameOpen(strikeWithBonus1)).Is.False();
			Assert.That.Value(Rule.IsFrameOpen(strikeWithBonus2)).Is.False();
		}

		[TestMethod()]
		public void IsGameEndedTest()
		{
			Frame[] nullFrames = null;
			var emptyFrames = new Frame[0];
			var inProgressFrames = new Frame[] { new Frame(new Roll[] { new Roll(0) }, false) };
			var completeFrames = Enumerable.Range(0, Rule.MaxFrameCount).Select(i => new Frame(open, i == Rule.LastFrameIndex, open.Sum(o => o.PinScore))).ToArray();


			Assert.That.Action(() => Rule.IsGameEnded(nullFrames)).Throws<ArgumentNullException>();
			Assert.That.Value(Rule.IsGameEnded(emptyFrames)).Is.False();
			Assert.That.Value(Rule.IsGameEnded(inProgressFrames)).Is.False();
			Assert.That.Value(Rule.IsGameEnded(completeFrames)).Is.True();
		}
	}
}