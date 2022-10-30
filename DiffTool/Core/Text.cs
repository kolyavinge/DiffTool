using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

public class Text
{
    private readonly Line[] _lines;

    public readonly int StartPosition;

    public readonly int EndPosition;

    public IEnumerable<Line> Lines => _lines;

    public Text(string text)
    {
        _lines = text
            .Split(new[] { "\r\n", "\n\r", "\r", "\n" }, StringSplitOptions.None)
            .Select((x, i) => new Line(x, i))
            .ToArray();
        StartPosition = 0;
        EndPosition = _lines.Length - 1;
    }

    internal Text(IEnumerable<Line> lines)
    {
        _lines = lines.ToArray();
        StartPosition = _lines.FirstOrDefault()?.Position ?? 0;
        EndPosition = _lines.LastOrDefault()?.Position ?? 0;
    }

    internal Line GetLineByPosition(int position)
    {
        return _lines[position - StartPosition];
    }

    internal Line[] GetLinesRange(int startPosition, int count)
    {
        return _lines.Skip(startPosition - StartPosition).Take(count).ToArray();
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
