namespace Collections
{
	public struct Handle<T> : IEqualityComparer<Handle<T>>
	{
		public int TypeId;
		public int id;

		public bool Equals(Handle<T> x, Handle<T> y)
		{
			return x.id == y.id;
		}

		public int GetHashCode(Handle<T> obj)
		{
			return obj.id;
		}
	}

	public static class HandleUtilities
	{
		public static Handle<T> CreateHandle<T>(int id, TypeManager typeManager)
		{
			var typeId = typeManager.GetTypeId(typeof(T));
			return new Handle<T>
			{
				TypeId = typeId.id,
				id = id,
			};
		}
	}
}