using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class MultiThreadingTest : MonoBehaviour
{
    public Thread testThread;
    int i;
    public int frame;
    public int prevFrame;
    public bool run = true;

    // Start is called before the first frame update
    void Start()
    {
        testThread = new Thread(new ThreadStart(testFunction));
        testThread.IsBackground = true;
        testThread.Start();
    }

    
    private void FixedUpdate()
    {
        frame += 1;
        Debug.Log(frame);
    }
    

    void testFunction()
    {
        while (run)
        {
            i += 1;
            Debug.Log(i + " " + frame.ToString());
        }
    }

    private void OnApplicationQuit()
    {
        run = false;
        testThread.Abort();
    }
}
