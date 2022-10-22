﻿using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

internal class Text
{
    public readonly IReadOnlyList<Line> Lines;

    public Text(string text)
    {
        Lines = text
            .Split(new[] { "\r\n", "\n\r", "\r", "\n" }, StringSplitOptions.None)
            .Select((x, i) => new Line(x, i))
            .ToList();
    }

    private Text(IReadOnlyList<Line> lines)
    {
        Lines = lines;
    }

    public Text GetRange(int fromIndex, int toIndex)
    {
        var range = new List<Line>();
        for (int i = fromIndex; i <= toIndex; i++)
        {
            range.Add(Lines[i]);
        }

        return new(range);
    }
}
