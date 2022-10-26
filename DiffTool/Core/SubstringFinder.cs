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
        InitSearchMatrix();
        FillSearchMatrix(oldText, newText);
        var result = FindResult(oldText, newText);

        return result;
    }

    private IReadOnlyCollection<SubstringResult> FindResult(string oldText, string newText)
    {
        var result = new LinkedList<SubstringResult>();
        int count = 0;
        int i = oldText.Length;
        int j = newText.Length;
        while (i > 0 && j > 0)
        {
            if (oldText[i - 1] == newText[j - 1])
            {
                count++;
                i--;
                j--;
            }
            else
            {
                if (count > 0)
                {
                    result.AddFirst(new SubstringResult(i, j, count));
                    count = 0;
                }
                if (_searchMatrix[i, j] == _searchMatrix[i - 1, j])
                {
                    i--;
                }
                else
                {
                    j--;
                }
            }
        }

        if (count > 0)
        {
            result.AddFirst(new SubstringResult(i, j, count));
        }

        return result;
    }

    private void FillSearchMatrix(string oldText, string newText)
    {
        for (int i = 1; i <= oldText.Length; i++)
        {
            for (int j = 1; j <= newText.Length; j++)
            {
                if (oldText[i - 1] == newText[j - 1])
                {
                    _searchMatrix[i, j] = _searchMatrix[i - 1, j - 1] + 1;
                }
                else
                {
                    _searchMatrix[i, j] = Math.Max(_searchMatrix[i - 1, j], _searchMatrix[i, j - 1]);
                }
            }
        }
    }

    private void InitSearchMatrix()
    {
        for (int i = 0; i < _searchMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _searchMatrix.GetLength(1); j++)
            {
                _searchMatrix[i, j] = 0;
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
