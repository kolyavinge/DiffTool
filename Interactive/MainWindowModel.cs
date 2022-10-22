using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;
using DiffTool.Core;
using DiffTool.Visualization;

namespace Interactive;

internal class MainWindowModel
{
    public CodeTextBoxModel LeftCodeTextBoxModel { get; set; }
    public CodeTextBoxModel RightCodeTextBoxModel { get; set; }
    public CodeTextBoxModel DiffCodeTextBoxModel { get; set; }

    public MainWindowModel()
    {
        LeftCodeTextBoxModel = new(new SqlCodeProvider());
        RightCodeTextBoxModel = new(new SqlCodeProvider());
        DiffCodeTextBoxModel = new(new SqlCodeProvider());
        var sqlText = File.ReadAllText(@"D:\Projects\DiffTool\Interactive\Examples\sql.txt");
        LeftCodeTextBoxModel.SetText(sqlText);
        RightCodeTextBoxModel.SetText(sqlText);
        LeftCodeTextBoxModel.TextChanged += OnTextChanged;
        RightCodeTextBoxModel.TextChanged += OnTextChanged;
        ProcessText();
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        ProcessText();
    }

    private void ProcessText()
    {
        DiffCodeTextBoxModel.IsReadOnly = false;
        var oldText = new DiffTool.Core.Text(LeftCodeTextBoxModel.Text.ToString());
        var newText = new DiffTool.Core.Text(RightCodeTextBoxModel.Text.ToString());
        var diffEngine = new DiffEngine();
        var diffResult = diffEngine.GetDiff(oldText, newText);
        var visualizator = new SingleTextVisualizer();
        var visualizationResult = visualizator.GetResult(oldText, newText, diffResult.LinesDiff);
        DiffCodeTextBoxModel.SetText(visualizationResult.Text);
        SetLineColors(visualizationResult.LinesDiff);
        DiffCodeTextBoxModel.IsReadOnly = true;
    }

    private void SetLineColors(IReadOnlyList<SingleTextVisualizerLineDiff> linesDiff)
    {
        for (int i = 0; i < DiffCodeTextBoxModel.Text.LinesCount; i++)
        {
            DiffCodeTextBoxModel.LinesDecoration[i] = null;
        }
        for (int i = 0; i < linesDiff.Count; i++)
        {
            var lineDiff = linesDiff[i];
            if (lineDiff.Kind == DiffKind.Add)
            {
                DiffCodeTextBoxModel.LinesDecoration[i] = new() { Background = Brushes.LightGreen };
            }
            else if (lineDiff.Kind == DiffKind.Remove)
            {
                DiffCodeTextBoxModel.LinesDecoration[i] = new() { Background = Brushes.LightCoral };
            }
            else if (lineDiff.Kind == DiffKind.Change)
            {
                DiffCodeTextBoxModel.LinesDecoration[i] = new() { Background = Brushes.LightGoldenrodYellow };
            }
        }
    }
}
