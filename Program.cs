class Program
{
	static void Main(string[] args)
	{
		StretchyArray<int> sa = new StretchyArray<int>(5);
		List2<int> list = new List2<int>();
		list.Add(10);
		ref int item = ref list._items[0];

		Console.WriteLine("Hello World!");
	}
}
