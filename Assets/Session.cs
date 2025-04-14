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

    [System.Serializable]
    public class PairRequest
    {
        public string grant_type;
        public string pair_code;
        public string identity;
    }

    [System.Serializable]
    public class TokenResponse
    {
        public string access_token;
        public string refresh_token;
        public string socket_address;
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

            while (!authenticated)
            {
                // Create request
                UnityWebRequest request = new UnityWebRequest(info.token_address, "POST");
                byte[] body = new System.Text.UTF8Encoding().GetBytes(JsonUtility.ToJson(new PairRequest
                {
                    grant_type = "pair_code",
                    pair_code = info.pair_code,
                    identity = $"device-{SystemInfo.deviceUniqueIdentifier}"
                }));
                Debug.Log("Request: " + System.Text.Encoding.UTF8.GetString(body));
                request.uploadHandler = new UploadHandlerRaw(body);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Token received: " + request.downloadHandler.text);
                    TokenResponse response = JsonUtility.FromJson<TokenResponse>(request.downloadHandler.text);

                    Debug.Log("Token received: " + $"access: {response.access_token}, refresh: {response.refresh_token}, address: {response.socket_address}");

                    authenticated = true;
                    canvas.SetActive(false);
                    LiveKitRenderer.Instance.Connect(response.socket_address, response.access_token);
                    break;
                }
                else
                {
                    Debug.Log("Token not ready yet or error: " + request.responseCode);
                }

                yield return new WaitForSeconds(1f);
            }
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
