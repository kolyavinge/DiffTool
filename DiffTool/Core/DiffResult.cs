using System.Collections.Generic;

namespace DiffTool.Core;

public class DiffResult
{
    public IReadOnlyCollection<LineDiff> LinesDiff { get; }

    internal DiffResult(IReadOnlyCollection<LineDiff> linesDiff)
    {
        LinesDiff = linesDiff;
    }
}
