using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceHudHelper : MonoBehaviour
{
    [SerializeField]
    private int verticalEdge = 100;
    [SerializeField]
    private int horizontalEdge = 100;

    private Canvas canvas;

    public void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public ScreenPointInfo GetScreenPoint(Vector3 worldPoint, Camera camera)
    {
        ScreenPointInfo output = new ScreenPointInfo();
        output.screenPoint = new Vector2();

        Vector3 worldToScreenPoint = camera.WorldToScreenPoint(worldPoint);

        Vector2 canvasDimensions = canvas.GetComponent<RectTransform>().sizeDelta;

        worldToScreenPoint.x /= Screen.width / canvasDimensions.x;
        worldToScreenPoint.y /= Screen.height / canvasDimensions.y;

        if (worldToScreenPoint.z < 0)
        {
            worldToScreenPoint.y = canvasDimensions.y - worldToScreenPoint.y;
            if (worldToScreenPoint.x >= canvasDimensions.x / 2)
                worldToScreenPoint.x = -1;
            else
                worldToScreenPoint.x = canvasDimensions.x + 1;
        }

        if (worldToScreenPoint.x < horizontalEdge)
        {
            output.screenPoint.x = horizontalEdge;
            output.leftEdge = true;
            output.rightEdge = false;
        }
        else if (worldToScreenPoint.x > canvasDimensions.x - horizontalEdge)
        {
            output.screenPoint.x = canvasDimensions.x - horizontalEdge;
            output.leftEdge = false;
            output.rightEdge = true;
        }
        else
        {
            output.screenPoint.x = worldToScreenPoint.x;
            output.leftEdge = false;
            output.rightEdge = false;
        }

        if (worldToScreenPoint.y < verticalEdge)
        {
            output.screenPoint.y = verticalEdge;
            output.topEdge = false;
            output.bottomEdge = true;
        }
        else if (worldToScreenPoint.y > canvasDimensions.y - verticalEdge)
        {
            output.screenPoint.y = canvasDimensions.y - verticalEdge;
            output.topEdge = true;
            output.bottomEdge = false;
        }
        else
        {
            output.screenPoint.y = worldToScreenPoint.y;
            output.topEdge = false;
            output.bottomEdge = false;
        }

        return output;
    }

    public struct ScreenPointInfo
    {
        public Vector2 screenPoint;
        public bool leftEdge;
        public bool rightEdge;
        public bool topEdge;
        public bool bottomEdge;
    }
}
