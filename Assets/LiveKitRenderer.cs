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
        Debug.Log("TrackSubscribed: " + track?.Sid + "|" + participant.Identity + "|" + participant.Name);
        if ((participant.Identity == "bot") && (track is RemoteVideoTrack videoTrack))
        {
            Debug.Log("Subscribed to video track: " + videoTrack.Sid);
        }
    }

    void TrackUnsubscribed(IRemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    {
        Debug.Log("TrackUnsubscribed: " + track?.Sid + "|" + participant.Identity + "|" + participant.Name);
    }

    IEnumerator ConnectToRoom(string serve, string token)
    {
        Debug.Log("ConnectToRoom: " + serve + "|" + token);
        var connect = room.Connect(serve, token, new LiveKit.RoomOptions());
        yield return connect;
        if (!connect.IsError)
        {
            Debug.Log("Connected: " + room.Name);
        }
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
