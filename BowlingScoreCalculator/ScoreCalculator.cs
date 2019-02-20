using System.Collections.Generic;
using System;
using System.Linq;

namespace BowlingScoreCalculator
{
    public static class ScoreCalculator
    {
		public static ScoreResult Calculate(IEnumerable<int> pinScores)
		{
			if (pinScores == null) { throw new ArgumentNullException(nameof(pinScores)); }

			var rolls = CreateRolls(pinScores);
			var frames = CreateFrames(rolls);

			return CalculateScoreResult(frames);
		}

		static IEnumerable<Roll> CreateRolls(IEnumerable<int> pinScores)
		{
			Roll nextRoll = null;
			return pinScores.Reverse()
							.Select(pinFall => nextRoll = new Roll(pinFall, nextRoll))
							.Reverse()
							.ToArray();
		}

		static IReadOnlyList<Frame> CreateFrames(IEnumerable<Roll> rolls)
		{
			var frames = new List<Frame>(Rule.MaxFrameCount);
			var frameRolls = new List<Roll>(Rule.MaxFrameRollCount);

			foreach (var roll in rolls)
			{
				var rollIndex = frameRolls.Count;
				frameRolls.Add(roll);

				if (Rule.IsFrameEnded(frames.Count, frameRolls))
				{
					frames.Add(CreateFrame(frameRolls.ToArray(), GetPrevFrameScore(frames), Rule.IsLastFrame(frames.Count)));
					frameRolls.Clear();
				}
			}

			if (frameRolls.Count > 0)
			{
				frames.Add(CreateFrame(frameRolls.ToArray(), GetPrevFrameScore(frames), Rule.IsLastFrame(frames.Count)));
			}

			return frames;
		}

		static int GetPrevFrameScore(IReadOnlyList<Frame> frames)
		{
			if (frames.Count > 0 && frames[frames.Count - 1].IsScoreConfirmed)
			{
				return frames[frames.Count - 1].Score;
			}

			return 0;
		}

		static Frame CreateFrame(IReadOnlyList<Roll> frameRolls, int prevFrameScore, bool isLastFrame)
		{
			if (CanCalculateFrameScore(frameRolls))
			{
				return new Frame(frameRolls, isLastFrame, CalculateFrameScore(prevFrameScore, frameRolls));
			}
			else
			{
				return new Frame(frameRolls, isLastFrame);
			}
		}

		static bool CanCalculateFrameScore(IReadOnlyList<Roll> frameRolls)
		{
			if (Rule.IsFrameOpen(frameRolls)) { return true; }
			if (Rule.IsFrameSpare(frameRolls) && frameRolls[1].Next != null) { return true; }
			if (Rule.IsFrameStrike(frameRolls) && frameRolls[0].Next != null && frameRolls[0].Next.Next != null) { return true; }

			return false;
		}

		static int CalculateFrameScore(int prevFrameScore, IReadOnlyList<Roll> frameRolls)
		{
			if (Rule.IsFrameOpen(frameRolls)) { return prevFrameScore + frameRolls[0].PinScore + frameRolls[1].PinScore; }
			if (Rule.IsFrameSpare(frameRolls) && frameRolls[1].Next != null) { return prevFrameScore + frameRolls[0].PinScore + frameRolls[1].PinScore + frameRolls[1].Next.PinScore; }
			if (Rule.IsFrameStrike(frameRolls) && frameRolls[0].Next != null && frameRolls[0].Next.Next != null)
			{
				return prevFrameScore + frameRolls[0].PinScore + frameRolls[0].Next.PinScore + frameRolls[0].Next.Next.PinScore;
			}

			throw new InvalidOperationException();
		}

		static ScoreResult CalculateScoreResult(IReadOnlyList<Frame> frames)
		{
			if (HasConfirmedScore(frames))
			{
				return new ScoreResult(frames, Rule.IsGameEnded(frames), GetLastConfirmedScore(frames));
			}
			else
			{
				return new ScoreResult(frames, Rule.IsGameEnded(frames));
			}
		}

		static bool HasConfirmedScore(IEnumerable<Frame> frames)
		{
			return frames.Any(frame => frame.IsScoreConfirmed);
		}

		static int GetLastConfirmedScore(IEnumerable<Frame> frames)
		{
			return frames.Last(frame => frame.IsScoreConfirmed).Score;
		}
	}
}
