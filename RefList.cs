public class RefList<T>
{
	public int Capacity { get; private set; }
	public int Count { get; private set; }
	private T[] _items;
	private static readonly T[] _emptyArray = new T[0];
	private int _version;
	private const int _defaultCapacity = 4;

	public RefList()
	{
		_items = _emptyArray;
		Capacity = _defaultCapacity;
	}

	public RefList(uint capacity)
	{
		if (capacity == 0)
		{
			_items = _emptyArray;
			Capacity = _defaultCapacity;
		}
		else
		{
			_items = new T[capacity];
			Capacity = (int)capacity;
		}
	}

	public ref T this[int index]
	{
		get
		{
			if (index < 0 || index >= Count)
			{
				throw new IndexOutOfRangeException();
			}
			return ref _items[index];
		}
	}

	public T[] GetArray()
	{
		_version++;
		return _items;
	}

	public ReadOnlySpan<T> GetSpan()
	{
		_version++;
		return _items.AsSpan(0, Count);
	}

	public void Add(T item)
	{
		if (Count == _items.Length)
		{
			T[] newArray = new T[Count * 2];
			Array.Copy(_items, newArray, Count);
			_items = newArray;
			Capacity = newArray.Length;
		}
		_items[Count++] = item;
		_version++;
	}

	public void Remove(T item)
	{
		int index = Array.IndexOf(_items, item);
		if (index >= 0)
		{
			RemoveAt((uint)index);
		}
	}

	public void RemoveAt(uint index)
	{
		if (index >= Count)
		{
			throw new IndexOutOfRangeException();
		}

		Count--;
		if (index < Count)
		{
			Array.Copy(_items, index + 1, _items, index, Count - index);
		}
		_items[Count] = default(T);
		_version++;
	}

	// Clears the contents of List.
	public void Clear()
	{
		if (Count > 0)
		{
			Array.Clear(_items, 0, Count); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
			Count = 0;
		}
		_version++;
	}
}