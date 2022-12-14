using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

public class DiffEngine
{
    private readonly PrefixFinder _prefixFinder;
    private readonly LinesBlockFinder _linesBlockFinder;
    private readonly LinesBlockProcessor _linesBlockProcessor;

    public DiffEngine()
    {
        _prefixFinder = new PrefixFinder();
        _linesBlockFinder = new LinesBlockFinder();
        _linesBlockProcessor = new LinesBlockProcessor();
    }

    public DiffResult GetDiff(Text oldText, Text newText)
    {
        var lineDiffs = new List<LineDiff>(Math.Max(oldText.Lines.Count, newText.Lines.Count));

        if (oldText.IsEmpty && newText.IsEmpty)
        {
            return new(lineDiffs);
        }
        else if (oldText.IsEmpty && !newText.IsEmpty)
        {
            lineDiffs.AddRange(newText.Lines.Select(x => new LineDiff(DiffKind.Add, -1, x.Position)));
            return new(lineDiffs);
        }
        else if (!oldText.IsEmpty && newText.IsEmpty)
        {
            lineDiffs.AddRange(oldText.Lines.Select(x => new LineDiff(DiffKind.Remove, x.Position, -1)));
            return new(lineDiffs);
        }

        var prefixLinesCount = _prefixFinder.GetPrefixLinesCount(oldText, newText);
        for (int i = 0; i < prefixLinesCount; i++)
        {
            lineDiffs.Add(new(DiffKind.Same, i, i));
        }
        if (prefixLinesCount == oldText.Lines.Count && prefixLinesCount == newText.Lines.Count)
        {
            return new(lineDiffs);
        }

        oldText = oldText.GetTextRange(prefixLinesCount, oldText.EndPosition);
        newText = newText.GetTextRange(prefixLinesCount, newText.EndPosition);

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

        return new(lineDiffs);
    }
}
