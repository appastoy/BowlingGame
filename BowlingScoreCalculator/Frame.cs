using System;
using System.Collections.Generic;

namespace BowlingScoreCalculator
{
	public struct Frame
	{
		readonly int? score;
		
		public readonly IReadOnlyList<Roll> Rolls;
		public readonly bool IsLastFrame;

		public bool IsScoreConfirmed => score.HasValue;
		public int Score
		{
			get
			{
				if (!IsScoreConfirmed) { throw new InvalidOperationException(nameof(Score)); }

				return score.Value;
			}
		}

		public Frame(IReadOnlyList<Roll> rolls, bool isLastFrame) : this(rolls, isLastFrame, null) { }
		public Frame(IReadOnlyList<Roll> rolls, bool isLastFrame, int score) : this(rolls, isLastFrame, new int?(score)) { }

		Frame(IReadOnlyList<Roll> rolls, bool isLastFrame, int? score)
		{
			Rolls = rolls ?? throw new ArgumentNullException(nameof(rolls));
			IsLastFrame = isLastFrame;
			this.score = score;
		}
	}
}
