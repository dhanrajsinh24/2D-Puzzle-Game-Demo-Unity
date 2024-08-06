using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HorizontalScrollSnap : MonoBehaviour, IEndDragHandler
{
    public ScrollRect scrollRect;
    public float snapSpeed = 10f;
    public float snapThreshold = 0.01f;

    private RectTransform contentRectTransform;
    private RectTransform viewportRectTransform;
    private float[] itemCenters;

    private void Start()
    {
        contentRectTransform = scrollRect.content;
        viewportRectTransform = scrollRect.viewport;

        // Initialize item centers
        CalculateItemCenters();
    }

    private void CalculateItemCenters()
    {
        int itemCount = contentRectTransform.childCount;
        itemCenters = new float[itemCount];
        for (int i = 0; i < itemCount; i++)
        {
            RectTransform itemRect = contentRectTransform.GetChild(i).GetComponent<RectTransform>();
            // Center of the item in content coordinates
            itemCenters[i] = itemRect.anchoredPosition.x + (itemRect.rect.width / 2);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToClosestElement();
    }

    private void SnapToClosestElement()
    {
        float viewportCenter = viewportRectTransform.rect.width / 2;
        float contentOffset = contentRectTransform.anchoredPosition.x;

        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        // Calculate the center of the viewport in content coordinates
        float viewportCenterInContent = contentOffset + viewportCenter;

        // Find the item closest to the center of the viewport
        for (int i = 0; i < itemCenters.Length; i++)
        {
            float itemCenter = itemCenters[i];
            float distance = Mathf.Abs(viewportCenterInContent - itemCenter);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        // Calculate the normalized scroll position for the target item
        RectTransform targetItemRect = contentRectTransform.GetChild(closestIndex).GetComponent<RectTransform>();
        float targetItemCenter = itemCenters[closestIndex];
        float contentWidth = contentRectTransform.rect.width;
        float viewportWidth = viewportRectTransform.rect.width;

        // Compute normalized position
        float targetNormalizedPosition = (targetItemCenter - viewportWidth / 2) / (contentWidth - viewportWidth);
        targetNormalizedPosition = Mathf.Clamp01(targetNormalizedPosition);

        Debug.Log($"Target Item Index: {closestIndex}");
        Debug.Log($"Target Item Center: {targetItemCenter}");
        Debug.Log($"Target Normalized Position: {targetNormalizedPosition}");

        StartCoroutine(SmoothSnap(targetNormalizedPosition));
    }

    private IEnumerator SmoothSnap(float targetNormalizedPosition)
    {
        float startNormalizedPosition = scrollRect.horizontalNormalizedPosition;

        while (Mathf.Abs(scrollRect.horizontalNormalizedPosition - targetNormalizedPosition) > snapThreshold)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetNormalizedPosition, snapSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the final position is set exactly
        scrollRect.horizontalNormalizedPosition = targetNormalizedPosition;
    }
}
