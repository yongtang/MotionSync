using UnityEngine;
using System.Collections;

public class WebSocket : MonoBehaviour
{
    public static WebSocket Instance;

    private Coroutine coroutine;

    private NativeWebSocket.WebSocket socket;

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void Connect(string serve, string token)
    {
        Debug.Log("Connect");
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }
        if (coroutine != null)
        {   
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine(ConnectToSocket(serve, token));
    }

    IEnumerator ConnectToSocket(string serve, string token)
    {
        Debug.Log("ConnectToChannel: " + serve + "|" + token);

        socket = new NativeWebSocket.WebSocket($"{serve}/nats");

        socket.OnOpen += () =>
        {
            Debug.Log("WebSocket connected!");
        };

        socket.OnError += (e) =>
        {
            Debug.LogError("WebSocket error: " + e);

            if (e.Contains("401") || e.ToLower().Contains("unauthorized"))
            {
                Debug.LogWarning("Unauthenticated via error.");
            }
        };

        socket.OnClose += (e) =>
        {
            Debug.LogWarning("WebSocket closed.");
        };

        var connect = socket.Connect();

        while (!connect.IsCompleted)
        {
            yield return null;
        }

        if (connect.Exception != null)
        {
            Debug.LogError("Connection exception: " + connect.Exception.Message);
        }
    }

}
