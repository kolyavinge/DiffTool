using System.Collections.Generic;
using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class LinesCacheTest
{
    [Test]
    public void LineTwoPositions()
    {
        var cache = new LinesCache(new Text(new List<Line> { new("line", 5), new("line", 6) }));

        Assert.That(cache.GetLinePositions(new("line", 2)), Is.EqualTo(new[] { 5, 6 }));
    }

    [Test]
    public void TextAfterPrefix()
    {
        var cache = new LinesCache(new Text(new List<Line> { new("line5", 5), new("line6", 6) }));

        Assert.That(cache.GetLinePositions(new("line5", 2)), Is.EqualTo(new[] { 5 }));
        Assert.That(cache.GetLinePositions(new("line6", 4)), Is.EqualTo(new[] { 6 }));
    }
}
