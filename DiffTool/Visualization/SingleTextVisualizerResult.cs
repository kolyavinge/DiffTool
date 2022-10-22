using System.Collections.Generic;
using DiffTool.Core;

namespace DiffTool.Visualization;

public readonly struct SingleTextVisualizerLineDiff
{
    public readonly DiffKind Kind;

    internal SingleTextVisualizerLineDiff(DiffKind kind)
    {
        Kind = kind;
    }
}

public class SingleTextVisualizerResult
{
    public string Text { get; }

    public IReadOnlyList<SingleTextVisualizerLineDiff> LinesDiff { get; }

    internal SingleTextVisualizerResult(string text, IReadOnlyList<SingleTextVisualizerLineDiff> linesDiff)
    {
        Text = text;
        LinesDiff = linesDiff;
    }
}
