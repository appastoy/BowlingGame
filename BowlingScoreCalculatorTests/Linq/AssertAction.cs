using System;

namespace BowlingScoreCalculatorTests.Linq
{
	struct AssertAction
	{
		public readonly Action Action;

		public AssertAction(Action action) => Action = action;
	}
}
