using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NamedPipe : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await Task.Run(async () =>
        {
            while (true)
            {
                using (var stream = new NamedPipeClientStream("NamedPipe"))
                {
                    // ConnectAsync : The method or operation is not implemented.
                    stream.Connect();

                    using (var reader = new StreamReader(stream))
                    {
                        var str = await reader.ReadLineAsync();
                        if (str != null)
                        {
                            Debug.Log("Data :" + str);
                        }
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
