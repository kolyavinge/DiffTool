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

    public DiffResult GetDiff(string oldTextString, string newTextString)
    {
        var lineDiffs = new List<LineDiff>();

        var oldText = new Text(oldTextString);
        var newText = new Text(newTextString);

        var prefixLinesCount = _prefixSuffixFinder.GetPrefixLinesCount(oldText, newText);
        for (int i = 0; i < prefixLinesCount; i++)
        {
            lineDiffs.Add(new(DiffKind.Same, i, i));
        }
        if (prefixLinesCount == oldText.Lines.Count && prefixLinesCount == newText.Lines.Count)
        {
            return new(lineDiffs);
        }

        var suffixLinesCount = _prefixSuffixFinder.GetSuffixLinesCount(oldText, newText);
        var suffixLineDiffs = new List<LineDiff>();
        for (int i = oldText.Lines.Count - suffixLinesCount; i < oldText.Lines.Count; i++)
        {
            suffixLineDiffs.Add(new(DiffKind.Same, i, i));
        }

        oldText = oldText.GetRange(prefixLinesCount, oldText.Lines.Count - 1 - suffixLinesCount);
        newText = newText.GetRange(prefixLinesCount, newText.Lines.Count - 1 - suffixLinesCount);

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
                for (int i = 0; i < Math.Min(oldText.Lines.Count, newText.Lines.Count); i++)
                {
                    lineDiffs.Add(new(DiffKind.Change, oldText.Lines[i].Position, newText.Lines[i].Position));
                }
                for (int i = oldText.Lines.Count; i < newText.Lines.Count; i++)
                {
                    lineDiffs.Add(new(DiffKind.Add, -1, i));
                }
                for (int i = newText.Lines.Count; i < oldText.Lines.Count; i++)
                {
                    lineDiffs.Add(new(DiffKind.Remove, i, -1));
                }
            }
        }

        lineDiffs.AddRange(suffixLineDiffs);

        return new(lineDiffs);
    }
}
