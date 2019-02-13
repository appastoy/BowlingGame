using System;
using System.Collections.Generic;
using System.Linq;

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

		public ScoreResult(IReadOnlyList<Frame> frames, bool isGameEnded)
		{
			Frames = frames ?? throw new ArgumentNullException(nameof(frames));
			IsGameEnded = isGameEnded;

			score = null;
			if (HasConfirmedScore(Frames))
			{
				score = GetLastConfirmedScore(Frames);
			}
		}

		bool HasConfirmedScore(IEnumerable<Frame> frames)
		{
			return frames.Any(frame => frame.IsScoreConfirmed);
		}

		int GetLastConfirmedScore(IEnumerable<Frame> frames)
		{
			return frames.Last(frame => frame.IsScoreConfirmed).Score;
		}
	}
}
