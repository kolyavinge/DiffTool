using System.Collections.Generic;

namespace DiffTool.Core;

internal readonly struct LinesBlock
{
    public readonly int OldLinePosition;
    public readonly int NewLinePosition;
    public readonly int LinesCount;

    public LinesBlock(int oldLinePosition, int newLinePosition, int equalLinesCount)
    {
        OldLinePosition = oldLinePosition;
        NewLinePosition = newLinePosition;
        LinesCount = equalLinesCount;
    }

    internal class ByEqualLinesCountComparer : IComparer<LinesBlock>
    {
        public int Compare(LinesBlock x, LinesBlock y)
        {
            int result = -1 * x.LinesCount.CompareTo(y.LinesCount);
            if (result != 0) return result;

            result = x.OldLinePosition.CompareTo(y.OldLinePosition);
            if (result != 0) return result;

            return x.NewLinePosition.CompareTo(y.NewLinePosition);
        }
    }

    internal class ByOldLinePositionComparer : IComparer<LinesBlock>
    {
        public int Compare(LinesBlock x, LinesBlock y)
        {
            return x.OldLinePosition.CompareTo(y.OldLinePosition);
        }
    }
}
