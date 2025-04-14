using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class Session : MonoBehaviour
{
    [Header("UI References")]
    public GameObject canvas;
    public TextMeshProUGUI text;

    [Header("Configurable Settings")]
    [SerializeField] private string endpoint = "https://service.zenimotion.com/api/pair";

    private bool authenticated = false
;
    [System.Serializable]
    public class PairInfo
    {
        public string pair_code;
        public string pair_address;
        public string token_address;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!authenticated)
        {
            StartCoroutine(Authentication());
        }
    }

    IEnumerator Authentication()
    {
        UnityWebRequest req = UnityWebRequest.PostWwwForm(endpoint, "");
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + req.downloadHandler.text);

            PairInfo info = JsonUtility.FromJson<PairInfo>(req.downloadHandler.text);

            text.text = $"Go to\n{info.pair_address}\n{info.pair_code}";
            canvas.SetActive(true);
        }
        else
        {
            Debug.LogError("Failed to get pairing code: " + req.error);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
