using System;
using System.Linq;
using DiffTool.Ancillary;
using NUnit.Framework;

namespace DiffTool.Test.Ancillary;

internal class SimpleRearrangerTest
{
    [Test]
    public void Rearrange_5_2()
    {
        var result = new SimpleRearranger().GetRearranges(5, 2).ToList();

        Assert.That(result, Has.Count.EqualTo(10));

        Assert.That(result[0], Is.EqualTo(new[] { 0, 1 }));
        Assert.That(result[1], Is.EqualTo(new[] { 0, 2 }));
        Assert.That(result[2], Is.EqualTo(new[] { 0, 3 }));
        Assert.That(result[3], Is.EqualTo(new[] { 0, 4 }));
        Assert.That(result[4], Is.EqualTo(new[] { 1, 2 }));
        Assert.That(result[5], Is.EqualTo(new[] { 1, 3 }));
        Assert.That(result[6], Is.EqualTo(new[] { 1, 4 }));
        Assert.That(result[7], Is.EqualTo(new[] { 2, 3 }));
        Assert.That(result[8], Is.EqualTo(new[] { 2, 4 }));
        Assert.That(result[9], Is.EqualTo(new[] { 3, 4 }));
    }

    [Test]
    public void Rearrange_5_3()
    {
        var result = new SimpleRearranger().GetRearranges(5, 3).ToList();

        Assert.That(result, Has.Count.EqualTo(10));

        Assert.That(result[0], Is.EqualTo(new[] { 0, 1, 2 }));
        Assert.That(result[1], Is.EqualTo(new[] { 0, 1, 3 }));
        Assert.That(result[2], Is.EqualTo(new[] { 0, 1, 4 }));
        Assert.That(result[3], Is.EqualTo(new[] { 0, 2, 3 }));
        Assert.That(result[4], Is.EqualTo(new[] { 0, 2, 4 }));
        Assert.That(result[5], Is.EqualTo(new[] { 0, 3, 4 }));
        Assert.That(result[6], Is.EqualTo(new[] { 1, 2, 3 }));
        Assert.That(result[7], Is.EqualTo(new[] { 1, 2, 4 }));
        Assert.That(result[8], Is.EqualTo(new[] { 1, 3, 4 }));
        Assert.That(result[9], Is.EqualTo(new[] { 2, 3, 4 }));
    }

    [Test]
    public void Rearrange_5_4()
    {
        var result = new SimpleRearranger().GetRearranges(5, 4).ToList();

        Assert.That(result, Has.Count.EqualTo(5));

        Assert.That(result[0], Is.EqualTo(new[] { 0, 1, 2, 3 }));
        Assert.That(result[1], Is.EqualTo(new[] { 0, 1, 2, 4 }));
        Assert.That(result[2], Is.EqualTo(new[] { 0, 1, 3, 4 }));
        Assert.That(result[3], Is.EqualTo(new[] { 0, 2, 3, 4 }));
        Assert.That(result[4], Is.EqualTo(new[] { 1, 2, 3, 4 }));
    }

    [Test]
    public void ItemsLessThanRearrangeLength()
    {
        try
        {
            new SimpleRearranger().GetRearranges(3, 10).ToList();
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("itemsCount must be greater or equal than rearrangeLength."));
        }
    }
}
