using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollContentBackgroundSizer : UIBehaviour
{
    public RectTransform imageRect;
    public RectTransform containerRect;
    private RectTransform cachedThis;
    protected override void OnRectTransformDimensionsChange()
    {
        if (cachedThis == null)
            cachedThis = GetComponent<RectTransform>();

        base.OnRectTransformDimensionsChange();

        imageRect.sizeDelta = cachedThis.sizeDelta;

        if (imageRect.rect.height > containerRect.rect.height)
        {
            imageRect.sizeDelta = new Vector2(imageRect.sizeDelta.x, containerRect.rect.height);
        }

        var innerCorners = new Vector3[4];
        cachedThis.GetWorldCorners(innerCorners);
        imageRect.position = innerCorners[1];
    }
}
