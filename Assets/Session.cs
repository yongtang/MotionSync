using UnityEngine;
using TMPro;

public class Session : MonoBehaviour
{
    [Header("UI References")]
    public GameObject canvas;
    public TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = $"Go to\nexample.com/pair\n123-456";
        canvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
