using System.Collections.Generic;
using System.Linq;
using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

public class DiffEngineTest
{
    private DiffResult _diffResult;
    private List<LineDiff> _linesDiff;
    private DiffEngine _engine;

    [SetUp]
    public void Setup()
    {
        _engine = new DiffEngine();
    }

    [Test]
    public void EmptyTexts()
    {
        MakeDiff("", "");

        Assert.That(_linesDiff, Has.Count.EqualTo(0));
    }

    [Test]
    public void SameTexts()
    {
        MakeDiff("same1\nsame2", "same1\nsame2");

        Assert.That(_linesDiff, Has.Count.EqualTo(2));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Same, 1, 1)));
    }

    [Test]
    public void AddOneNewLine()
    {
        MakeDiff("same", "same\nadd");

        Assert.That(_linesDiff, Has.Count.EqualTo(2));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 1)));
    }

    [Test]
    public void RemoveOneOldLine()
    {
        MakeDiff("same\nremove", "same");

        Assert.That(_linesDiff, Has.Count.EqualTo(2));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Remove, 1, -1)));
    }

    [Test]
    public void OneEmptyLineChange()
    {
        MakeDiff("", "add");

        Assert.That(_linesDiff, Has.Count.EqualTo(1));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 0)));
    }

    [Test]
    public void OneLineWithSpacesChange()
    {
        MakeDiff("  ", "new");

        Assert.That(_linesDiff, Has.Count.EqualTo(1));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
    }

    [Test]
    public void OneEmptyLineChangeAndOneAdd()
    {
        MakeDiff("", "add1\nadd2");

        Assert.That(_linesDiff, Has.Count.EqualTo(2));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 1)));
    }

    [Test]
    public void OneLineChangeToEmpty()
    {
        MakeDiff("remove", "");

        Assert.That(_linesDiff, Has.Count.EqualTo(1));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Remove, 0, -1)));
    }

    [Test]
    public void OneLineChangeToSpaces()
    {
        MakeDiff("old", "  ");

        Assert.That(_linesDiff, Has.Count.EqualTo(1));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
    }

    [Test]
    public void OneLineChangeToEmptyAndOneRemove()
    {
        MakeDiff("remove1\nremove2", "");

        Assert.That(_linesDiff, Has.Count.EqualTo(2));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Remove, 0, -1)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Remove, 1, -1)));
    }

    [Test]
    public void ChangeOneLine()
    {
        MakeDiff("old", "new");

        Assert.That(_linesDiff, Has.Count.EqualTo(1));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
    }

    [Test]
    public void ChangeOneLineTwoSame()
    {
        MakeDiff("same\nold\nsame", "same\nnew\nsame");

        Assert.That(_linesDiff, Has.Count.EqualTo(3));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 2)));
    }

    [Test]
    public void AddSameLineAfterSuffix()
    {
        MakeDiff("1\n2\n3", "1\n1\n2\n3");

        Assert.That(_linesDiff, Has.Count.EqualTo(4));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Same, 1, 2)));
        Assert.That(_linesDiff[3], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 3)));
    }

    [Test]
    public void ChangesAroundZeroSymnNoSuffix()
    {
        MakeDiff("0", "+\n0\nx\n1\n0");

        Assert.That(_linesDiff, Has.Count.EqualTo(5));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 2)));
        Assert.That(_linesDiff[3], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 3)));
        Assert.That(_linesDiff[4], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 4)));
    }

    [Test]
    public void ChangesAroundZeroSymnNoSuffix2()
    {
        MakeDiff("-\n0", "+\n0\nx\n1\n0");

        Assert.That(_linesDiff, Has.Count.EqualTo(5));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Same, 1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 2)));
        Assert.That(_linesDiff[3], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 3)));
        Assert.That(_linesDiff[4], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 4)));
    }

    [Test]
    public void SameInMiddle()
    {
        MakeDiff("1\nsame\nold\n1", "2\nsame\nnew\n2");

        Assert.That(_linesDiff, Has.Count.EqualTo(4));
        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Same, 1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Change, 2, 2)));
        Assert.That(_linesDiff[3], Is.EqualTo(new LineDiff(DiffKind.Change, 3, 3)));
    }

    [Test]
    public void SameInMiddleChangeAndNew()
    {
        MakeDiff("old1\nsame\nold2", "new1\nadd\nsame\nnew2\nadd");

        Assert.That(_linesDiff, Has.Count.EqualTo(5));

        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Same, 1, 2)));
        Assert.That(_linesDiff[3], Is.EqualTo(new LineDiff(DiffKind.Change, 2, 3)));
        Assert.That(_linesDiff[4], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 4)));
    }

    [Test]
    public void SameInMiddleChangeAndRemove()
    {
        MakeDiff("old1\nremove\nsame\nold2\nremove", "new1\nsame\nnew2");

        Assert.That(_linesDiff, Has.Count.EqualTo(5));

        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Remove, 1, -1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 1)));
        Assert.That(_linesDiff[3], Is.EqualTo(new LineDiff(DiffKind.Change, 3, 2)));
        Assert.That(_linesDiff[4], Is.EqualTo(new LineDiff(DiffKind.Remove, 4, -1)));
    }

    [Test]
    public void SameOldAdd()
    {
        MakeDiff("same\nold", "same\nnew\nadd");

        Assert.That(_linesDiff, Has.Count.EqualTo(3));

        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 2)));
    }

    [Test]
    public void SameOldRemove()
    {
        MakeDiff("same\nold\nremove", "same\nnew");

        Assert.That(_linesDiff, Has.Count.EqualTo(3));

        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Remove, 2, -1)));
    }

    [Test]
    public void SameChangeSameChange()
    {
        MakeDiff("same\nold\nsame\nold", "same\nnew\nsame\nnew");

        Assert.That(_linesDiff, Has.Count.EqualTo(4));

        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 2)));
        Assert.That(_linesDiff[3], Is.EqualTo(new LineDiff(DiffKind.Change, 3, 3)));
    }

    [Test]
    public void DifficultSituation()
    {
        MakeDiff("0\n\n1\n\n1\n1\n1", "1\n\n1\n\n\n1\n1\n1");

        Assert.That(_linesDiff, Has.Count.EqualTo(8));

        Assert.That(_linesDiff[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_linesDiff[1], Is.EqualTo(new LineDiff(DiffKind.Same, 1, 1)));
        Assert.That(_linesDiff[2], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 2)));
        Assert.That(_linesDiff[3], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 3)));
        Assert.That(_linesDiff[4], Is.EqualTo(new LineDiff(DiffKind.Same, 3, 4)));
        Assert.That(_linesDiff[5], Is.EqualTo(new LineDiff(DiffKind.Same, 4, 5)));
        Assert.That(_linesDiff[6], Is.EqualTo(new LineDiff(DiffKind.Same, 5, 6)));
        Assert.That(_linesDiff[7], Is.EqualTo(new LineDiff(DiffKind.Same, 6, 7)));
    }

    private void MakeDiff(string oldText, string newText)
    {
        _diffResult = _engine.GetDiff(new(oldText), new(newText));
        _linesDiff = _diffResult.LinesDiff.ToList();
    }
}
