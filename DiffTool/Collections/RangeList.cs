using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiffTool.Collections;

internal class RangeList<T> : IReadOnlyList<T>
{
	private readonly IReadOnlyList<T> _list;
	private readonly int _startIndex;
	private readonly int _count;

	public int Count => _count;

	public T this[int index] => _list[index + _startIndex];

	public RangeList(IReadOnlyList<T> list, int startIndex, int count)
	{
		_list = list;
		_startIndex = startIndex;
		_count = count;
	}

	public IEnumerator<T> GetEnumerator() => _list.Skip(_startIndex).Take(_count).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => _list.Skip(_startIndex).Take(_count).GetEnumerator();
}
