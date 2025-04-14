using UnityEngine;
using System.Collections;
using LiveKit;
using LiveKit.Proto;

public class LiveKitRenderer : MonoBehaviour
{
    public static LiveKitRenderer Instance;

    private Room room;
    private Coroutine roomCoroutine;

    private VideoStream stream;
    private Coroutine streamCoroutine;

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

    void OnDestroy()
    {
        if (streamCoroutine != null)
        {
            StopCoroutine(streamCoroutine);
        }
        if (stream != null)
        {
            stream.Stop();
            stream.Dispose();
        }
        if (roomCoroutine != null)
        {
            StopCoroutine(roomCoroutine);
        }
        if (room != null)
        {
            room.Disconnect();
        }
    }

    void TrackSubscribed(IRemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    {
        Debug.Log("TrackSubscribed: " + track?.Sid + "|" + participant.Identity + "|" + participant.Name);
        if ((participant.Identity == "bot") && (track is RemoteVideoTrack videoTrack))
        {
            Debug.Log("Subscribed to video track: " + videoTrack.Sid);

            // Clean up old stream
            if (stream != null)
            {
                StopCoroutine(streamCoroutine);
                stream.Dispose();
            }
            // Start new stream
            stream = new VideoStream(videoTrack);
            stream.TextureReceived += tex =>
            {
                Debug.Log("Frame received — applying to cylinder");
                Renderer renderer = transform.Find("Cylinder")?.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.mainTexture = tex;
                    renderer.material.mainTextureScale = new Vector2(-1, 1); // Flip horizontally
                }
            };
            stream.Start();
            streamCoroutine = StartCoroutine(stream.Update());
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
        if (roomCoroutine != null)
        {
            StopCoroutine(roomCoroutine);
        }
        roomCoroutine = StartCoroutine(ConnectToRoom(serve, token));
    }
}
