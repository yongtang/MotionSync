using UnityEngine;
using UnityEngine;
using LiveKit;
using LiveKit.Proto;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiveKit;

public class LiveKitRenderer : MonoBehaviour
{
    [Header("LiveKit Settings")]
    public string livekitUrl = "wss://service.zenimotion.com";
    public string livekitToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3NDQ2MDgwMDMsImlzcyI6ImRldmtleSIsIm5hbWUiOiJib3QyIiwibmJmIjoxNzQ0NTIxNjAzLCJzdWIiOiJib3QyIiwidmlkZW8iOnsicm9vbSI6InRlc3Rfcm9vbSIsInJvb21Kb2luIjp0cnVlfX0.V_Jhd_PSezlBD27ACqU7xkBuTpE-Rv7_upyPXQ9vikk";

    [Header("Video Target")]
    public Renderer cylinderRenderer;

    private Room room;
    private List<VideoStream> activeStreams = new();

    void Start()
    {
        StartCoroutine(ConnectToLiveKit());
    }

IEnumerator ConnectToLiveKit()
{
    Debug.Log("🚀 Start() running: beginning LiveKit connection" + livekitUrl + livekitToken);

            room = new Room();
            room.TrackSubscribed += OnTrackSubscribed;
            room.TrackUnsubscribed += OnTrackUnsubscribed;

            var options = new LiveKit.RoomOptions();
            var connect = room.Connect(livekitUrl, livekitToken, options);
            yield return connect;
            if (!connect.IsError)
            {
                Debug.Log("Connected to ----- " + room.Name);
            }
/*
    room = new Room();

    room.TrackSubscribed += OnTrackSubscribed;
    room.TrackUnsubscribed += OnTrackUnsubscribed;

    //var options = new LiveKit.RoomOptions(); // resolve ambiguity here

    var connectTask = room.Connect(livekitUrl, livekitToken);

    if (connectTask.Exception == null)
    {
        Debug.Log("✅ Connected to LiveKit: " + room.Name);
    }
    else
    {
        Debug.LogError("❌ Failed to connect to LiveKit: " + connectTask.Exception);
    }
*/
}


    void OnTrackSubscribed(IRemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    {
        if (track is RemoteVideoTrack videoTrack)
        {
            Debug.Log("📹 Subscribed to video track: " + videoTrack.Sid);

            var stream = new VideoStream(videoTrack);

            stream.TextureReceived += tex =>
            {
                Debug.Log("🎥 Frame received — applying to cylinder");
                if (cylinderRenderer != null)
                {
                    cylinderRenderer.material.mainTexture = tex;
                    cylinderRenderer.material.mainTextureScale = new Vector2(-1, 1); // Flip for immersive interior
                }
            };

            stream.Start();
            StartCoroutine(stream.Update());
            activeStreams.Add(stream);
        }
    }

    void OnTrackUnsubscribed(IRemoteTrack track, RemoteTrackPublication publication, RemoteParticipant participant)
    {
        Debug.Log("🛑 Track unsubscribed: " + track?.Sid);
        // Cleanup logic can go here if you manage per-track state
    }

    void OnDestroy()
    {
        foreach (var stream in activeStreams)
        {
            stream.Stop();
            stream.Dispose();
        }

        if (room != null)
        {
            room.Disconnect();
        }
    }
void OnConnect(ConnectCallback e) {
Debug.Log("Error: " + e.Error);
}
}

