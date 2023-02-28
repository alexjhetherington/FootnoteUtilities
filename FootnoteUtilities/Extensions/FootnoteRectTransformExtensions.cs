using System;
using UnityEngine;
using UnityEngine.UI;
using static AnchorUtil;

public static class FootnoteRectTransformExtensions
{
    public static RectTransform ModifyInline<T>(this RectTransform rectTransform, Action<T> action)
    {
        action.Invoke(rectTransform.GetComponent<T>());
        return rectTransform;
    }

    public static RectTransform ModifyInline<T>(
        this RectTransform rectTransform,
        String gameObjectName,
        Action<T> action
    ) where T : Component
    {
        var array = rectTransform.GetComponentsInChildren<T>();
        foreach (T component in array)
        {
            if (component.gameObject.name.Equals(gameObjectName))
            {
                action.Invoke(component);
            }
        }
        return rectTransform;
    }

    public static RectTransform AddOnMouseOver(
        this RectTransform rectTransform,
        Action onEnter,
        Action onExit
    )
    {
        var mouseOver = rectTransform.gameObject.AddComponent<OnMouseOver>();
        mouseOver.OnEnter = onEnter;
        mouseOver.OnExit = onExit;
        return rectTransform;
    }

    public static RectTransform SetAnchorPos(this RectTransform rt, AnchorPosParams anchorParams)
    {
        rt.anchorMin = anchorParams.anchorMin;
        rt.anchorMax = anchorParams.anchorMax;
        rt.offsetMin = anchorParams.offsetMin;
        rt.offsetMax = anchorParams.offsetMax;
        rt.pivot = anchorParams.pivot;
        rt.anchoredPosition = anchorParams.position;

        return rt;
    }

    public static RectTransform AddChildren(this RectTransform rt, params RectTransform[] children)
    {
        foreach (RectTransform child in children)
        {
            if (child != null) // Support adding null rect transforms to allow inline skipping elements
                child.SetParent(rt, false);
        }
        return rt;
    }

    public static void AddPreferredSizing(this RectTransform rt)
    {
        var csf = rt.AddComponent<ContentSizeFitter>();
        csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    public static void CopyPositionFrom(this RectTransform rt, RectTransform copyFrom)
    {
        rt.anchorMin = copyFrom.anchorMin;
        rt.anchorMax = copyFrom.anchorMax;
        rt.anchoredPosition = copyFrom.anchoredPosition;
        rt.sizeDelta = copyFrom.sizeDelta;
        rt.pivot = copyFrom.pivot;
    }

    public static Rect GetWorldRect(this RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        // Get the bottom left corner.
        Vector3 position = corners[0];

        Vector2 size = new Vector2(
            rectTransform.lossyScale.x * rectTransform.rect.size.x,
            rectTransform.lossyScale.y * rectTransform.rect.size.y
        );

        return new Rect(position, size);
    }
}
