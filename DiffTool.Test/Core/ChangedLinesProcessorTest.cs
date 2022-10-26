using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class ChangedLinesProcessorTest
{
    private ChangedLinesProcessor _processor;

    [SetUp]
    public void Setup()
    {
        _processor = new ChangedLinesProcessor();
    }

    [Test]
    public void GetResult_FirstNew()
    {
        var result = _processor.GetResult(
            0, 3, new Text("change1\nchange2\nchange3"),
            0, 4, new Text("new\nchange1\nchange2\nchange3"));

        Assert.That(result, Has.Count.EqualTo(4));

        Assert.That(result[0], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 0)));
        Assert.That(result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 1)));
        Assert.That(result[2], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 2)));
        Assert.That(result[3], Is.EqualTo(new LineDiff(DiffKind.Change, 2, 3)));
    }

    [Test]
    public void GetResult_MiddleNew()
    {
        var result = _processor.GetResult(
            0, 3, new Text("change1\nchange2\nchange3"),
            0, 4, new Text("change1\nchange2\nnew\nchange3"));

        Assert.That(result, Has.Count.EqualTo(4));

        Assert.That(result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(result[2], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 2)));
        Assert.That(result[3], Is.EqualTo(new LineDiff(DiffKind.Change, 2, 3)));
    }

    [Test]
    public void GetResult_LastNew()
    {
        var result = _processor.GetResult(
            0, 3, new Text("change1\nchange2\nchange3"),
            0, 4, new Text("change1\nchange2\nchange3\nnew"));

        Assert.That(result, Has.Count.EqualTo(4));

        Assert.That(result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(result[2], Is.EqualTo(new LineDiff(DiffKind.Change, 2, 2)));
        Assert.That(result[3], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 3)));
    }

    [Test]
    public void GetResult_FirstRemove()
    {
        var result = _processor.GetResult(
            0, 4, new Text("old\nchange1\nchange2\nchange3"),
            0, 3, new Text("change1\nchange2\nchange3"));

        Assert.That(result, Has.Count.EqualTo(4));

        Assert.That(result[0], Is.EqualTo(new LineDiff(DiffKind.Remove, 0, -1)));
        Assert.That(result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 0)));
        Assert.That(result[2], Is.EqualTo(new LineDiff(DiffKind.Change, 2, 1)));
        Assert.That(result[3], Is.EqualTo(new LineDiff(DiffKind.Change, 3, 2)));
    }

    [Test]
    public void GetResult_MiddleRemove()
    {
        var result = _processor.GetResult(
            0, 4, new Text("change1\nchange2\nold\nchange3"),
            0, 3, new Text("change1\nchange2\nchange3"));

        Assert.That(result, Has.Count.EqualTo(4));

        Assert.That(result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(result[2], Is.EqualTo(new LineDiff(DiffKind.Remove, 2, -1)));
        Assert.That(result[3], Is.EqualTo(new LineDiff(DiffKind.Change, 3, 2)));
    }

    [Test]
    public void GetResult_LastRemove()
    {
        var result = _processor.GetResult(
            0, 4, new Text("change1\nchange2\nchange3\nold"),
            0, 3, new Text("change1\nchange2\nchange3"));

        Assert.That(result, Has.Count.EqualTo(4));

        Assert.That(result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(result[2], Is.EqualTo(new LineDiff(DiffKind.Change, 2, 2)));
        Assert.That(result[3], Is.EqualTo(new LineDiff(DiffKind.Remove, 3, -1)));
    }
}
