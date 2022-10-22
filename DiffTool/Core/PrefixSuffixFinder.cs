namespace DiffTool.Core;

internal class PrefixSuffixFinder
{
    public int GetPrefixLinesCount(Text oldText, Text newText)
    {
        int prefixLinesCount = 0;
        int length = Math.Min(oldText.Lines.Count, newText.Lines.Count);
        for (int i = 0; i < length; i++)
        {
            if (oldText.Lines[i].Equals(newText.Lines[i]))
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
        for (int i = oldText.Lines.Count - 1, j = newText.Lines.Count - 1; i >= 0 && j >= 0; i--, j--)
        {
            if (oldText.Lines[i].Equals(newText.Lines[j]))
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
