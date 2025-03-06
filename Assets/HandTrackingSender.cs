using UnityEngine;
using TMPro;

public class HandTrackingSender : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        debugText.text = "Start";
        
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = "Update";
        
    }
}
