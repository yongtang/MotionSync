using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class HandTrackingSender : MonoBehaviour
{
    private UdpClient client;
    private string remoteIP = "192.168.1.100"; // Change to your receiver’s IP
    private int remotePort = 9000;

    private XRHandSubsystem handSubsystem;

void Start()
{
    client = new UdpClient();

    // Get the XR Hand Subsystem
    handSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();

    if (handSubsystem == null || !handSubsystem.running)
    {
        Debug.LogError("XR Hand Subsystem not found or not running!");
        return;
    }

    Debug.Log("XR Hand Subsystem successfully initialized.");
}


    private void OnUpdatedHands(XRHandSubsystem subsystem, XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags, XRHandSubsystem.UpdateType updateType)
    {
        if (updateType != XRHandSubsystem.UpdateType.Dynamic)
            return;

        if (updateSuccessFlags.HasFlag(XRHandSubsystem.UpdateSuccessFlags.LeftHandJoints))
            SendHandData(subsystem.leftHand, "L");

        if (updateSuccessFlags.HasFlag(XRHandSubsystem.UpdateSuccessFlags.RightHandJoints))
            SendHandData(subsystem.rightHand, "R");
    }

    private void SendHandData(XRHand hand, string label)
    {
        if (!hand.isTracked)
            return;

        StringBuilder data = new StringBuilder($"{label}:{hand.rootPose.position.x},{hand.rootPose.position.y},{hand.rootPose.position.z}");

        foreach (XRHandJointID jointID in System.Enum.GetValues(typeof(XRHandJointID)))
        {
            XRHandJoint joint = hand.GetJoint(jointID);
            if (joint.TryGetPose(out Pose pose))
            {
                data.Append($"|{jointID}:{pose.position.x},{pose.position.y},{pose.position.z}");
            }
        }

        SendData(data.ToString());
    }

    private void SendData(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        client.Send(data, data.Length, remoteIP, remotePort);
    }

    void OnDestroy()
    {
        if (handSubsystem != null)
            handSubsystem.updatedHands -= OnUpdatedHands;

        client.Close();
    }
}

