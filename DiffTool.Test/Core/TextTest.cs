using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class TextTest
{
    [Test]
    public void GetTextRange()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetTextRange(0, 2);

        Assert.That(result.Lines, Has.Length.EqualTo(3));
        Assert.That(result.GetLineByPosition(0).Content, Is.EqualTo("line1"));
        Assert.That(result.GetLineByPosition(1).Content, Is.EqualTo("line2"));
        Assert.That(result.GetLineByPosition(2).Content, Is.EqualTo("line3"));
    }

    [Test]
    public void GetTextRange_TwoLast()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetTextRange(1, 2);

        Assert.That(result.Lines, Has.Length.EqualTo(2));
        Assert.That(result.GetLineByPosition(1).Content, Is.EqualTo("line2"));
        Assert.That(result.GetLineByPosition(2).Content, Is.EqualTo("line3"));
    }

    [Test]
    public void GetTextRange_First()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetTextRange(0, 0);

        Assert.That(result.Lines, Has.Length.EqualTo(1));
        Assert.That(result.GetLineByPosition(0).Content, Is.EqualTo("line1"));
    }

    [Test]
    public void GetTextRange_Second()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetTextRange(1, 1);

        Assert.That(result.Lines, Has.Length.EqualTo(1));
        Assert.That(result.GetLineByPosition(1).Content, Is.EqualTo("line2"));
    }

    [Test]
    public void GetTextRange_Last()
    {
        var text = new Text("line1\nline2\nline3");

        var result = text.GetTextRange(2, 2);

        Assert.That(result.Lines, Has.Length.EqualTo(1));
        Assert.That(result.GetLineByPosition(2).Content, Is.EqualTo("line3"));
    }

    [Test]
    public void GetLinesRange()
    {
        var text = new Text("0\n1\n2\n3\n4\n5");

        var result = text.GetLinesRange(2, 3);

        Assert.That(result, Has.Length.EqualTo(3));
        Assert.That(result[0].Content, Is.EqualTo("2"));
        Assert.That(result[1].Content, Is.EqualTo("3"));
        Assert.That(result[2].Content, Is.EqualTo("4"));
    }
}
