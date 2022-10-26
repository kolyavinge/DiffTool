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

    private readonly SolidColorBrush _brushAdd = new SolidColorBrush(new() { R = 85, G = 222, B = 78, A = 255 });
    private readonly SolidColorBrush _brushRemove = new SolidColorBrush(new() { R = 219, G = 94, B = 94, A = 255 });
    private readonly SolidColorBrush _brushChangeOld = new SolidColorBrush(new() { R = 224, G = 155, B = 150, A = 255 });
    private readonly SolidColorBrush _brushChangeNew = new SolidColorBrush(new() { R = 128, G = 227, B = 123, A = 255 });

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
