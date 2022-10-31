namespace DiffTool.Core;

internal class PrefixFinder
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
}
