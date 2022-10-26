using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

public class DiffEngine
{
    private readonly PrefixSuffixFinder _prefixSuffixFinder;
    private readonly LinesBlockFinder _linesBlockFinder;
    private readonly LinesBlockProcessor _linesBlockProcessor;

    public DiffEngine()
    {
        _prefixSuffixFinder = new PrefixSuffixFinder();
        _linesBlockFinder = new LinesBlockFinder();
        _linesBlockProcessor = new LinesBlockProcessor();
    }

    public DiffResult GetDiff(Text oldText, Text newText)
    {
        var lineDiffs = new List<LineDiff>();

        var prefixLinesCount = _prefixSuffixFinder.GetPrefixLinesCount(oldText, newText);
        for (int i = 0; i < prefixLinesCount; i++)
        {
            lineDiffs.Add(new(DiffKind.Same, i, i));
        }
        if (prefixLinesCount == oldText.EndPosition + 1 && prefixLinesCount == newText.EndPosition + 1)
        {
            return new(lineDiffs);
        }

        var suffixLinesCount = _prefixSuffixFinder.GetSuffixLinesCount(oldText, newText);
        var suffixLineDiffs = new List<LineDiff>();
        for (int i = oldText.EndPosition + 1 - suffixLinesCount; i <= oldText.EndPosition; i++)
        {
            suffixLineDiffs.Add(new(DiffKind.Same, i, i));
        }

        oldText = oldText.GetRange(prefixLinesCount, oldText.EndPosition - suffixLinesCount);
        newText = newText.GetRange(prefixLinesCount, newText.EndPosition - suffixLinesCount);

        if (!oldText.Lines.Any() && newText.Lines.Any())
        {
            lineDiffs.AddRange(newText.Lines.Select(x => new LineDiff(DiffKind.Add, -1, x.Position)));
        }
        else if (oldText.Lines.Any() && !newText.Lines.Any())
        {
            lineDiffs.AddRange(oldText.Lines.Select(x => new LineDiff(DiffKind.Remove, x.Position, -1)));
        }
        else
        {
            var lineBlocks = _linesBlockFinder.FindLongestBlocks(oldText, newText);
            if (lineBlocks.Any())
            {
                lineDiffs.AddRange(_linesBlockProcessor.ProcessLineBlocks(oldText, newText, lineBlocks));
            }
            else
            {
                for (int i = oldText.StartPosition, j = newText.StartPosition; i <= oldText.EndPosition && j <= newText.EndPosition; i++, j++)
                {
                    lineDiffs.Add(new(DiffKind.Change, i, i));
                }
                for (int i = oldText.EndPosition + 1; i <= newText.EndPosition; i++)
                {
                    lineDiffs.Add(new(DiffKind.Add, -1, i));
                }
                for (int i = newText.EndPosition + 1; i <= oldText.EndPosition; i++)
                {
                    lineDiffs.Add(new(DiffKind.Remove, i, -1));
                }
            }
        }

        lineDiffs.AddRange(suffixLineDiffs);

        return new(lineDiffs);
    }
}
