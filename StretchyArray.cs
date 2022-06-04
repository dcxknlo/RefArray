public class StretchyArray<T>
{
	public T[] _items;
	static readonly T[] _emptyArray = new T[0];
	public StretchyArray(int elements)
	{
		_items = _emptyArray;
	}
}

namespace System.Collections.Generic
{
	using System;
	using System.Diagnostics.Contracts;

	// Implements a variable-size List that uses an array of objects to store the
	// elements. A List has a capacity, which is the allocated length
	// of the internal array. As elements are added to a List, the capacity
	// of the List is automatically increased as required by reallocating the
	// internal array.
	[Serializable]
	public class List2<T> 
	{
		private const int _defaultCapacity = 4;

		public T[] _items;
		[ContractPublicPropertyName("Count")]
		private int _size;
		private int _version;
		[NonSerialized]

		static readonly T[] _emptyArray = new T[0];

		// Constructs a List. The list is initially empty and has a capacity
		// of zero. Upon adding the first element to the list the capacity is
		// increased to 16, and then increased in multiples of two as required.
		public List2()
		{
			_items = _emptyArray;
		}

		// Constructs a List with a given initial capacity. The list is
		// initially empty, but will have room for the given number of elements
		// before any reallocations are required.
		// 
		public List2(int capacity)
		{
			if (capacity < 0) throw new ArgumentOutOfRangeException("Capacity is less than 0.");
			if (capacity == 0)
				_items = _emptyArray;
			else
				_items = new T[capacity];
		}

		
		// Gets and sets the capacity of this list.  The capacity is the size of
		// the internal array used to hold items.  When set, the internal 
		// array of the list is reallocated to the given capacity.
		// 
		public int Capacity
		{
			get
			{
				return _items.Length;
			}
			set
			{
				if (value < _size)
				{
					throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_SmallCapacity");
				}

				if (value != _items.Length)
				{
					if (value > 0)
					{
						T[] newItems = new T[value];
						if (_size > 0)
						{
							Array.Copy(_items, 0, newItems, 0, _size);
						}
						_items = newItems;
					}
					else
					{
						_items = _emptyArray;
					}
				}
			}
		}

		// Read-only property describing how many elements are in the List.
		public int Count
		{
			get
			{
				Contract.Ensures(Contract.Result<int>() >= 0);
				return _size;
			}
		}

		// Sets or Gets the element at the given index.
		// 
		public T this[int index]
		{
			get
			{
				// Following trick can reduce the range check by one
				if ((uint)index >= (uint)_size)
				{
					throw new ArgumentOutOfRangeException("Index is equal to or greater than size of the List.");
				}
				Contract.EndContractBlock();
				return _items[index];
			}

			set
			{
				if ((uint)index >= (uint)_size)
				{
					throw new ArgumentOutOfRangeException("Index is equal to or greater than size of the List.");
				}
				Contract.EndContractBlock();
				_items[index] = value;
				_version++;
			}
		}


		// Adds the given object to the end of this list. The size of the list is
		// increased by one. If required, the capacity of the list is doubled
		// before adding the new element.
		//
		public void Add(T item)
		{
			if (_size == _items.Length) EnsureCapacity(_size + 1);
			_items[_size++] = item;
			_version++;
		}


		// Clears the contents of List.
		public void Clear()
		{
			if (_size > 0)
			{
				Array.Clear(_items, 0, _size); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
				_size = 0;
			}
			_version++;
		}

		// Copies this List into array, which must be of a 
		// compatible array type.  
		//
		public void CopyTo(T[] array)
		{
			CopyTo(array, 0);
		}

		// Copies a section of this list to the given array at the given index.
		// 
		// The method uses the Array.Copy method to copy the elements.
		// 
		public void CopyTo(int index, T[] array, int arrayIndex, int count)
		{
			if (_size - index < count)
			{
				throw new ArgumentException("Invalid Offset Length.");
			}
			Contract.EndContractBlock();

			// Delegate rest of error checking to Array.Copy.
			Array.Copy(_items, index, array, arrayIndex, count);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			// Delegate rest of error checking to Array.Copy.
			Array.Copy(_items, 0, array, arrayIndex, _size);
		}

		// Ensures that the capacity of this list is at least the given minimum
		// value. If the currect capacity of the list is less than min, the
		// capacity is increased to twice the current capacity or to min,
		// whichever is larger.
		private void EnsureCapacity(int min)
		{
			if (_items.Length < min)
			{
				int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
				// Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
				// Note that this check works even when _items.Length overflowed thanks to the (uint) cast
				if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;
				if (newCapacity < min) newCapacity = min;
				Capacity = newCapacity;
			}
		}

		// Removes the element at the given index. The size of the list is
		// decreased by one.
		// 
		public void RemoveAt(int index)
		{
			if ((uint)index >= (uint)_size)
			{
				throw new ArgumentOutOfRangeException("Index is equal to or greater than size of the List.");
			}
			Contract.EndContractBlock();
			_size--;
			if (index < _size)
			{
				Array.Copy(_items, index + 1, _items, index, _size - index);
			}
			_items[_size] = default(T);
			_version++;
		}

		// Sets the capacity of this list to the size of the list. This method can
		// be used to minimize a list's memory overhead once it is known that no
		// new elements will be added to the list. To completely clear a list and
		// release all memory referenced by the list, execute the following
		// statements:
		// 
		// list.Clear();
		// list.TrimExcess();
		// 
		public void TrimExcess()
		{
			int threshold = (int)(((double)_items.Length) * 0.9);
			if (_size < threshold)
			{
				Capacity = _size;
			}
		}

		[Serializable]
		public struct Enumerator {
			private List2<T> list;
			private int index;
			private int version;
			private T current;

			internal Enumerator(List2<T> list)
			{
				this.list = list;
				index = 0;
				version = list._version;
				current = default(T);
			}

			public bool MoveNext()
			{

				List2<T> localList = list;

				if (version == localList._version && ((uint)index < (uint)localList._size))
				{
					current = localList._items[index];
					index++;
					return true;
				}
				return MoveNextRare();
			}

			private bool MoveNextRare()
			{
				if (version != list._version)
				{
					throw new InvalidOperationException("Invalid Operation Enumeration Failed Version.");
				}

				index = list._size + 1;
				current = default(T);
				return false;
			}

			public T Current
			{
				get
				{
					return current;
				}
			}
		}
	}
}