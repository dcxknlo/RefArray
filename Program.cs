using Collections;

class Program
{
	static void Main(string[] args)
	{
		RefList<int> stretchyArray = new RefList<int>(5);
		stretchyArray.Add(1);
		ref int test = ref stretchyArray[0];

		Console.WriteLine(test);
	}
}
