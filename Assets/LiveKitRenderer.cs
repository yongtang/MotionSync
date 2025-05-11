using UnityEngine;

public class LiveKitRenderer : MonoBehaviour
{
    public Renderer targetRenderer;

    private Texture2D liveVideoTexture;
    private Color[] pixels;
    private int width = 256;
    private int height = 256;

    void Start()
    {
        liveVideoTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        liveVideoTexture.wrapMode = TextureWrapMode.Clamp;
        liveVideoTexture.filterMode = FilterMode.Bilinear;

        pixels = new Color[width * height];
        targetRenderer.material.SetTexture("_MainTex", liveVideoTexture);
    }

    void Update()
    {
        float t = Time.time;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float r = Mathf.Sin((x + t * 30f) * 0.05f) * 0.5f + 0.5f;
                float g = Mathf.Sin((y + t * 45f) * 0.07f) * 0.5f + 0.5f;
                float b = Mathf.Sin((x + y + t * 15f) * 0.02f) * 0.5f + 0.5f;
                pixels[y * width + x] = new Color(r, g, b);
            }
        }

        liveVideoTexture.SetPixels(pixels);
        liveVideoTexture.Apply();
    }
}
