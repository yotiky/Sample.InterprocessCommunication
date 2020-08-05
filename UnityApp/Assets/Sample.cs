using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Sample : MonoBehaviour
{
	
    // Start is called before the first frame update
    async void Start()
    {
		var sharedMemory = MemoryMappedFile.OpenExisting("SharedMemory");

		await Task.Run(() =>
		{
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
						Debug.Log("Data :" + str);
					}
				}

				Thread.Sleep(100);
			}
		});
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
