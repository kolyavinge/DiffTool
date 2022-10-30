using System;
using System.Collections.Generic;
using System.Linq;
using DiffTool.Ancillary;
using NUnit.Framework;

namespace DiffTool.Test.Ancillary;

internal class SimpleRearrangerTest
{
    private List<int[]> _result;

    [Test]
    public void Rearrange_5_2()
    {
        GetRearranges(5, 2);

        Assert.That(_result, Has.Count.EqualTo(10));

        Assert.That(_result[0], Is.EqualTo(new[] { 0, 1 }));
        Assert.That(_result[1], Is.EqualTo(new[] { 0, 2 }));
        Assert.That(_result[2], Is.EqualTo(new[] { 0, 3 }));
        Assert.That(_result[3], Is.EqualTo(new[] { 0, 4 }));
        Assert.That(_result[4], Is.EqualTo(new[] { 1, 2 }));
        Assert.That(_result[5], Is.EqualTo(new[] { 1, 3 }));
        Assert.That(_result[6], Is.EqualTo(new[] { 1, 4 }));
        Assert.That(_result[7], Is.EqualTo(new[] { 2, 3 }));
        Assert.That(_result[8], Is.EqualTo(new[] { 2, 4 }));
        Assert.That(_result[9], Is.EqualTo(new[] { 3, 4 }));
    }

    [Test]
    public void Rearrange_5_3()
    {
        GetRearranges(5, 3);

        Assert.That(_result, Has.Count.EqualTo(10));

        Assert.That(_result[0], Is.EqualTo(new[] { 0, 1, 2 }));
        Assert.That(_result[1], Is.EqualTo(new[] { 0, 1, 3 }));
        Assert.That(_result[2], Is.EqualTo(new[] { 0, 1, 4 }));
        Assert.That(_result[3], Is.EqualTo(new[] { 0, 2, 3 }));
        Assert.That(_result[4], Is.EqualTo(new[] { 0, 2, 4 }));
        Assert.That(_result[5], Is.EqualTo(new[] { 0, 3, 4 }));
        Assert.That(_result[6], Is.EqualTo(new[] { 1, 2, 3 }));
        Assert.That(_result[7], Is.EqualTo(new[] { 1, 2, 4 }));
        Assert.That(_result[8], Is.EqualTo(new[] { 1, 3, 4 }));
        Assert.That(_result[9], Is.EqualTo(new[] { 2, 3, 4 }));
    }

    [Test]
    public void Rearrange_5_4()
    {
        GetRearranges(5, 4);

        Assert.That(_result, Has.Count.EqualTo(5));

        Assert.That(_result[0], Is.EqualTo(new[] { 0, 1, 2, 3 }));
        Assert.That(_result[1], Is.EqualTo(new[] { 0, 1, 2, 4 }));
        Assert.That(_result[2], Is.EqualTo(new[] { 0, 1, 3, 4 }));
        Assert.That(_result[3], Is.EqualTo(new[] { 0, 2, 3, 4 }));
        Assert.That(_result[4], Is.EqualTo(new[] { 1, 2, 3, 4 }));
    }

    [Test]
    public void ItemsLessThanRearrangeLength()
    {
        try
        {
            GetRearranges(3, 10);
            Assert.Fail();
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("itemsCount must be greater or equal than rearrangeLength."));
        }
    }

    private void GetRearranges(int itemsCount, int rearrangeLength)
    {
        _result = new List<int[]>();
        new SimpleRearranger().GetRearranges(itemsCount, rearrangeLength, rearrange => _result.Add(rearrange.ToArray()));
    }
}
