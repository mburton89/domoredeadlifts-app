
using UnityEngine;
using UnityEngine.UI;

public class ColorDetector : MonoBehaviour
{
    public RenderTexture videoTexture;
    public RectTransform trackingPoint;
    public TrajectoryDrawer trajectoryDrawer;
    public RectTransform trackingContainer;

    [Header("Detection Settings")]
    public float detectionInterval = 0.1f;
    public int minimumPixelCount = 20;

    [Header("Green Thresholds")]
    public float minimumGreen = 0.4f;
    public float greenDominance = 1.5f;

    private Texture2D readableTexture;
    private float timer;

    private Vector2 previousPoint;
    private bool hasPreviousPoint;

    void Start()
    {
        readableTexture = new Texture2D(
            videoTexture.width,
            videoTexture.height,
            TextureFormat.RGB24,
            false
        );
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < detectionInterval)
            return;

        timer = 0f;

        DetectGreen();
    }

    void DetectGreen()
    {
        if (videoTexture == null)
            return;

        RenderTexture.active = videoTexture;

        readableTexture.ReadPixels(
            new Rect(0, 0, videoTexture.width, videoTexture.height),
            0, 0
        );

        readableTexture.Apply();

        RenderTexture.active = null;

        Color[] pixels = readableTexture.GetPixels();

        float totalX = 0f;
        float totalY = 0f;
        int detectedPixels = 0;

        for (int y = 0; y < videoTexture.height; y++)
        {
            for (int x = 0; x < videoTexture.width; x++)
            {
                Color pixel = pixels[y * videoTexture.width + x];

                if (IsGreen(pixel))
                {
                    totalX += x;
                    totalY += y;
                    detectedPixels++;
                }
            }
        }

        Debug.Log("Green pixels detected: " + detectedPixels);

        if (detectedPixels < minimumPixelCount)
            return;

        float centerX = totalX / detectedPixels;
        float centerY = totalY / detectedPixels;

        Vector2 normalizedPosition = new Vector2(
            centerX / videoTexture.width,
            centerY / videoTexture.height
        );

        Vector2 localPosition = new Vector2(
            (normalizedPosition.x - 0.5f) * trackingContainer.rect.width,
            (normalizedPosition.y - 0.5f) * trackingContainer.rect.height
        );

        trackingPoint.anchoredPosition = localPosition;

        if (hasPreviousPoint)
        {
            trajectoryDrawer.CreateTrackingSegment(
                previousPoint,
                localPosition
            );
        }

        previousPoint = localPosition;
        hasPreviousPoint = true;
    }

    bool IsGreen(Color pixel)
    {
        return pixel.g > 0.25f &&
               pixel.g > pixel.r * 1.15f &&
               pixel.g > pixel.b * 1.15f;
    }

    void OnDestroy()
    {
        if (readableTexture != null)
            Destroy(readableTexture);
    }
}
