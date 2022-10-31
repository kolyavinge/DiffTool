using System.Collections.Generic;

namespace DiffTool.Core;

internal readonly struct SubstringResult
{
    public readonly int OldPosition;
    public readonly int NewPosition;
    public readonly int Count;

    public SubstringResult(int oldPosition, int newPosition, int count)
    {
        OldPosition = oldPosition;
        NewPosition = newPosition;
        Count = count;
    }

    public override string ToString()
    {
        return $"{OldPosition}:{NewPosition} {Count}";
    }
}

internal class SubstringFinder
{
    private int[,] _searchMatrix;

    public SubstringFinder()
    {
        _searchMatrix = new int[100, 100];
    }

    public IReadOnlyCollection<SubstringResult> GetResult(string oldText, string newText)
    {
        SetSearchMatrixSize(oldText, newText);
        FillSearchMatrix(oldText, newText);
        var result = FindResult(oldText, newText);

        return result;
    }

    private IReadOnlyCollection<SubstringResult> FindResult(string oldText, string newText)
    {
        var result = new List<SubstringResult>();
        int count = 0;
        int i = 0;
        int j = 0;
        while (i < oldText.Length && j < newText.Length)
        {
            if (oldText[i] == newText[j])
            {
                count++;
                i++;
                j++;
            }
            else
            {
                if (count > 0)
                {
                    result.Add(new(i - count, j - count, count));
                    count = 0;
                }
                if (_searchMatrix[i, j] == _searchMatrix[i + 1, j])
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }
        }

        if (count > 0)
        {
            result.Add(new(i - count, j - count, count));
        }

        return result;
    }

    private void FillSearchMatrix(string oldText, string newText)
    {
        for (int i = oldText.Length - 1; i >= 0; i--)
        {
            for (int j = newText.Length - 1; j >= 0; j--)
            {
                if (oldText[i] == newText[j])
                {
                    _searchMatrix[i, j] = _searchMatrix[i + 1, j + 1] + 1;
                }
                else
                {
                    _searchMatrix[i, j] = Math.Max(_searchMatrix[i + 1, j], _searchMatrix[i, j + 1]);
                }
            }
        }
    }

    private void SetSearchMatrixSize(string oldText, string newText)
    {
        if (_searchMatrix.GetLength(0) < oldText.Length + 1 || _searchMatrix.GetLength(1) < newText.Length + 1)
        {
            var newLength = 2 * Math.Max(oldText.Length, newText.Length);
            _searchMatrix = new int[newLength, newLength];
        }
    }
}
