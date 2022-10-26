using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

internal class LinesCache
{
    private readonly Dictionary<string, HashSet<int>> _positions;

    public LinesCache(Text text)
    {
        _positions = text.Lines
            .Select(x => new { Line = x.Content, Position = x.Position })
            .GroupBy(x => x.Line)
            .ToDictionary(k => k.Key, v => v.Select(x => x.Position).ToHashSet());
    }

    public IReadOnlyCollection<int>? GetLinePositions(Line line)
    {
        return _positions.TryGetValue(line.Content, out HashSet<int> positions) ? positions : null;
    }
}
