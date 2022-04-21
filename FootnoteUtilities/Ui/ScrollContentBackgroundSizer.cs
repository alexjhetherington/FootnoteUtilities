using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollContentBackgroundSizer : UIBehaviour
{
    public RectTransform imageRect;
    public RectTransform containerRect;
    private RectTransform cachedThis;
    private RectTransform cachedContainerParent;

    private Vector3[] localCorners = new Vector3[4];
    private Vector3[] worldCorners = new Vector3[4];
    protected override void OnRectTransformDimensionsChange()
    {
        if (cachedThis == null)
            cachedThis = GetComponent<RectTransform>();
        if (cachedContainerParent == null)
            cachedContainerParent = cachedThis.parent.GetComponent<RectTransform>();

        base.OnRectTransformDimensionsChange();

        cachedThis.GetWorldCorners(worldCorners);
        cachedThis.GetLocalCorners(localCorners);

        float width =
            (containerRect.InverseTransformPoint(worldCorners[2])).x
            - (containerRect.InverseTransformPoint(worldCorners[1])).x;
        float height =
            (containerRect.InverseTransformPoint(worldCorners[1])).y
            - (containerRect.InverseTransformPoint(worldCorners[0])).y;

        if (height > containerRect.rect.height)
        {
            height = containerRect.rect.height;
        }

        imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        imageRect.localPosition = new Vector3(
            containerRect.InverseTransformPoint(worldCorners[1]).x,
            cachedContainerParent.rect.height / 2,
            0
        );
    }
}
