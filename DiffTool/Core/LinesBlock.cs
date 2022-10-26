using System.Collections.Generic;

namespace DiffTool.Core;

internal readonly struct LinesBlock
{
    public readonly int OldPosition;
    public readonly int NewPosition;
    public readonly int LinesCount;

    public LinesBlock(int oldLinePosition, int newLinePosition, int equalLinesCount)
    {
        OldPosition = oldLinePosition;
        NewPosition = newLinePosition;
        LinesCount = equalLinesCount;
    }

    internal class ByEqualLinesCountComparer : IComparer<LinesBlock>
    {
        public int Compare(LinesBlock x, LinesBlock y)
        {
            int result = -1 * x.LinesCount.CompareTo(y.LinesCount);
            if (result != 0) return result;

            result = x.OldPosition.CompareTo(y.OldPosition);
            if (result != 0) return result;

            return x.NewPosition.CompareTo(y.NewPosition);
        }
    }

    internal class ByOldLinePositionComparer : IComparer<LinesBlock>
    {
        public int Compare(LinesBlock x, LinesBlock y)
        {
            return x.OldPosition.CompareTo(y.OldPosition);
        }
    }
}
