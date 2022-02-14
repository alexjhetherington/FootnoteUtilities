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
}
