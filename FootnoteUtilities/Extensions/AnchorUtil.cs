using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnchorUtil
{
    public struct AnchorPosParams
    {
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 offsetMin;
        public Vector2 offsetMax;
        public Vector2 pivot;
        public Vector2 position;
    }

    public static AnchorPosParams FullScreenStretch()
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(0, 0),
            anchorMax = new Vector2(1, 1),
            offsetMin = new Vector2(0, 0),
            offsetMax = new Vector2(0, 0),
            pivot = new Vector2(0.5f, 0.5f)
        };
    }

    public static AnchorPosParams TopHorizontalStretch()
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(0f, 1),
            anchorMax = new Vector2(1, 1),
            offsetMin = new Vector2(0, 0),
            offsetMax = new Vector2(0, 0),
            pivot = new Vector2(0.5f, 1f)
        };
    }

    public static AnchorPosParams RightVerticalStretch()
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(1f, 0),
            anchorMax = new Vector2(1, 1),
            offsetMin = new Vector2(0, 0),
            offsetMax = new Vector2(0, 0),
            pivot = new Vector2(1f, 0.5f)
        };
    }

    public static AnchorPosParams TopLeft(float x, float y)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(0, 1),
            anchorMax = new Vector2(0, 1),
            position = new Vector2(x, -y),
            pivot = new Vector2(0, 1f)
        };
    }

    public static AnchorPosParams TopCentre(float y)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(0.5f, 1),
            anchorMax = new Vector2(0.5f, 1),
            position = new Vector2(0, -y),
            pivot = new Vector2(0.5f, 1f)
        };
    }

    public static AnchorPosParams TopRight(float x, float y)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(1, 1),
            anchorMax = new Vector2(1, 1),
            position = new Vector2(-x, -y),
            pivot = new Vector2(1f, 1f)
        };
    }

    public static AnchorPosParams CentreLeft(float x)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(0, 0.5f),
            anchorMax = new Vector2(0, 0.5f),
            position = new Vector2(x, 0),
            pivot = new Vector2(0f, 0.5f)
        };
    }

    public static AnchorPosParams Centre(float x, float y)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(0.5f, 0.5f),
            anchorMax = new Vector2(0.5f, 0.5f),
            position = new Vector2(x, y),
            pivot = new Vector2(0.5f, 0.5f)
        };
    }

    public static AnchorPosParams CentreRight(float x)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(1, 0.5f),
            anchorMax = new Vector2(1, 0.5f),
            position = new Vector2(-x, 0),
            pivot = new Vector2(1f, 0.5f)
        };
    }

    public static AnchorPosParams BottomLeft(float x, float y)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(0, 0),
            anchorMax = new Vector2(0, 0),
            position = new Vector2(x, y),
            pivot = new Vector2(0f, 0f)
        };
    }

    public static AnchorPosParams BottomCentre(float y)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(0.5f, 0),
            anchorMax = new Vector2(0.5f, 0),
            position = new Vector2(0, y),
            pivot = new Vector2(0.5f, 0f)
        };
    }

    public static AnchorPosParams BottomRight(float x, float y)
    {
        return new AnchorPosParams
        {
            anchorMin = new Vector2(1, 0),
            anchorMax = new Vector2(1, 0),
            position = new Vector2(-x, y),
            pivot = new Vector2(1f, 0f)
        };
    }
}
