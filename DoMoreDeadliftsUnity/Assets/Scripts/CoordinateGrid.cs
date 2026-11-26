
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoordinateGrid : MonoBehaviour
{
    [Header("Grid Values")]
    public int horizontalMin = -50;
    public int horizontalMax = 50;
    public int verticalMin = 0;
    public int verticalMax = 150;

    public int horizontalInterval = 10;
    public int verticalInterval = 10;

    [Header("Grid Appearance")]
    public float leftMargin = 100f;
    public float rightMargin = 40f;
    public float topMargin = 50f;
    public float bottomMargin = 100f;

    public Color gridColor = new Color(1f, 1f, 1f, 0.35f);
    public Color axisColor = new Color(1f, 1f, 1f, 0.8f);
    public Color labelColor = new Color(1f, 1f, 1f, 0.8f);

    public float lineThickness = 2f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float plotWidth = width - leftMargin - rightMargin;
        float plotHeight = height - topMargin - bottomMargin;

        // Vertical grid lines and X labels
        for (int x = horizontalMin; x <= horizontalMax;
             x += horizontalInterval)
        {
            float normalizedX =
                (float)(x - horizontalMin) /
                (horizontalMax - horizontalMin);

            float posX = leftMargin + normalizedX * plotWidth;

            Color currentColor = x == 0 ? axisColor : gridColor;
            float currentThickness = x == 0 ? lineThickness * 2f : lineThickness;

            CreateLine(
                new Vector2(posX, bottomMargin),
                new Vector2(posX, height - topMargin),
                currentColor,
                currentThickness
            );

            CreateLabel(
                x.ToString(),
                new Vector2(posX, bottomMargin - 30f),
                new Vector2(60f, 30f)
            );
        }

        // Horizontal grid lines and Y labels
        for (int y = verticalMin; y <= verticalMax;
             y += verticalInterval)
        {
            float normalizedY =
                (float)(y - verticalMin) /
                (verticalMax - verticalMin);

            float posY = bottomMargin + normalizedY * plotHeight;

            Color currentColor = y == 0 ? axisColor : gridColor;
            float currentThickness = y == 0 ? lineThickness * 2f : lineThickness;

            CreateLine(
                new Vector2(leftMargin, posY),
                new Vector2(width - rightMargin, posY),
                currentColor,
                currentThickness
            );

            CreateLabel(
                y.ToString(),
                new Vector2(leftMargin - 40f, posY),
                new Vector2(60f, 30f)
            );
        }
    }

    void CreateLine(Vector2 start, Vector2 end, Color color, float thickness)
    {
        GameObject lineObject = new GameObject("GridLine", typeof(Image));

        lineObject.transform.SetParent(transform, false);

        Image image = lineObject.GetComponent<Image>();
        image.color = color;

        RectTransform lineRect = lineObject.GetComponent<RectTransform>();

        Vector2 direction = end - start;
        float length = direction.magnitude;

        lineRect.anchorMin = new Vector2(0.5f, 0.5f);
        lineRect.anchorMax = new Vector2(0.5f, 0.5f);
        lineRect.pivot = new Vector2(0.5f, 0.5f);

        //lineRect.anchoredPosition = (start + end) / 2f;
        lineRect.anchoredPosition = (start + end) / 2f - new Vector2(rectTransform.rect.width / 2f, rectTransform.rect.height / 2f);

        lineRect.sizeDelta = new Vector2(length, thickness);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRect.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    void CreateLabel(string text, Vector2 position, Vector2 size)
    {
        GameObject labelObject = new GameObject("GridLabel", typeof(TextMeshProUGUI));

        labelObject.transform.SetParent(transform, false);

        TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();

        label.text = text;
        label.fontSize = 22;
        label.color = labelColor;
        label.alignment = TextAlignmentOptions.Center;
        label.raycastTarget = false;

        RectTransform labelRect = labelObject.GetComponent<RectTransform>();

        labelRect.anchorMin = new Vector2(0.5f, 0.5f);
        labelRect.anchorMax = new Vector2(0.5f, 0.5f);
        labelRect.pivot = new Vector2(0.5f, 0.5f);
        
        //labelRect.anchoredPosition = position;

        labelRect.anchoredPosition = position - new Vector2(rectTransform.rect.width / 2f, rectTransform.rect.height / 2f);

        labelRect.sizeDelta = size;
    }
}
