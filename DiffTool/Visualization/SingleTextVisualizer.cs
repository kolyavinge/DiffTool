using System.Collections.Generic;
using DiffTool.Core;

namespace DiffTool.Visualization;

public class SingleTextVisualizer
{
    public SingleTextVisualizerResult GetResult(Text oldText, Text newText, IReadOnlyList<LineDiff> linesDiff)
    {
        var resultText = new List<string>();
        var tempNewLinesText = new List<string>();
        var resultLinesDiff = new List<SingleTextVisualizerLineDiff>();
        var tempNewLinesDiff = new List<SingleTextVisualizerLineDiff>();
        int lineIndex = 0;
        LineDiff lineDiff;
        switch (1)
        {
            case 1:
                if (lineIndex == linesDiff.Count) break;
                lineDiff = linesDiff[lineIndex];
                if (lineDiff.Kind == DiffKind.Add)
                {
                    resultText.Add(newText.Lines[lineDiff.NewLine].AsString());
                    resultLinesDiff.Add(new(DiffKind.Add));
                    lineIndex++;
                    goto case 1;
                }
                else if (lineDiff.Kind == DiffKind.Remove)
                {
                    resultText.Add(oldText.Lines[lineDiff.OldLine].AsString());
                    resultLinesDiff.Add(new(DiffKind.Remove));
                    lineIndex++;
                    goto case 1;
                }
                else if (lineDiff.Kind == DiffKind.Same)
                {
                    resultText.Add(oldText.Lines[lineDiff.OldLine].AsString());
                    resultLinesDiff.Add(new(DiffKind.Same));
                    lineIndex++;
                    goto case 1;
                }
                else // Change
                {
                    goto case 2;
                }
            case 2:
                if (lineIndex == linesDiff.Count)
                {
                    resultText.AddRange(tempNewLinesText);
                    resultLinesDiff.AddRange(tempNewLinesDiff);
                    break;
                }
                lineDiff = linesDiff[lineIndex];
                if (lineDiff.Kind == DiffKind.Change)
                {
                    resultText.Add(oldText.Lines[lineDiff.OldLine].AsString());
                    resultLinesDiff.Add(new(DiffKind.Change));
                    tempNewLinesText.Add(newText.Lines[lineDiff.NewLine].AsString());
                    tempNewLinesDiff.Add(new(DiffKind.Change));
                    lineIndex++;
                    goto case 2;
                }
                else
                {
                    resultText.AddRange(tempNewLinesText);
                    tempNewLinesText.Clear();
                    resultLinesDiff.AddRange(tempNewLinesDiff);
                    tempNewLinesDiff.Clear();
                    goto case 1;
                }
        }

        return new(String.Join(Environment.NewLine, resultText), resultLinesDiff);
    }
}
