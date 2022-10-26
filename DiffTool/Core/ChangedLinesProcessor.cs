using System.Collections.Generic;
using System.Linq;
using DiffTool.Ancillary;

namespace DiffTool.Core;

internal class ChangedLinesProcessor
{
    private readonly SubstringFinder _substringFinder = new SubstringFinder();
    private readonly SimpleRearranger _rearranger = new SimpleRearranger();

    public IReadOnlyList<LineDiff> GetResult(
        int oldTextStartPosition,
        int oldTextLinesCount,
        Text oldText,
        int newTextStartPosition,
        int newTextLinesCount,
        Text newText)
    {
        IReadOnlyList<Line> shortRange;
        IReadOnlyList<Line> longRange;
        int rearrangeLength;
        if (oldTextLinesCount < newTextLinesCount)
        {
            shortRange = oldText.GetLinesRange(oldTextStartPosition, oldTextLinesCount);
            longRange = newText.GetLinesRange(newTextStartPosition, newTextLinesCount);
            rearrangeLength = oldTextLinesCount;
        }
        else
        {
            shortRange = newText.GetLinesRange(newTextStartPosition, newTextLinesCount);
            longRange = oldText.GetLinesRange(oldTextStartPosition, oldTextLinesCount);
            rearrangeLength = newTextLinesCount;
        }
        int maxCount = int.MinValue;
        var longChangedLines = new Line[rearrangeLength];
        var substringCache = new Dictionary<(int, int), int>();
        var rearranges = _rearranger.GetRearranges(longRange.Count, rearrangeLength).ToList();
        foreach (var rearrange in rearranges)
        {
            var currentCount = 0;
            for (int i = 0; i < rearrange.Length; i++)
            {
                var shortRangeLine = shortRange[i];
                var longRangeLine = longRange[rearrange[i]];
                var key = (shortRangeLine.Position, longRangeLine.Position);
                if (substringCache.TryGetValue(key, out int sum))
                {
                    currentCount += sum;
                }
                else
                {
                    var result = _substringFinder.GetResult(shortRangeLine.Content, longRangeLine.Content);
                    sum = result.Sum(x => x.Count);
                    substringCache.Add((shortRangeLine.Position, longRangeLine.Position), sum);
                    currentCount += sum;
                }
            }
            if (currentCount > maxCount)
            {
                maxCount = currentCount;
                for (int i = 0; i < rearrange.Length; i++) longChangedLines[i] = longRange[rearrange[i]];
            }
        };
        if (oldTextLinesCount < newTextLinesCount)
        {
            var addedPositions = new HashSet<int>(longRange.Select(x => x.Position));
            addedPositions.ExceptWith(longChangedLines.Select(x => x.Position));
            var result = longChangedLines
                .Select((_, i) => new LineDiff(DiffKind.Change, shortRange[i].Position, longChangedLines[i].Position))
                .Union(addedPositions.Select(addedPosition => new LineDiff(DiffKind.Add, -1, addedPosition)))
                .OrderBy(x => x.NewLine).ToList();

            return result;
        }
        else
        {
            var removedPositions = new HashSet<int>(longRange.Select(x => x.Position));
            removedPositions.ExceptWith(longChangedLines.Select(x => x.Position));
            var result = longChangedLines
                .Select((_, i) => new LineDiff(DiffKind.Change, longChangedLines[i].Position, shortRange[i].Position))
                .Union(removedPositions.Select(removedPosition => new LineDiff(DiffKind.Remove, removedPosition, -1)))
                .OrderBy(x => x.OldLine).ToList();

            return result;
        }
    }
}
