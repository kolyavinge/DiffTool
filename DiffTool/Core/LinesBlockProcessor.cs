using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

internal class LinesBlockProcessor
{
    public IReadOnlyCollection<LineDiff> ProcessLineBlocks(Text oldText, Text newText, IEnumerable<LinesBlock> lineBlocks)
    {
        if (!lineBlocks.Any()) throw new ArgumentException($"{nameof(lineBlocks)} cannot be empty.");

        var result = new List<LineDiff>();

        var lastBlock = new LinesBlock();
        foreach (var block in lineBlocks.Union(new[] { new LinesBlock(oldText.Lines.Count, newText.Lines.Count, 0) }))
        {
            ProcessLineBlock(oldText, newText, lastBlock, block, result);
            lastBlock = block;
        }

        return result;
    }

    private void ProcessLineBlock(Text oldText, Text newText, LinesBlock lastBlock, LinesBlock block, List<LineDiff> result)
    {
        if (lastBlock.OldLinePosition + lastBlock.LinesCount == block.OldLinePosition)
        {
            result.AddRange(AllAdded(lastBlock, block));
        }
        else if (lastBlock.NewLinePosition + lastBlock.LinesCount == block.NewLinePosition)
        {
            result.AddRange(AllRemoved(lastBlock, block));
        }
        else if (
            block.OldLinePosition - (lastBlock.OldLinePosition + lastBlock.LinesCount) ==
            block.NewLinePosition - (lastBlock.NewLinePosition + lastBlock.LinesCount))
        {
            result.AddRange(AllChanged(lastBlock, block));
        }
        else
        {
            result.AddRange(Changed(lastBlock, block));
        }
        for (int i = 0; i < block.LinesCount; i++)
        {
            result.Add(new(DiffKind.Same, block.OldLinePosition + i, block.NewLinePosition + i));
        }
    }

    private IEnumerable<LineDiff> AllAdded(LinesBlock lastBlock, LinesBlock block)
    {
        for (int i = lastBlock.NewLinePosition + lastBlock.LinesCount; i < block.NewLinePosition; i++)
        {
            yield return new(DiffKind.Add, -1, i);
        }
    }

    private IEnumerable<LineDiff> AllRemoved(LinesBlock lastBlock, LinesBlock block)
    {
        for (int i = lastBlock.OldLinePosition + lastBlock.LinesCount; i < block.OldLinePosition; i++)
        {
            yield return new(DiffKind.Remove, i, -1);
        }
    }

    private IEnumerable<LineDiff> AllChanged(LinesBlock lastBlock, LinesBlock block)
    {
        for (int i = lastBlock.OldLinePosition + lastBlock.LinesCount, j = lastBlock.NewLinePosition + lastBlock.LinesCount; i < block.OldLinePosition; i++, j++)
        {
            yield return new(DiffKind.Change, i, j);
        }
    }

    private IEnumerable<LineDiff> Changed(LinesBlock lastBlock, LinesBlock block)
    {
        int i = lastBlock.OldLinePosition + lastBlock.LinesCount;
        int j = lastBlock.NewLinePosition + lastBlock.LinesCount; ;
        for (; i < block.OldLinePosition && j < block.NewLinePosition;
            i++, j++)
        {
            yield return new(DiffKind.Change, i, j);
        }

        for (; j < block.NewLinePosition; j++)
        {
            yield return new(DiffKind.Add, -1, j);
        }

        for (; i < block.OldLinePosition; i++)
        {
            yield return new(DiffKind.Remove, i, -1);
        }
    }
}
