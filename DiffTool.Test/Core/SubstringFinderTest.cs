using System.Collections.Generic;
using System.Linq;
using DiffTool.Core;
using NUnit.Framework;

namespace DiffTool.Test.Core;

internal class SubstringFinderTest
{
    private string _oldText;
    private string _newText;
    private List<SubstringResult> _result;
    private SubstringFinder _finder;

    [SetUp]
    public void Setuo()
    {
        _finder = new SubstringFinder();
    }

    [Test]
    public void GetResult_Empty()
    {
        _oldText = "";
        _newText = "";

        GetResult();

        Assert.That(_result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetResult_Same()
    {
        _oldText = "line 123 ABC";
        _newText = "line 123 ABC";

        GetResult();

        Assert.That(_result, Has.Count.EqualTo(1));

        Assert.That(_result[0], Is.EqualTo(new SubstringResult(0, 0, 12)));
    }

    [Test]
    public void GetResult_Different()
    {
        _oldText = "ABC";
        _newText = "XYZ";

        GetResult();

        Assert.That(_result, Has.Count.EqualTo(0));
    }

    [Test]
    public void GetResult_1()
    {
        _oldText = "line 123 ABC";
        _newText = "line ABC";

        GetResult();

        Assert.That(_result, Has.Count.EqualTo(2));

        Assert.That(_result[0], Is.EqualTo(new SubstringResult(0, 0, 4)));
        Assert.That(_result[1], Is.EqualTo(new SubstringResult(8, 4, 4)));
    }

    [Test]
    public void GetResult_2()
    {
        _oldText = "AABABC";
        _newText = "ABC";

        GetResult();

        Assert.That(_result, Has.Count.EqualTo(1));

        Assert.That(_result[0], Is.EqualTo(new SubstringResult(3, 0, 3)));
    }

    private void GetResult()
    {
        _result = _finder.GetResult(_oldText, _newText).ToList();
    }
}
