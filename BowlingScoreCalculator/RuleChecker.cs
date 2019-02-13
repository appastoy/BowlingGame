using System;
using System.Collections.Generic;

namespace BowlingScoreCalculator
{
	public static class RuleChecker
	{
		public static readonly int MaxPinCount = 10;
		public static readonly int MaxFrameCount = 10;
		public static readonly int LastFrameIndex = MaxFrameCount - 1;
		public static readonly int MaxRollCount = 2;
		public static readonly int LastRollIndex = MaxRollCount - 1;

		public static bool IsLastFrame(int frameIndex)
		{
			return frameIndex == LastFrameIndex;
		}

		public static bool IsLastRoll(int rollIndex)
		{
			return rollIndex == LastRollIndex;
		}

		public static bool IsStrike(int rollIndex, int pinScore)
		{
			return rollIndex == 0 && pinScore == MaxRollCount;
		}

		public static bool IsFrameStrike(IReadOnlyList<Roll> frameRolls)
		{
			ValidateFrameRolls(frameRolls);

			return frameRolls[0].PinScore == MaxPinCount;
		}

		public static bool IsFrameSpare(IReadOnlyList<Roll> frameRolls)
		{
			ValidateFrameRolls(frameRolls);

			return !IsFrameStrike(frameRolls) && frameRolls.Count >= 2 && frameRolls[0].PinScore + frameRolls[1].PinScore == MaxPinCount;
		}

		public static bool IsFrameOpen(IReadOnlyList<Roll> frameRolls)
		{
			ValidateFrameRolls(frameRolls);

			return frameRolls.Count >= 2 && frameRolls[0].PinScore + frameRolls[1].PinScore < MaxPinCount;
		}

		static void ValidateFrameRolls(IReadOnlyList<Roll> frameRolls)
		{
			if (frameRolls == null) { throw new ArgumentNullException(nameof(frameRolls)); }
			if (frameRolls.Count == 0) { throw new ArgumentException(nameof(frameRolls)); }
		}

		public static bool IsGameEnded(IReadOnlyList<Frame> frames)
		{
			if (frames == null) { throw new ArgumentNullException(nameof(frames)); }

			return frames.Count == MaxFrameCount && frames[LastFrameIndex].IsScoreConfirmed;
		}
	}
}
