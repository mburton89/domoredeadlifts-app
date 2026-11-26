
using UnityEngine;
using UnityEngine.UI;

public class MouseTracker : MonoBehaviour
{
    [Header("References")]
    public RectTransform trackingPoint;
    public TrajectoryDrawer trajectoryDrawer;

    [Header("Tracking")]
    public float minimumDistance = 2f;

    private RectTransform drawingArea;
    private Vector2 previousPoint;
    private bool isTracking;

    void Start()
    {
        drawingArea = GetComponent<RectTransform>();

        if (trackingPoint != null)
        {
            trackingPoint.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryGetLocalMousePosition(out Vector2 point))
            {
                isTracking = true;
                previousPoint = point;

                if (trackingPoint != null)
                {
                    trackingPoint.gameObject.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButton(0) && isTracking)
        {
            if (TryGetLocalMousePosition(out Vector2 point))
            {
                MoveTrackingPoint(point);

                if (Vector2.Distance(previousPoint, point) >= minimumDistance)
                {
                    trajectoryDrawer.CreateTrackingSegment(previousPoint, point);
                    previousPoint = point;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isTracking = false;
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

    void MoveTrackingPoint(Vector2 point)
    {
        if (trackingPoint != null)
        {
            trackingPoint.anchoredPosition = point;
        }
    }
}
