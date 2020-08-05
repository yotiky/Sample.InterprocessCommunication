using System;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {
		static void Main(string[] args)
		{
			var sharedMemory = MemoryMappedFile.OpenExisting("SharedMemory");

			var latestMsg = string.Empty;
			while (true)
			{
				using (var accessor = sharedMemory.CreateViewAccessor())
				{
					var size = accessor.ReadInt32(0);
					var data = new char[size];
					accessor.ReadArray<char>(sizeof(int), data, 0, data.Length);
					var str = new string(data);
					if (latestMsg != str)
					{
						latestMsg = str;
						Console.WriteLine("Data :" + str);
					}
				}

				Thread.Sleep(100);
			}
		}
	}
}
