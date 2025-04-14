using UnityEngine;
using LiveKit;

public class LiveKitRenderer : MonoBehaviour
{
    public static LiveKitRenderer Instance;

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

    public void Connect()
    {
        Debug.Log("Connect Room");
    }
}
