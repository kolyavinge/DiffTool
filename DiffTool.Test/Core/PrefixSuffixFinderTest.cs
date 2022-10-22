using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class PrefixSuffixFinderTest
{
    private PrefixSuffixFinder _finder;

    [SetUp]
    public void Setup()
    {
        _finder = new PrefixSuffixFinder();
    }

    [Test]
    public void GetPrefixLinesCount_Empty()
    {
        var result = GetPrefixLinesCount("", "");

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void GetPrefixLinesCount_LeftEmpty()
    {
        var result = GetPrefixLinesCount("", "line");

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetPrefixLinesCount_RightEmpty()
    {
        var result = GetPrefixLinesCount("line", "");

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetPrefixLinesCount_Three()
    {
        var result = GetPrefixLinesCount("line1\nline2\nline3", "line1\nline2\nline3");

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void GetPrefixLinesCount_ThreeAndTwo()
    {
        var result = GetPrefixLinesCount("line1\nline2\nline3", "line1\nline2");

        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void GetPrefixLinesCount_TwoAndThree()
    {
        var result = GetPrefixLinesCount("line1\nline2", "line1\nline2\nline3");

        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void GetSuffixLinesCount_Empty()
    {
        var result = GetSuffixLinesCount("", "");

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void GetSuffixLinesCount_LeftEmpty()
    {
        var result = GetSuffixLinesCount("", "line");

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetSuffixLinesCount_RightEmpty()
    {
        var result = GetSuffixLinesCount("line", "");

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void GetSuffixLinesCount_Three()
    {
        var result = GetSuffixLinesCount("line1\nline2\nline3", "line1\nline2\nline3");

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void GetSuffixLinesCount_ThreeAndTwo()
    {
        var result = GetSuffixLinesCount("line1\nline2\nline3", "line2\nline3");

        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void GetSuffixLinesCount_TwoAndThree()
    {
        var result = GetSuffixLinesCount("line2\nline3", "line1\nline2\nline3");

        Assert.That(result, Is.EqualTo(2));
    }

    private int GetPrefixLinesCount(string oldText, string newText)
    {
        return _finder.GetPrefixLinesCount(new Text(oldText), new Text(newText));
    }

    private int GetSuffixLinesCount(string oldText, string newText)
    {
        return _finder.GetSuffixLinesCount(new Text(oldText), new Text(newText));
    }
}
