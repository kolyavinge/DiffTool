using System.Collections.Generic;
using DiffTool.Core;

namespace DiffTool.Visualization;

public readonly struct SingleTextVisualizerLineDiff
{
    public readonly DiffKind DiffKind;
    public readonly TextKind TextKind;

    internal SingleTextVisualizerLineDiff(DiffKind diffKind, TextKind textKind = TextKind.Unspecified)
    {
        TextKind = textKind;
        DiffKind = diffKind;
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
