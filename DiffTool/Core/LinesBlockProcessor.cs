using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Core;

internal class LinesBlockProcessor
{
    public IReadOnlyCollection<LineDiff> ProcessLineBlocks(Text oldText, Text newText, IEnumerable<LinesBlock> lineBlocks)
    {
        if (!lineBlocks.Any()) throw new ArgumentException($"{nameof(lineBlocks)} cannot be empty.");

        var result = new List<LineDiff>();

        var startBlock = new LinesBlock(oldText.Lines.Min(x => x.Position), newText.Lines.Min(x => x.Position), 0);
        var endBlock = new LinesBlock(oldText.Lines.Max(x => x.Position) + 1, newText.Lines.Max(x => x.Position) + 1, 0);

        var lastBlock = startBlock;
        foreach (var block in lineBlocks.Union(new[] { endBlock }))
        {
            ProcessLineBlock(lastBlock, block, result);
            result.AddRange(AddSameLines(block));
            lastBlock = block;
        }

        return result;
    }

    private void ProcessLineBlock(LinesBlock lastBlock, LinesBlock block, List<LineDiff> result)
    {
        if (block.OldPosition - (lastBlock.OldPosition + lastBlock.LinesCount) ==
            block.NewPosition - (lastBlock.NewPosition + lastBlock.LinesCount))
        {
            result.AddRange(AllChanged(lastBlock, block));
        }
        else if (lastBlock.OldPosition + lastBlock.LinesCount == block.OldPosition)
        {
            result.AddRange(AllAdded(lastBlock, block));
        }
        else if (lastBlock.NewPosition + lastBlock.LinesCount == block.NewPosition)
        {
            result.AddRange(AllRemoved(lastBlock, block));
        }
        else
        {
            result.AddRange(Changed(lastBlock, block));
        }
    }

    private IEnumerable<LineDiff> AllChanged(LinesBlock lastBlock, LinesBlock block)
    {
        for (int i = lastBlock.OldPosition + lastBlock.LinesCount, j = lastBlock.NewPosition + lastBlock.LinesCount; i < block.OldPosition; i++, j++)
        {
            yield return new(DiffKind.Change, i, j);
        }
    }

    private IEnumerable<LineDiff> AllAdded(LinesBlock lastBlock, LinesBlock block)
    {
        for (int i = lastBlock.NewPosition + lastBlock.LinesCount; i < block.NewPosition; i++)
        {
            yield return new(DiffKind.Add, -1, i);
        }
    }

    private IEnumerable<LineDiff> AllRemoved(LinesBlock lastBlock, LinesBlock block)
    {
        for (int i = lastBlock.OldPosition + lastBlock.LinesCount; i < block.OldPosition; i++)
        {
            yield return new(DiffKind.Remove, i, -1);
        }
    }

    private IEnumerable<LineDiff> Changed(LinesBlock lastBlock, LinesBlock block)
    {
        int i = lastBlock.OldPosition + lastBlock.LinesCount;
        int j = lastBlock.NewPosition + lastBlock.LinesCount;

        for (; i < block.OldPosition && j < block.NewPosition; i++, j++)
        {
            yield return new(DiffKind.Change, i, j);
        }

        for (; j < block.NewPosition; j++)
        {
            yield return new(DiffKind.Add, -1, j);
        }

        for (; i < block.OldPosition; i++)
        {
            yield return new(DiffKind.Remove, i, -1);
        }
    }

    private IEnumerable<LineDiff> AddSameLines(LinesBlock block)
    {
        for (int i = 0; i < block.LinesCount; i++)
        {
            yield return new(DiffKind.Same, block.OldPosition + i, block.NewPosition + i);
        }
    }
}
