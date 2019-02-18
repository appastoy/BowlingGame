using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingScoreCalculatorTests.Linq;
using System;
using System.Collections.Generic;

namespace BowlingScoreCalculator.Tests
{
	[TestClass()]
	public class ScoreCalculatorTests
	{
		[TestMethod()]
		public void CalculateTest()
		{
			IEnumerable<int> nullInput = null;
			IEnumerable<int> emptyInput = new int[0];

			Assert.That.Action(() => ScoreCalculator.Calculate(nullInput)).Throws<ArgumentNullException>();

			var scoreResult = ScoreCalculator.Calculate(emptyInput);
			Assert.That.Value(scoreResult).Is.Not.Null();
			Assert.That.Values(scoreResult.Frames).Are.Empty();
			Assert.That.Value(scoreResult.IsScoreConfirmed).Is.False();
			Assert.That.Action(() => scoreResult.Score).Throws<InvalidOperationException>();
			Assert.That.Value(scoreResult.IsGameEnded).Is.False();
		}
	}
}