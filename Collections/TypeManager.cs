namespace Collections
{
	public struct TypeId
	{
		public int id;
	}
	
	public class TypeManager
	{
		private Dictionary<Type, TypeId> _typeMap;
		public TypeManager()
		{
			_typeMap = new Dictionary<Type, TypeId>();
		}

		public void RegisterType(Type type)
		{
			if (!_typeMap.ContainsKey(type))
			{
				_typeMap.Add(
					type,
					new TypeId
					{
						id = _typeMap.Count
					});
			}
		}

		public Type GetType(TypeId typeId)
		{
			return _typeMap.ElementAt(typeId.id).Key;
		}

		public TypeId GetTypeId(Type type)
		{
			return _typeMap[type];
		}
	}
}