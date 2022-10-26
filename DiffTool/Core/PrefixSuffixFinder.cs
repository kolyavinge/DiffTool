namespace DiffTool.Core;

internal class PrefixSuffixFinder
{
    public int GetPrefixLinesCount(Text oldText, Text newText)
    {
        int prefixLinesCount = 0;
        for (int i = oldText.StartPosition, j = newText.StartPosition; i <= oldText.EndPosition && j <= newText.EndPosition; i++, j++)
        {
            if (oldText.GetLineByPosition(i).Equals(newText.GetLineByPosition(j)))
            {
                prefixLinesCount++;
            }
            else
            {
                break;
            }
        }

        return prefixLinesCount;
    }

    public int GetSuffixLinesCount(Text oldText, Text newText)
    {
        int suffixLinesCount = 0;
        for (int i = oldText.EndPosition, j = newText.EndPosition; i >= 0 && j >= 0; i--, j--)
        {
            if (oldText.GetLineByPosition(i).Equals(newText.GetLineByPosition(j)))
            {
                suffixLinesCount++;
            }
            else
            {
                break;
            }
        }

        return suffixLinesCount;
    }
}
