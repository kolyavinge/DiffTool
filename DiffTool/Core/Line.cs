namespace DiffTool.Core;

public class Line : IEquatable<Line>
{
    private readonly string _line;

    public readonly int Position;

    internal Line(string line, int position)
    {
        _line = line;
        Position = position;
    }

    public string AsString()
    {
        return _line;
    }

    public bool Equals(Line? other)
    {
        return _line == other?._line;
    }

    public override bool Equals(object? obj)
    {
        return Equals((Line?)obj);
    }

    public override int GetHashCode() => HashCode.Combine(_line);
}
