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

			return new ScoreResult(frames, RuleChecker.IsGameEnded(frames));
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
			var frames = new List<Frame>(RuleChecker.MaxFrameCount);
			var frameRolls = new List<Roll>(RuleChecker.MaxRollCount);

			foreach (var roll in rolls)
			{
				var rollIndex = frameRolls.Count;
				frameRolls.Add(roll);

				var isLastFrame = RuleChecker.IsLastFrame(frames.Count);
				if (!isLastFrame && (RuleChecker.IsLastRoll(rollIndex) || RuleChecker.IsStrike(rollIndex, roll.PinScore)))
				{
					frames.Add(CreateFrame(frameRolls, isLastFrame));
					frameRolls.Clear();
				}
			}

			if (frameRolls.Count > 0)
			{
				frames.Add(CreateFrame(frameRolls, RuleChecker.IsLastFrame(frames.Count)));
			}

			return frames;
		}

		static Frame CreateFrame(IReadOnlyList<Roll> frameRolls, bool isLastFrame)
		{
			if (CanCalculateFrameScore(frameRolls))
			{
				return new Frame(frameRolls, isLastFrame, CalculateFrameScore(frameRolls));
			}
			else
			{
				return new Frame(frameRolls, isLastFrame);
			}
		}

		static bool CanCalculateFrameScore(IReadOnlyList<Roll> frameRolls)
		{
			if (RuleChecker.IsFrameOpen(frameRolls)) { return true; }
			if (RuleChecker.IsFrameSpare(frameRolls) && frameRolls[1].Next != null) { return true; }
			if (RuleChecker.IsFrameStrike(frameRolls) && frameRolls[0].Next != null && frameRolls[0].Next.Next != null) { return true; }

			return false;
		}

		static int CalculateFrameScore(IReadOnlyList<Roll> frameRolls)
		{
			if (RuleChecker.IsFrameOpen(frameRolls)) { return frameRolls[0].PinScore + frameRolls[1].PinScore; }
			if (RuleChecker.IsFrameSpare(frameRolls) && frameRolls[1].Next != null) { return frameRolls[0].PinScore + frameRolls[1].PinScore + frameRolls[1].Next.PinScore; }
			if (RuleChecker.IsFrameStrike(frameRolls) && frameRolls[0].Next != null && frameRolls[0].Next.Next != null)
			{
				return frameRolls[0].PinScore + frameRolls[0].Next.PinScore + frameRolls[0].Next.Next.PinScore;
			}

			throw new InvalidOperationException();
		}
	}
}
