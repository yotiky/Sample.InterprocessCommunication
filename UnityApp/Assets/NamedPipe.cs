using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NamedPipe : MonoBehaviour
{
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    // Start is called before the first frame update
    async void Start()
    {
        await Task.Run(async () =>
        {
            using (var stream = new NamedPipeClientStream("NamedPipe"))
            {
                // ConnectAsync : The method or operation is not implemented.
                stream.Connect();

                using (var reader = new StreamReader(stream))
                {
                    while (true)
                    {
                        var str = await reader.ReadLineAsync();
                        if (str == null)
                        {
                            break;
                        }
                        Debug.Log("Data :" + str);
                        Thread.Sleep(100);

                        if(tokenSource.Token.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                }
            }
            Debug.Log("End of sample.");
        }, 
        tokenSource.Token);

    }

    void OnDestroy()
    {
        tokenSource.Cancel();
        Debug.Log("task cancelled.");
    }
}
