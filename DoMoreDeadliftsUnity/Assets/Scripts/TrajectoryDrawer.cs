
using UnityEngine;
using UnityEngine.UI;

public class TrajectoryDrawer : MonoBehaviour
{
    [Header("Drawing")]
    public Color trajectoryColor = Color.cyan;
    public float lineThickness = 5f;
    public float minimumDistance = 2f;

    private RectTransform drawingArea;
    private Vector2 previousPoint;
    private bool isDrawing;

    void Start()
    {
        drawingArea = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryGetLocalMousePosition(out Vector2 point))
            {
                previousPoint = point;
                isDrawing = true;
            }
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            if (TryGetLocalMousePosition(out Vector2 point))
            {
                if (Vector2.Distance(previousPoint, point) >= minimumDistance)
                {
                    CreateTrackingSegment(previousPoint, point);
                    previousPoint = point;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }
    }

    bool TryGetLocalMousePosition(out Vector2 point)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            drawingArea,
            Input.mousePosition,
            null,
            out point
        );
    }

    public void CreateTrackingSegment(Vector2 start, Vector2 end)
    {
        GameObject segment = new GameObject("TrajectorySegment", typeof(Image));

        segment.transform.SetParent(transform, false);

        Image image = segment.GetComponent<Image>();
        image.color = trajectoryColor;
        image.raycastTarget = false;

        RectTransform rect = segment.GetComponent<RectTransform>();

        Vector2 direction = end - start;
        float length = direction.magnitude;

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.anchoredPosition = (start + end) / 2f;
        rect.sizeDelta = new Vector2(length, lineThickness);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void ClearTrajectory()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
