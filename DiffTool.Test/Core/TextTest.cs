using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class TextTest
{
    [Test]
    public void GetRange()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetRange(0, 2);

        Assert.That(result.Lines, Has.Count.EqualTo(3));
        Assert.That(result.GetLineByPosition(0).Content, Is.EqualTo("line1"));
        Assert.That(result.GetLineByPosition(1).Content, Is.EqualTo("line2"));
        Assert.That(result.GetLineByPosition(2).Content, Is.EqualTo("line3"));
    }

    [Test]
    public void GetRange_TwoLast()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetRange(1, 2);

        Assert.That(result.Lines, Has.Count.EqualTo(2));
        Assert.That(result.GetLineByPosition(1).Content, Is.EqualTo("line2"));
        Assert.That(result.GetLineByPosition(2).Content, Is.EqualTo("line3"));
    }

    [Test]
    public void GetRange_First()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetRange(0, 0);

        Assert.That(result.Lines, Has.Count.EqualTo(1));
        Assert.That(result.GetLineByPosition(0).Content, Is.EqualTo("line1"));
    }

    [Test]
    public void GetRange_Second()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetRange(1, 1);

        Assert.That(result.Lines, Has.Count.EqualTo(1));
        Assert.That(result.GetLineByPosition(1).Content, Is.EqualTo("line2"));
    }

    [Test]
    public void GetRange_Last()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetRange(2, 2);

        Assert.That(result.Lines, Has.Count.EqualTo(1));
        Assert.That(result.GetLineByPosition(2).Content, Is.EqualTo("line3"));
    }
}
