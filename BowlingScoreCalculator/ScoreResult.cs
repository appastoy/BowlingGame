using System;
using System.Collections.Generic;

namespace BowlingScoreCalculator
{
	public struct ScoreResult
	{
		readonly int? score;

		public readonly IReadOnlyList<Frame> Frames;
		public readonly bool IsGameEnded;

		public bool IsScoreConfirmed => score.HasValue;
		public int Score
		{
			get
			{
				if (!IsScoreConfirmed) { throw new InvalidOperationException(nameof(Score)); }

				return score.Value;
			}
		}

		public ScoreResult(IReadOnlyList<Frame> frames, bool isGameEnded, int score) : this(frames, isGameEnded, new int?(score)) { }
		public ScoreResult(IReadOnlyList<Frame> frames, bool isGameEnded) : this(frames, isGameEnded, null) { }
		ScoreResult(IReadOnlyList<Frame> frames, bool isGameEnded, int? score)
		{
			Frames = frames ?? throw new ArgumentNullException(nameof(frames));
			IsGameEnded = isGameEnded;
			this.score = null;
		}
	}
}
