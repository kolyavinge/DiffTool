using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

public class Text
{
    private readonly List<Line> _lines;

    public readonly int StartPosition;

    public readonly int EndPosition;

    public IEnumerable<Line> Lines => _lines;

    public Text(string text)
    {
        _lines = text
            .Split(new[] { "\r\n", "\n\r", "\r", "\n" }, StringSplitOptions.None)
            .Select((x, i) => new Line(x, i))
            .ToList();
        StartPosition = 0;
        EndPosition = _lines.Count - 1;
    }

    internal Text(List<Line> lines)
    {
        _lines = lines;
        StartPosition = _lines.FirstOrDefault()?.Position ?? 0;
        EndPosition = _lines.LastOrDefault()?.Position ?? 0;
    }

    public Line GetLineByPosition(int position)
    {
        return _lines[position - StartPosition];
    }

    internal Text GetRange(int fromIndex, int toIndex)
    {
        var range = new List<Line>();
        for (int i = fromIndex; i <= toIndex; i++)
        {
            range.Add(_lines[i]);
        }

        return new(range);
    }
}
