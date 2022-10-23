using System.Collections.Generic;
using DiffTool.Core;
using DiffTool.Visualization;
using NUnit.Framework;

namespace DiffTool.Test.Visualization;

internal class SingleTextVisualizerTest
{
    private SingleTextVisualizer _visualizer;

    [SetUp]
    public void Setup()
    {
        _visualizer = new SingleTextVisualizer();
    }

    [Test]
    public void GetResult_Add()
    {
        var oldText = new Text("same");
        var newText = new Text("new\nsame\nnew");
        var linesDiff = new List<LineDiff>
        {
            new(DiffKind.Add, -1, 0),
            new(DiffKind.Same, 0, 1),
            new(DiffKind.Add, -1, 2)
        };

        var result = _visualizer.GetResult(oldText, newText, linesDiff);

        Assert.That(result.Text, Is.EqualTo("new\r\nsame\r\nnew"));
        Assert.That(result.LinesDiff, Has.Count.EqualTo(3));
        Assert.That(result.LinesDiff[0], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Add)));
        Assert.That(result.LinesDiff[1], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Same)));
        Assert.That(result.LinesDiff[2], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Add)));
    }

    [Test]
    public void GetResult_Remove()
    {
        var oldText = new Text("old\nsame\nold");
        var newText = new Text("same");
        var linesDiff = new List<LineDiff>
        {
            new(DiffKind.Remove, 0, -1),
            new(DiffKind.Same, 1, 0),
            new(DiffKind.Remove, 2, -1)
        };

        var result = _visualizer.GetResult(oldText, newText, linesDiff);

        Assert.That(result.Text, Is.EqualTo("old\r\nsame\r\nold"));
        Assert.That(result.LinesDiff, Has.Count.EqualTo(3));
        Assert.That(result.LinesDiff[0], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Remove)));
        Assert.That(result.LinesDiff[1], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Same)));
        Assert.That(result.LinesDiff[2], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Remove)));
    }

    [Test]
    public void GetResult_Change()
    {
        var oldText = new Text("old1\nold2\nsame\nold3\nold4");
        var newText = new Text("new1\nnew2\nsame\nnew3\nnew4");
        var linesDiff = new List<LineDiff>
        {
            new(DiffKind.Change, 0, 0),
            new(DiffKind.Change, 1, 1),
            new(DiffKind.Same, 2, 2),
            new(DiffKind.Change, 3, 3),
            new(DiffKind.Change, 4, 4),
        };

        var result = _visualizer.GetResult(oldText, newText, linesDiff);

        Assert.That(result.Text, Is.EqualTo("old1\r\nold2\r\nnew1\r\nnew2\r\nsame\r\nold3\r\nold4\r\nnew3\r\nnew4"));
        Assert.That(result.LinesDiff, Has.Count.EqualTo(9));
        Assert.That(result.LinesDiff[0], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Change, TextKind.Old)));
        Assert.That(result.LinesDiff[1], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Change, TextKind.Old)));
        Assert.That(result.LinesDiff[2], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Change, TextKind.New)));
        Assert.That(result.LinesDiff[3], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Change, TextKind.New)));
        Assert.That(result.LinesDiff[4], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Same)));
        Assert.That(result.LinesDiff[5], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Change, TextKind.Old)));
        Assert.That(result.LinesDiff[6], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Change, TextKind.Old)));
        Assert.That(result.LinesDiff[7], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Change, TextKind.New)));
        Assert.That(result.LinesDiff[8], Is.EqualTo(new SingleTextVisualizerLineDiff(DiffKind.Change, TextKind.New)));
    }
}
