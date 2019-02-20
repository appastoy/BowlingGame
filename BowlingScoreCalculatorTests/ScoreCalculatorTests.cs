using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingScoreCalculatorTests.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScoreCalculator.Tests
{
	[TestClass()]
	public class ScoreCalculatorTests
	{
		[TestMethod()]
		public void Calculate_NullOrEmptyTest()
		{
			IEnumerable<int> nullInput = null;
			Assert.That.It(() => ScoreCalculator.Calculate(nullInput)).Throws<ArgumentNullException>();

			var emptyInput = new int[0];
			var emptyResult = ScoreCalculator.Calculate(emptyInput);
			Assert.That.They(emptyResult.Frames).Are.Empty();
			Assert.That.It(emptyResult.IsScoreConfirmed).Is.False();
			Assert.That.It(() => emptyResult.Score).Throws<InvalidOperationException>();
			Assert.That.It(emptyResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_InProgressTest()
		{
			var inProgress = new int[] { 5 };
			var inProgressResult = ScoreCalculator.Calculate(inProgress);
			Assert.That.It(inProgressResult.Frames.Count).Is.EqualTo(1);
			Assert.That.It(inProgressResult.Frames[0].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(inProgressResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(5);
			Assert.That.It(inProgressResult.Frames[0].Rolls[0].Next).Is.Null();
			Assert.That.It(inProgressResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(inProgressResult.Frames[0].IsScoreConfirmed).Is.False();
			Assert.That.It(() => inProgressResult.Frames[0].Score).Throws<InvalidOperationException>();
			Assert.That.It(inProgressResult.IsScoreConfirmed).Is.False();
			Assert.That.It(() => inProgressResult.Score).Throws<InvalidOperationException>();
			Assert.That.It(inProgressResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_OpenTest()
		{
			var open = new int[] { 5, 4 };
			var openResult = ScoreCalculator.Calculate(open);
			Assert.That.It(openResult.Frames.Count).Is.EqualTo(1);
			Assert.That.It(openResult.Frames[0].Rolls.Count).Is.EqualTo(2);
			Assert.That.It(openResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(5);
			Assert.That.It(openResult.Frames[0].Rolls[1].PinScore).Is.EqualTo(4);
			Assert.That.It(openResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(openResult.Frames[0].IsScoreConfirmed).Is.True();
			Assert.That.It(openResult.Frames[0].Score).Is.EqualTo(9);
			Assert.That.It(openResult.IsScoreConfirmed).Is.True();
			Assert.That.It(openResult.Score).Is.EqualTo(9);
			Assert.That.It(openResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_SpareTest()
		{
			var spare = new int[] { 5, 5 };
			var spareResult = ScoreCalculator.Calculate(spare);
			Assert.That.It(spareResult.Frames.Count).Is.EqualTo(1);
			Assert.That.It(spareResult.Frames[0].Rolls.Count).Is.EqualTo(2);
			Assert.That.It(spareResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(5);
			Assert.That.It(spareResult.Frames[0].Rolls[1].PinScore).Is.EqualTo(5);
			Assert.That.It(spareResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(spareResult.Frames[0].IsScoreConfirmed).Is.False();
			Assert.That.It(() => spareResult.Frames[0].Score).Throws<InvalidOperationException>();
			Assert.That.It(spareResult.IsScoreConfirmed).Is.False();
			Assert.That.It(() => spareResult.Score).Throws<InvalidOperationException>();
			Assert.That.It(spareResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_StrikeTest()
		{
			var strike = new int[] { Rule.MaxPinCount };
			var strikeResult = ScoreCalculator.Calculate(strike);
			Assert.That.It(strikeResult.Frames.Count).Is.EqualTo(1);
			Assert.That.It(strikeResult.Frames[0].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(strikeResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(10);
			Assert.That.It(strikeResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(strikeResult.Frames[0].IsScoreConfirmed).Is.False();
			Assert.That.It(() => strikeResult.Frames[0].Score).Throws<InvalidOperationException>();
			Assert.That.It(strikeResult.IsScoreConfirmed).Is.False();
			Assert.That.It(() => strikeResult.Score).Throws<InvalidOperationException>();
			Assert.That.It(strikeResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_SpareBonusTest()
		{
			var spareWithBonus = new int[] { 4, 6, 5 };
			var spareWithBonusResult = ScoreCalculator.Calculate(spareWithBonus);
			Assert.That.It(spareWithBonusResult.Frames.Count).Is.EqualTo(2);
			Assert.That.It(spareWithBonusResult.Frames[0].Rolls.Count).Is.EqualTo(2);
			Assert.That.It(spareWithBonusResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(4);
			Assert.That.It(spareWithBonusResult.Frames[0].Rolls[1].PinScore).Is.EqualTo(6);
			Assert.That.It(spareWithBonusResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(spareWithBonusResult.Frames[0].IsScoreConfirmed).Is.True();
			Assert.That.It(spareWithBonusResult.Frames[0].Score).Is.EqualTo(15);
			Assert.That.It(spareWithBonusResult.Frames[1].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(spareWithBonusResult.Frames[1].Rolls[0].PinScore).Is.EqualTo(5);
			Assert.That.It(spareWithBonusResult.Frames[1].IsLastFrame).Is.False();
			Assert.That.It(spareWithBonusResult.Frames[1].IsScoreConfirmed).Is.False();
			Assert.That.It(() => spareWithBonusResult.Frames[1].Score).Throws<InvalidOperationException>();
			Assert.That.It(spareWithBonusResult.IsScoreConfirmed).Is.True();
			Assert.That.It(spareWithBonusResult.Score).Is.EqualTo(15);
			Assert.That.It(spareWithBonusResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_StrikeBonusTest()
		{
			var strikeWithBonus = new int[] { Rule.MaxPinCount, 5 };
			var strikeWithBonusResult = ScoreCalculator.Calculate(strikeWithBonus);
			Assert.That.It(strikeWithBonusResult.Frames.Count).Is.EqualTo(2);
			Assert.That.It(strikeWithBonusResult.Frames[0].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(strikeWithBonusResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(Rule.MaxPinCount);
			Assert.That.It(strikeWithBonusResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(strikeWithBonusResult.Frames[0].IsScoreConfirmed).Is.False();
			Assert.That.It(() => strikeWithBonusResult.Frames[0].Score).Throws<InvalidOperationException>();
			Assert.That.It(strikeWithBonusResult.Frames[1].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(strikeWithBonusResult.Frames[1].Rolls[0].PinScore).Is.EqualTo(5);
			Assert.That.It(strikeWithBonusResult.Frames[1].IsLastFrame).Is.False();
			Assert.That.It(strikeWithBonusResult.Frames[1].IsScoreConfirmed).Is.False();
			Assert.That.It(() => strikeWithBonusResult.Frames[1].Score).Throws<InvalidOperationException>();
			Assert.That.It(strikeWithBonusResult.IsScoreConfirmed).Is.False();
			Assert.That.It(() => strikeWithBonusResult.Score).Throws<InvalidOperationException>();
			Assert.That.It(strikeWithBonusResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_StrikeOpenTest()
		{
			var strikeOpen = new int[] { Rule.MaxPinCount, 5, 4 };
			var strikeOpenResult = ScoreCalculator.Calculate(strikeOpen);
			Assert.That.It(strikeOpenResult.Frames.Count).Is.EqualTo(2);
			Assert.That.It(strikeOpenResult.Frames[0].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(strikeOpenResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(Rule.MaxPinCount);
			Assert.That.It(strikeOpenResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(strikeOpenResult.Frames[0].IsScoreConfirmed).Is.True();
			Assert.That.It(strikeOpenResult.Frames[0].Score).Is.EqualTo(19);
			Assert.That.It(strikeOpenResult.Frames[1].Rolls.Count).Is.EqualTo(2);
			Assert.That.It(strikeOpenResult.Frames[1].Rolls[0].PinScore).Is.EqualTo(5);
			Assert.That.It(strikeOpenResult.Frames[1].Rolls[1].PinScore).Is.EqualTo(4);
			Assert.That.It(strikeOpenResult.Frames[1].IsLastFrame).Is.False();
			Assert.That.It(strikeOpenResult.Frames[1].IsScoreConfirmed).Is.True();
			Assert.That.It(strikeOpenResult.Frames[1].Score).Is.EqualTo(28);
			Assert.That.It(strikeOpenResult.IsScoreConfirmed).Is.True();
			Assert.That.It(strikeOpenResult.Score).Is.EqualTo(28);
			Assert.That.It(strikeOpenResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_StrikeSpareTest()
		{
			var strikeSpare = new int[] { Rule.MaxPinCount, 5, 5 };
			var strikeSpareResult = ScoreCalculator.Calculate(strikeSpare);
			Assert.That.It(strikeSpareResult.Frames.Count).Is.EqualTo(2);
			Assert.That.It(strikeSpareResult.Frames[0].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(strikeSpareResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(Rule.MaxPinCount);
			Assert.That.It(strikeSpareResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(strikeSpareResult.Frames[0].IsScoreConfirmed).Is.True();
			Assert.That.It(strikeSpareResult.Frames[0].Score).Is.EqualTo(20);
			Assert.That.It(strikeSpareResult.Frames[1].Rolls.Count).Is.EqualTo(2);
			Assert.That.It(strikeSpareResult.Frames[1].Rolls[0].PinScore).Is.EqualTo(5);
			Assert.That.It(strikeSpareResult.Frames[1].Rolls[1].PinScore).Is.EqualTo(5);
			Assert.That.It(strikeSpareResult.Frames[1].IsLastFrame).Is.False();
			Assert.That.It(strikeSpareResult.Frames[1].IsScoreConfirmed).Is.False();
			Assert.That.It(() => strikeSpareResult.Frames[1].Score).Throws<InvalidOperationException>();
			Assert.That.It(strikeSpareResult.IsScoreConfirmed).Is.True();
			Assert.That.It(strikeSpareResult.Score).Is.EqualTo(20);
			Assert.That.It(strikeSpareResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_StrikeStrikeBonusTest()
		{
			var strikeStrikeBonus = new int[] { Rule.MaxPinCount, Rule.MaxPinCount, 5 };
			var strikeStrikeBonusResult = ScoreCalculator.Calculate(strikeStrikeBonus);
			Assert.That.It(strikeStrikeBonusResult.Frames.Count).Is.EqualTo(3);
			Assert.That.It(strikeStrikeBonusResult.Frames[0].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(strikeStrikeBonusResult.Frames[0].Rolls[0].PinScore).Is.EqualTo(Rule.MaxPinCount);
			Assert.That.It(strikeStrikeBonusResult.Frames[0].IsLastFrame).Is.False();
			Assert.That.It(strikeStrikeBonusResult.Frames[0].IsScoreConfirmed).Is.True();
			Assert.That.It(strikeStrikeBonusResult.Frames[0].Score).Is.EqualTo(25);
			Assert.That.It(strikeStrikeBonusResult.Frames[1].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(strikeStrikeBonusResult.Frames[1].Rolls[0].PinScore).Is.EqualTo(Rule.MaxPinCount);
			Assert.That.It(strikeStrikeBonusResult.Frames[1].IsLastFrame).Is.False();
			Assert.That.It(strikeStrikeBonusResult.Frames[1].IsScoreConfirmed).Is.False();
			Assert.That.It(() => strikeStrikeBonusResult.Frames[1].Score).Throws<InvalidOperationException>();
			Assert.That.It(strikeStrikeBonusResult.Frames[2].Rolls.Count).Is.EqualTo(1);
			Assert.That.It(strikeStrikeBonusResult.Frames[2].Rolls[0].PinScore).Is.EqualTo(5);
			Assert.That.It(strikeStrikeBonusResult.Frames[2].IsLastFrame).Is.False();
			Assert.That.It(strikeStrikeBonusResult.Frames[2].IsScoreConfirmed).Is.False();
			Assert.That.It(() => strikeStrikeBonusResult.Frames[2].Score).Throws<InvalidOperationException>();
			Assert.That.It(strikeStrikeBonusResult.IsScoreConfirmed).Is.True();
			Assert.That.It(strikeStrikeBonusResult.Score).Is.EqualTo(25);
			Assert.That.It(strikeStrikeBonusResult.IsGameEnded).Is.False();
		}

		[TestMethod()]
		public void Calculate_AllStrikeTest()
		{
			var allStrike = Enumerable.Range(0, Rule.MaxFrameCount + Rule.MaxLastFrameRollCount - 1).Select(_ => Rule.MaxPinCount);
			var allStrikeResult = ScoreCalculator.Calculate(allStrike);
			Assert.That.It(allStrikeResult.Frames.Count).Is.EqualTo(Rule.MaxFrameCount);
			Assert.That.It(allStrikeResult.Frames.Last().Rolls.Count).Is.EqualTo(Rule.MaxLastFrameRollCount);
			Assert.That.They(allStrikeResult.Frames.SelectMany(frame => frame.Rolls).Select(roll => roll.PinScore)).Are.All.EqualTo(Rule.MaxPinCount);
			Assert.That.They(allStrikeResult.Frames.Select(frame => frame.IsScoreConfirmed)).Are.All.True();
			Assert.That.It(allStrikeResult.IsScoreConfirmed).Is.True();
			Assert.That.It(allStrikeResult.Score).Is.EqualTo(300);
			Assert.That.It(allStrikeResult.IsGameEnded).Is.True();
		}
	}
}