using System.Collections.Generic;

namespace DiffTool.Core;

public class DiffResult
{
    public IReadOnlyList<LineDiff> LinesDiff { get; }

    internal DiffResult(IReadOnlyList<LineDiff> linesDiff)
    {
        LinesDiff = linesDiff;
    }
}
