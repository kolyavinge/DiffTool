using System;
using System.Collections.Generic;
using System.IO;
using CodeHighlighter;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;
using DiffTool.Core;
using DiffTool.Visualization;

namespace Interactive;

internal class MainWindowModel
{
    public ICodeTextBoxModel LeftCodeTextBoxModel { get; set; }
    public ICodeTextBoxModel RightCodeTextBoxModel { get; set; }
    public ICodeTextBoxModel DiffCodeTextBoxModel { get; set; }

    public MainWindowModel()
    {
        LeftCodeTextBoxModel = CodeTextBoxModelFactory.MakeModel(new SqlCodeProvider());
        RightCodeTextBoxModel = CodeTextBoxModelFactory.MakeModel(new SqlCodeProvider());
        DiffCodeTextBoxModel = CodeTextBoxModelFactory.MakeModel(new SqlCodeProvider());
        var sqlText = File.ReadAllText(@"D:\Projects\DiffTool\Interactive\Examples\sql.txt");
        LeftCodeTextBoxModel.Text = sqlText;
        RightCodeTextBoxModel.Text = sqlText;
        LeftCodeTextBoxModel.TextEvents.TextChanged += OnTextChanged;
        RightCodeTextBoxModel.TextEvents.TextChanged += OnTextChanged;
        ProcessText();
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        ProcessText();
    }

    private void ProcessText()
    {
        DiffCodeTextBoxModel.IsReadOnly = false;
        var oldText = new Text(LeftCodeTextBoxModel.Text.ToString());
        var newText = new Text(RightCodeTextBoxModel.Text.ToString());
        var diffEngine = new DiffEngine();
        var diffResult = diffEngine.GetDiff(oldText, newText);
        var visualizator = new SingleTextVisualizer();
        var visualizationResult = visualizator.GetResult(oldText, newText, diffResult.LinesDiff);
        DiffCodeTextBoxModel.Text = visualizationResult.Text;
        SetLineColors(visualizationResult.LinesDiff);
        DiffCodeTextBoxModel.IsReadOnly = true;
    }

    private readonly CodeHighlighter.Common.Color _brushAdd = new(85, 222, 78);
    private readonly CodeHighlighter.Common.Color _brushRemove = new(219, 94, 94);
    private readonly CodeHighlighter.Common.Color _brushChangeOld = new(224, 155, 150);
    private readonly CodeHighlighter.Common.Color _brushChangeNew = new(128, 227, 123);

    private void SetLineColors(IReadOnlyList<SingleTextVisualizerLineDiff> linesDiff)
    {
        DiffCodeTextBoxModel.LinesDecoration.Clear();

        for (int i = 0; i < linesDiff.Count; i++)
        {
            var lineDiff = linesDiff[i];

            if (lineDiff.DiffKind == DiffKind.Add)
            {
                DiffCodeTextBoxModel.LinesDecoration[i] = new() { Background = _brushAdd };
            }
            else if (lineDiff.DiffKind == DiffKind.Remove)
            {
                DiffCodeTextBoxModel.LinesDecoration[i] = new() { Background = _brushRemove };
            }
            else if (lineDiff.DiffKind == DiffKind.Change && lineDiff.TextKind == TextKind.Old)
            {
                DiffCodeTextBoxModel.LinesDecoration[i] = new() { Background = _brushChangeOld };
            }
            else if (lineDiff.DiffKind == DiffKind.Change && lineDiff.TextKind == TextKind.New)
            {
                DiffCodeTextBoxModel.LinesDecoration[i] = new() { Background = _brushChangeNew };
            }
        }
    }
}
