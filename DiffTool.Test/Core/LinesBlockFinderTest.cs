using System.Collections.Generic;
using System.Linq;
using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class LinesBlockFinderTest
{
    private LinesBlockFinder _finder;
    private List<LinesBlock> _result;

    [SetUp]
    public void Setup()
    {
        _finder = new LinesBlockFinder();
        _result = new List<LinesBlock>();
    }

    [Test]
    public void Empty()
    {
        FindLongestBlocks("", "");

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result[0].OldLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].LinesCount, Is.EqualTo(1));
    }

    [Test]
    public void AddLines()
    {
        FindLongestBlocks("", "add\nsome\nlines");

        Assert.That(_result, Has.Count.EqualTo(0));
    }

    [Test]
    public void RemoveLines()
    {
        FindLongestBlocks("remove\nsome\nlines", "");

        Assert.That(_result, Has.Count.EqualTo(0));
    }

    [Test]
    public void OneLine()
    {
        FindLongestBlocks("line", "line");

        Assert.That(_result, Has.Count.EqualTo(1));
        Assert.That(_result[0].OldLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].LinesCount, Is.EqualTo(1));
    }

    [Test]
    public void OneBlockTwoLines()
    {
        FindLongestBlocks("line1\nline2", "line1\nline2");

        Assert.That(_result, Has.Count.EqualTo(1));

        Assert.That(_result[0].OldLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].LinesCount, Is.EqualTo(2));
    }

    [Test]
    public void OneBlockFourLines()
    {
        FindLongestBlocks("line1\nline2\nline3\nline4", "line1\nline2\nline3\nline4");

        Assert.That(_result, Has.Count.EqualTo(1));

        Assert.That(_result[0].OldLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].LinesCount, Is.EqualTo(4));
    }

    [Test]
    public void TwoBlocksTwoLinesWithAdded_1()
    {
        FindLongestBlocks("line1\nline2\nline3\nline4", "line1\nline2\nadded line\nline3\nline4");

        Assert.That(_result, Has.Count.EqualTo(2));

        Assert.That(_result[0].OldLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].LinesCount, Is.EqualTo(2));

        Assert.That(_result[1].OldLinePosition, Is.EqualTo(2));
        Assert.That(_result[1].NewLinePosition, Is.EqualTo(3));
        Assert.That(_result[1].LinesCount, Is.EqualTo(2));
    }

    [Test]
    public void TwoBlocksTwoLinesWithAdded_2()
    {
        FindLongestBlocks("line1\nline2\nline1\nline2", "line1\nline2\nadded line\nline1\nline2");

        Assert.That(_result, Has.Count.EqualTo(2));

        Assert.That(_result[0].OldLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].LinesCount, Is.EqualTo(2));

        Assert.That(_result[1].OldLinePosition, Is.EqualTo(2));
        Assert.That(_result[1].NewLinePosition, Is.EqualTo(3));
        Assert.That(_result[1].LinesCount, Is.EqualTo(2));
    }

    [Test]
    public void TwoInverseBlocksTwoLines()
    {
        FindLongestBlocks("line1\nline2\nline3\nline4", "line3\nline4\nline1\nline2");

        Assert.That(_result, Has.Count.EqualTo(1));

        Assert.That(_result[0].OldLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(2));
        Assert.That(_result[0].LinesCount, Is.EqualTo(2));
    }

    [Test]
    public void TwoInverseBlocksThreeAndTwoLines()
    {
        FindLongestBlocks("line1\nline2\nline3\nline4\nline5", "line3\nline4\nline5\nline1\nline2");

        Assert.That(_result, Has.Count.EqualTo(1));

        Assert.That(_result[0].OldLinePosition, Is.EqualTo(2));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].LinesCount, Is.EqualTo(3));
    }

    [Test]
    public void TwoBlocksOneAndTwoLines()
    {
        FindLongestBlocks("line2\nline1\nline2", "line1\nline1\nline2");

        Assert.That(_result, Has.Count.EqualTo(1));

        Assert.That(_result[0].OldLinePosition, Is.EqualTo(1));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(1));
        Assert.That(_result[0].LinesCount, Is.EqualTo(2));
    }

    [Test]
    public void ThreeBlocksTwoLinesWithAdded()
    {
        FindLongestBlocks("line1\nline2\nline3\nline4\nline5\nline6", "line1\nline2\nadded line\nline3\nline4\nadded line\nline5\nline6");

        Assert.That(_result, Has.Count.EqualTo(3));

        Assert.That(_result[0].OldLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].NewLinePosition, Is.EqualTo(0));
        Assert.That(_result[0].LinesCount, Is.EqualTo(2));

        Assert.That(_result[1].OldLinePosition, Is.EqualTo(2));
        Assert.That(_result[1].NewLinePosition, Is.EqualTo(3));
        Assert.That(_result[1].LinesCount, Is.EqualTo(2));

        Assert.That(_result[2].OldLinePosition, Is.EqualTo(4));
        Assert.That(_result[2].NewLinePosition, Is.EqualTo(6));
        Assert.That(_result[2].LinesCount, Is.EqualTo(2));
    }

    private void FindLongestBlocks(string oldText, string newText)
    {
        _result = _finder.FindLongestBlocks(new Text(oldText), new Text(newText)).ToList();
    }
}
