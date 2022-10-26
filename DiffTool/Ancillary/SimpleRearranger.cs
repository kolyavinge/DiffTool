using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Ancillary;

internal class SimpleRearranger
{
    public IEnumerable<int[]> GetRearranges(int itemsCount, int rearrangeLength)
    {
        if (itemsCount < rearrangeLength) throw new ArgumentException("itemsCount must be greater or equal than rearrangeLength.");
        var currentRearrange = new int[rearrangeLength];
        for (int i = 0; i < rearrangeLength; i++) currentRearrange[i] = i;
        yield return currentRearrange.ToArray();
        while (true)
        {
            while (currentRearrange[rearrangeLength - 1] < itemsCount - 1)
            {
                currentRearrange[rearrangeLength - 1]++;
                yield return currentRearrange.ToArray();
            }
            int i = rearrangeLength - 2;
            for (; i >= 0; i--)
            {
                if (CanBeIncreased(currentRearrange, i, rearrangeLength, itemsCount))
                {
                    currentRearrange[i]++;
                    for (int j = i + 1; j < rearrangeLength; j++) currentRearrange[j] = currentRearrange[j - 1] + 1;
                    yield return currentRearrange.ToArray();
                    break;
                }
            }
            if (i == -1) break;
        }
    }

    private bool CanBeIncreased(int[] currentRearrange, int i, int rearrangeLength, int itemsCount)
    {
        return (currentRearrange[i] + 1) + (rearrangeLength - i - 1) < itemsCount;
    }
}
