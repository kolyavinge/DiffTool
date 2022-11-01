using System.Collections.Generic;
using System.Linq;
using DiffTool.Ancillary;

namespace DiffTool.Core;

internal class ChangedLinesProcessor
{
    private readonly SubstringFinder _substringFinder = new SubstringFinder();
    private readonly SimpleRearranger _rearranger = new SimpleRearranger();

    public IReadOnlyList<LineDiff> GetResult(
        int oldTextStartPosition, int oldTextLinesCount, Text oldText,
        int newTextStartPosition, int newTextLinesCount, Text newText)
    {
        Line[] shortRange;
        Line[] longRange;
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

        var lineWeightMatrix = new int[shortRange.Length, longRange.Length];
        for (int i = 0; i < shortRange.Length; i++)
        {
            var shortRangeContent = shortRange[i].Content;
            for (int j = i; j < longRange.Length; j++)
            {
                lineWeightMatrix[i, j] = _substringFinder.GetResult(shortRangeContent, longRange[j].Content).Sum(x => x.EqualSymbolsCount);
            }
        }

        int maxWeight = int.MinValue;
        var bestRearrange = new int[rearrangeLength];
        void ProcessRearrange(int[] rearrange)
        {
            var currentWeight = 0;
            for (int i = 0; i < rearrangeLength; i++)
            {
                currentWeight += lineWeightMatrix[i, rearrange[i]];
            }
            if (currentWeight > maxWeight)
            {
                maxWeight = currentWeight;
                Array.Copy(rearrange, bestRearrange, rearrangeLength);
            }
        };
        _rearranger.GetRearranges(longRange.Length, rearrangeLength, ProcessRearrange);

        var longChangedLines = new Line[rearrangeLength];
        for (int i = 0; i < rearrangeLength; i++) longChangedLines[i] = longRange[bestRearrange[i]];

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
