using System;
using System.Collections.Generic;

namespace BowlingScoreCalculator
{
	public static class RuleChecker
	{
		public static readonly int MaxPinCount = 10;
		public static readonly int MaxFrameCount = 10;
		public static readonly int LastFrameIndex = MaxFrameCount - 1;
		public static readonly int MaxFrameRollCount = 2;
		public static readonly int FrameLastRollIndex = MaxFrameRollCount - 1;
		public static readonly int MaxLastFrameRollCount = 2;
		public static readonly int LastFrameLastRollIndex = MaxLastFrameRollCount - 1;

		public static bool IsLastFrame(int frameIndex)
		{
			return frameIndex == LastFrameIndex;
		}

		public static bool IsFrameLastRoll(int frameRollIndex)
		{
			return frameRollIndex == FrameLastRollIndex;
		}

		public static bool IsStrike(int frameRollIndex, int pinScore)
		{
			return frameRollIndex == 0 && pinScore == MaxPinCount;
		}

		public static bool IsFrameEnded(int frameIndex, IReadOnlyList<Roll> frameRolls)
		{
			if (frameIndex < 0 || frameIndex >= MaxFrameCount) { throw new ArgumentOutOfRangeException(nameof(frameIndex)); }
			ValidateFrameRolls(frameRolls);

			if (IsLastFrame(frameIndex))
			{
				if (frameRolls.Count < 2) { return false; }
				if (IsFrameOpen(frameRolls) && frameRolls.Count == 2) { return true; }
				if (frameRolls.Count == MaxLastFrameRollCount) { return true; }
			}
			else
			{
				if (IsFrameStrike(frameRolls) && frameRolls.Count == 1) { return true; }
				if (frameRolls.Count == 2) { return true; }
			}

			throw new ArgumentOutOfRangeException(nameof(frameRolls));
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
