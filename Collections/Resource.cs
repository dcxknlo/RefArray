namespace Collections
{
	public class Resource<T>
	{
		private Dictionary<Handle<T>, T> _handleMap;
		private TypeManager _typeManager;
		private int _handleId;
		public Resource(TypeManager typeManager)
		{
			_typeManager = typeManager;
			_handleMap = new Dictionary<Handle<T>, T>();
		}

		public Handle<T> Add(T item)
		{
			var handle = HandleUtilities.CreateHandle<T>(_handleId++, _typeManager);	
			_handleMap.Add(handle, item);
			return handle;
		}

		public T Get(Handle<T> handle)
		{
			return _handleMap[handle];
		}

		public void Set(Handle<T> handle, T item)
		{
			_handleMap[handle] = item;
		}

		public void Remove(Handle<T> handle)
		{
			_handleMap.Remove(handle);
		}
	}
}