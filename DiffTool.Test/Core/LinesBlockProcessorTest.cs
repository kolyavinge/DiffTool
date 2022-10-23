using System;
using System.Collections.Generic;
using System.Linq;
using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class LinesBlockProcessorTest
{
    private List<LineDiff> _result;
    private LinesBlockProcessor _processor;

    [SetUp]
    public void Setup()
    {
        _processor = new LinesBlockProcessor();
    }

    [Test]
    public void EmptyLineBlocks_Error()
    {
        try
        {
            ProcessLineBlocks("", "", new LinesBlock[0]);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("lineBlocks cannot be empty."));
        }
    }

    [Test]
    public void ProcessLineBlocks_AllAdded()
    {
        ProcessLineBlocks("same\nsame", "add\nadd\nsame\nsame", new LinesBlock[]
        {
            new(0, 2, 2)
        });

        Assert.That(_result, Has.Count.EqualTo(4));
        Assert.That(_result[0], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 0)));
        Assert.That(_result[1], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 1)));
        Assert.That(_result[2], Is.EqualTo(new LineDiff(DiffKind.Same, 0, 2)));
        Assert.That(_result[3], Is.EqualTo(new LineDiff(DiffKind.Same, 1, 3)));
    }

    [Test]
    public void ProcessLineBlocks_AllRemoved()
    {
        ProcessLineBlocks("removed\nremoved\nsame\nsame", "same\nsame", new LinesBlock[]
        {
            new(2, 0, 2)
        });

        Assert.That(_result, Has.Count.EqualTo(4));
        Assert.That(_result[0], Is.EqualTo(new LineDiff(DiffKind.Remove, 0, -1)));
        Assert.That(_result[1], Is.EqualTo(new LineDiff(DiffKind.Remove, 1, -1)));
        Assert.That(_result[2], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 0)));
        Assert.That(_result[3], Is.EqualTo(new LineDiff(DiffKind.Same, 3, 1)));
    }

    [Test]
    public void ProcessLineBlocks_AllChanged()
    {
        ProcessLineBlocks("old\nold\nsame\nsame", "new\nnew\nsame\nsame", new LinesBlock[]
        {
            new(2, 2, 2)
        });

        Assert.That(_result, Has.Count.EqualTo(4));
        Assert.That(_result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(_result[2], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 2)));
        Assert.That(_result[3], Is.EqualTo(new LineDiff(DiffKind.Same, 3, 3)));
    }

    [Test]
    public void ProcessLineBlocks_Changed_1()
    {
        ProcessLineBlocks("old\nold\nsame\nsame", "new\nnew\nnew\nnew\nsame\nsame", new LinesBlock[]
        {
            new(2, 4, 2)
        });

        Assert.That(_result, Has.Count.EqualTo(6));
        Assert.That(_result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(_result[2], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 2)));
        Assert.That(_result[3], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 3)));
        Assert.That(_result[4], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 4)));
        Assert.That(_result[5], Is.EqualTo(new LineDiff(DiffKind.Same, 3, 5)));
    }

    [Test]
    public void ProcessLineBlocks_Changed_2()
    {
        ProcessLineBlocks("old\nold\nold\nold\nsame\nsame", "new\nnew\nsame\nsame", new LinesBlock[]
        {
            new(4, 2, 2)
        });

        Assert.That(_result, Has.Count.EqualTo(6));
        Assert.That(_result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_result[1], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 1)));
        Assert.That(_result[2], Is.EqualTo(new LineDiff(DiffKind.Remove, 2, -1)));
        Assert.That(_result[3], Is.EqualTo(new LineDiff(DiffKind.Remove, 3, -1)));
        Assert.That(_result[4], Is.EqualTo(new LineDiff(DiffKind.Same, 4, 2)));
        Assert.That(_result[5], Is.EqualTo(new LineDiff(DiffKind.Same, 5, 3)));
    }

    [Test]
    public void ProcessLineBlocks_Changed_3()
    {
        ProcessLineBlocks("old\nsame\nold", "new\nadd\nsame\nnew\nadd", new LinesBlock[]
        {
            new(1, 2, 1)
        });

        Assert.That(_result, Has.Count.EqualTo(5));
        Assert.That(_result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_result[1], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 1)));
        Assert.That(_result[2], Is.EqualTo(new LineDiff(DiffKind.Same, 1, 2)));
        Assert.That(_result[3], Is.EqualTo(new LineDiff(DiffKind.Change, 2, 3)));
        Assert.That(_result[4], Is.EqualTo(new LineDiff(DiffKind.Add, -1, 4)));
    }

    [Test]
    public void ProcessLineBlocks_Changed_4()
    {
        ProcessLineBlocks("old\nremove\nsame\nold\nremove", "new\nsame\nnew", new LinesBlock[]
        {
            new(2, 1, 1)
        });

        Assert.That(_result, Has.Count.EqualTo(5));
        Assert.That(_result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 0, 0)));
        Assert.That(_result[1], Is.EqualTo(new LineDiff(DiffKind.Remove, 1, -1)));
        Assert.That(_result[2], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 1)));
        Assert.That(_result[3], Is.EqualTo(new LineDiff(DiffKind.Change, 3, 2)));
        Assert.That(_result[4], Is.EqualTo(new LineDiff(DiffKind.Remove, 4, -1)));
    }

    [Test]
    public void ProcessLineBlocks_WithPrefix()
    {
        ProcessLineBlocks(
            new Text(new Line[] { new("old", 1), new("same", 2), new("old", 3) }),
            new Text(new Line[] { new("new", 2), new("same", 3), new("new", 4) }),
            new LinesBlock[]
            {
                new(2, 3, 1)
            });

        Assert.That(_result, Has.Count.EqualTo(3));
        Assert.That(_result[0], Is.EqualTo(new LineDiff(DiffKind.Change, 1, 2)));
        Assert.That(_result[1], Is.EqualTo(new LineDiff(DiffKind.Same, 2, 3)));
        Assert.That(_result[2], Is.EqualTo(new LineDiff(DiffKind.Change, 3, 4)));
    }

    private void ProcessLineBlocks(string oldText, string newText, IEnumerable<LinesBlock> lineBlocks)
    {
        _result = _processor.ProcessLineBlocks(new Text(oldText), new Text(newText), lineBlocks).ToList();
    }

    private void ProcessLineBlocks(Text oldText, Text newText, IEnumerable<LinesBlock> lineBlocks)
    {
        _result = _processor.ProcessLineBlocks(oldText, newText, lineBlocks).ToList();
    }
}
