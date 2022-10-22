using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class LineTest
{
    [Test]
    public void Equals()
    {
        var line1 = new Line("line", 10);
        var line2 = new Line("line", 10);

        Assert.IsTrue(line1.Equals(line2));
        Assert.IsTrue(line2.Equals(line1));

        Assert.IsTrue(line1.Equals((object)line2));
        Assert.IsTrue(line2.Equals((object)line1));
    }

    [Test]
    public void EqualsWithoutPosition()
    {
        var line1 = new Line("line", 10);
        var line2 = new Line("line", 1000);

        Assert.IsTrue(line1.Equals(line2));
        Assert.IsTrue(line2.Equals(line1));

        Assert.IsTrue(line1.Equals((object)line2));
        Assert.IsTrue(line2.Equals((object)line1));
    }

    [Test]
    public void NotEquals()
    {
        var line1 = new Line("line", 10);
        var line2 = new Line("line 123", 10);

        Assert.IsFalse(line1.Equals(line2));
        Assert.IsFalse(line2.Equals(line1));

        Assert.IsFalse(line1.Equals((object)line2));
        Assert.IsFalse(line2.Equals((object)line1));
    }

    [Test]
    public void NotEqualsNull()
    {
        var line1 = new Line("line", 10);

        Assert.IsFalse(line1.Equals(null));
    }

    [Test]
    public void GetHashCodeWithoutPosition()
    {
        var line1 = new Line("line", 10);
        var line2 = new Line("line", 1000);

        Assert.IsTrue(line1.GetHashCode() == line2.GetHashCode());
    }
}
