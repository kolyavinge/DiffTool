namespace DiffTool.Core;

public readonly struct LineDiff
{
    public readonly DiffKind Kind;
    public readonly int OldLine;
    public readonly int NewLine;

    internal LineDiff(DiffKind kind, int oldLine, int newLine)
    {
        Kind = kind;
        OldLine = oldLine;
        NewLine = newLine;
    }
}

public enum DiffKind { Same, Add, Remove, Change }
