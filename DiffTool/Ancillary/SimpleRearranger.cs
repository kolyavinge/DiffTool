namespace DiffTool.Ancillary;

internal class SimpleRearranger
{
    public void GetRearranges(int itemsCount, int rearrangeLength, Action<int[]> resultFunc)
    {
        if (itemsCount < rearrangeLength) throw new ArgumentException("itemsCount must be greater or equal than rearrangeLength.");
        var currentRearrange = new int[rearrangeLength];
        for (int i = 0; i < rearrangeLength; i++) currentRearrange[i] = i;
        resultFunc(currentRearrange);
        while (true)
        {
            while (currentRearrange[rearrangeLength - 1] < itemsCount - 1)
            {
                currentRearrange[rearrangeLength - 1]++;
                resultFunc(currentRearrange);
            }
            int i = rearrangeLength - 2;
            for (; i >= 0; i--)
            {
                if ((currentRearrange[i] + 1) + (rearrangeLength - i - 1) < itemsCount) // can be increased
                {
                    currentRearrange[i]++;
                    for (int j = i + 1; j < rearrangeLength; j++) currentRearrange[j] = currentRearrange[j - 1] + 1;
                    resultFunc(currentRearrange);
                    break;
                }
            }
            if (i == -1) break;
        }
    }
}
