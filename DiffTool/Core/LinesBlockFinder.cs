using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

internal class LinesBlockFinder
{
    public IReadOnlyCollection<LinesBlock> FindLongestBlocks(Text oldText, Text newText)
    {
        var blocks = FindAllBlocks(oldText, newText).ToList();
        blocks.Sort(new LinesBlock.ByEqualLinesCountComparer());
        GetCorrectBlocks(blocks);
        blocks.Sort(new LinesBlock.ByOldLinePositionComparer());
        blocks = GetOverlayingBlocks(blocks).ToList();

        return blocks;
    }

    private IEnumerable<LinesBlock> GetOverlayingBlocks(List<LinesBlock> blocks)
    {
        if (blocks.Count == 0) yield break;
        yield return blocks[0];
        int prevIndex = 0;
        for (int currentIndex = 1; currentIndex < blocks.Count; currentIndex++)
        {
            var prev = blocks[prevIndex];
            var current = blocks[currentIndex];
            if (prev.OldPosition + prev.LinesCount <= current.OldPosition)
            {
                yield return current;
                prevIndex = currentIndex;
            }
        }
    }

    private void GetCorrectBlocks(List<LinesBlock> blocks)
    {
        for (int prevIndex = 0; prevIndex < blocks.Count - 1; prevIndex++)
        {
            var prev = blocks[prevIndex];
            for (int currentIndex = prevIndex + 1; currentIndex < blocks.Count;)
            {
                var current = blocks[currentIndex];
                var currentCorrect =
                    prev.OldPosition + prev.LinesCount <= current.OldPosition &&
                    prev.NewPosition + prev.LinesCount <= current.NewPosition ||
                    current.OldPosition + current.LinesCount <= prev.OldPosition &&
                    current.NewPosition + current.LinesCount <= prev.NewPosition;
                if (!currentCorrect)
                {
                    blocks.RemoveAt(currentIndex);
                }
                else
                {
                    currentIndex++;
                }
            }
        }
    }

    private IEnumerable<LinesBlock> FindAllBlocks(Text oldText, Text newText)
    {
        var newLinesCache = new LinesCache(newText);
        foreach (var oldLine in oldText.Lines)
        {
            var newLinePositions = newLinesCache.GetLinePositions(oldLine);
            if (newLinePositions == null) continue;
            var resultBlocks = new List<LinesBlock>();
            foreach (var newLinePosition in newLinePositions)
            {
                int equalLinesCount = 1;
                var nextOldPosition = oldLine.Position + 1;
                var nextNewPosition = newLinePosition + 1;
                while (nextOldPosition <= oldText.EndPosition && nextNewPosition <= newText.EndPosition)
                {
                    var nextOldLine = oldText.GetLineByPosition(nextOldPosition);
                    var nextNewLine = newText.GetLineByPosition(nextNewPosition);
                    if (nextOldLine.Equals(nextNewLine))
                    {
                        equalLinesCount++;
                        nextOldPosition++;
                        nextNewPosition++;
                    }
                    else
                    {
                        break;
                    }
                }
                resultBlocks.Add(new(oldLine.Position, newLinePosition, equalLinesCount));
            }
            var maxEqualLinesCount = resultBlocks.Max(x => x.LinesCount);
            foreach (var resultBlock in resultBlocks.Where(x => x.LinesCount == maxEqualLinesCount))
            {
                yield return resultBlock;
            }
        }
    }
}
