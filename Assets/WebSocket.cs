using UnityEngine;
using UnityEngine;
using LiveKit;
using LiveKit.Proto;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiveKit;

public class WebSocket : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
/*
public class LiveKitRenderer : MonoBehaviour
{
    [Header("LiveKit Settings")]
    public string livekitUrl = "wss://your-livekit-server";
    public string livekitToken = "your-access-token";

    [Header("Video Target")]
    public Renderer cylinderRenderer;

    private Room room;
    private List<VideoStream> activeStreams = new();

    void Start()
    {
Debug.Log("🚀 Start() running: beginning LiveKit connection");
        StartCoroutine(ConnectToLiveKit());
    }

    IEnumerator ConnectToLiveKit()
    {

            Debug.Log("✅ Connected to LiveKit: ");
        room = new Room();

        // Subscribe to track events
        room.TrackSubscribed += OnTrackSubscribed;
        room.TrackUnsubscribed += OnTrackUnsubscribed;

        var connectInstruction = room.Connect(livekitUrl, livekitToken, new LiveKit.RoomOptions());
        yield return connectInstruction;

        // Check if connected (this avoids accessing ErrorMessage)
        if (room.IsConnected)
        {
            Debug.Log("✅ Connected to LiveKit: " + room.Name);
        }
        else
        {
            Debug.LogError("❌ Failed to connect to LiveKit.");
        }
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
}

*/
