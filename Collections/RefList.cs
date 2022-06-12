
namespace Collections
{
	/// <summary>
	/// Minimal implementation of a List that can access its elements by index.
	/// Use version to verify it has not been modified.
	/// </summary>
	/// <typeparam name="T"></typeparam>
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

		/// <summary>
		/// Allows indexed access to underlying array by ref.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
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

		/// <summary>
		/// Gets array of items as a readonly span.
		/// </summary>
		/// <returns></returns>
		public ReadOnlySpan<T> GetSpan()
		{
			_version++;
			return _items.AsSpan(0, Count);
		}

		/// <summary>
		/// Adds an item to the end of the list.
		/// </summary>
		/// <param name="item"></param>
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

		/// <summary>
		/// Removes the first occurrence of a specific object from the list.
		/// </summary>
		/// <param name="item"></param>
		public void Remove(T item)
		{
			int index = Array.IndexOf(_items, item);
			if (index >= 0)
			{
				RemoveAt((uint)index);
			}
		}

		/// <summary>
		/// Removes the element at the specified index of the list.
		/// </summary>
		/// <param name="index"></param>
		/// <exception cref="IndexOutOfRangeException"></exception>
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

		/// <summary>
		/// Clears the RefList
		/// </summary>
		public void Clear()
		{
			if (Count > 0)
			{
				Array.Clear(_items, 0, Count);
				Count = 0;
			}
			_version++;
		}
	}
}