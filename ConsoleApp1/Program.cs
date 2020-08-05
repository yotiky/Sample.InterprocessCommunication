using System;
using System.IO.MemoryMappedFiles;

namespace ConsoleApp1
{
    class Program
    {
		static void Main(string[] args)
		{
			var sharedMemory = MemoryMappedFile.CreateNew("SharedMemory", 1024);

			while (true)
			{
				var str = Console.ReadLine();
				var data = str.ToCharArray();
				using (var accessor = sharedMemory.CreateViewAccessor())
				{
					accessor.Write(0, data.Length);
					accessor.WriteArray(sizeof(int), data, 0, data.Length);
				}
			}
		}
	}
}
