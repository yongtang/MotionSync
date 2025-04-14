using UnityEngine;
using System.Collections;
using LiveKit;
using LiveKit.Proto;

public class LiveKitRenderer : MonoBehaviour
{
    public static LiveKitRenderer Instance;

    private Coroutine coroutine;

    private Room room;

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        room = new Room();
        room.TrackSubscribed += TrackSubscribed;
        room.TrackUnsubscribed += TrackUnsubscribed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TrackSubscribed(IRemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    {
        Debug.Log("TrackSubscribed: " + track?.Sid);
    }

    void TrackUnsubscribed(IRemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    {
        Debug.Log("TrackUnsubscribed: " + track?.Sid);
    }

    IEnumerator ConnectToRoom(string serve, string token)
    {
        Debug.Log("ConnectToRoom");
        yield return null;
    }

    public void Connect(string serve, string token)
    {
        Debug.Log("Connect: " + room.ConnectionState + "|" + room.IsConnected);
        if (room.IsConnected)
        {
            return;
        }
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(ConnectToRoom(serve, token));
    }
}
