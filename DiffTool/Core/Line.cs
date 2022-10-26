namespace DiffTool.Core;

public class Line : IEquatable<Line>
{
    public readonly string Content;

    public readonly int Position;

    internal Line(string content, int position)
    {
        Content = content;
        Position = position;
    }

    public bool Equals(Line? other)
    {
        return Content == other?.Content;
    }

    public override bool Equals(object? obj)
    {
        return Equals((Line?)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Content);
}
