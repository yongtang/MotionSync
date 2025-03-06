using UnityEngine;
using TMPro;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class HandTrackingDisplay : MonoBehaviour
{
    public TextMeshProUGUI debugText; // UI Text in Meta Quest

    private XRHandSubsystem handSubsystem;

    void Start()
    {
        // Get the Hand Tracking Subsystem
        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if (loader != null)
        {
            handSubsystem = loader.GetLoadedSubsystem<XRHandSubsystem>();
        }
    }

    void Update()
    {
        if (handSubsystem != null)
        {
            XRHand leftHand = handSubsystem.leftHand;
            XRHand rightHand = handSubsystem.rightHand;

            // Build text message
            string message = "Hand Tracking Active\n";

            if (leftHand.isTracked)
            {
                message += $"Left Hand Position: {leftHand.rootPose.position}\n";
            }
            else
            {
                message += "Left Hand Not Tracked\n";
            }

            if (rightHand.isTracked)
            {
                message += $"Right Hand Position: {rightHand.rootPose.position}\n";
            }
            else
            {
                message += "Right Hand Not Tracked\n";
            }

            // Update UI Text
            debugText.text = message;
        }
        else
        {
            debugText.text = "Hand Tracking Subsystem Not Found!";
        }
    }
}
