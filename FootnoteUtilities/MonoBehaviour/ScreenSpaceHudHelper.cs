using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceHudHelper : MonoBehaviour
{
    [SerializeField]
    private int verticalEdge = 100;
    [SerializeField]
    private int horizontalEdge = 100;

    public ScreenPointInfo GetScreenPoint(Vector3 worldPoint, Camera camera)
    {
        ScreenPointInfo output = new ScreenPointInfo();
        output.screenPoint = new Vector2();

        Vector3 worldToScreenPoint = camera.WorldToScreenPoint(worldPoint);

        if (worldToScreenPoint.z < 0)
        {
            worldToScreenPoint.y = Screen.height - worldToScreenPoint.y;
            if (worldToScreenPoint.x >= Screen.width / 2)
                worldToScreenPoint.x = -1;
            else
                worldToScreenPoint.x = Screen.width + 1;
        }

        if (worldToScreenPoint.x < horizontalEdge)
        {
            output.screenPoint.x = horizontalEdge;
            output.leftEdge = true;
            output.rightEdge = false;
        }
        else if (worldToScreenPoint.x > Screen.width - horizontalEdge)
        {
            output.screenPoint.x = Screen.width - horizontalEdge;
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
        else if (worldToScreenPoint.y > Screen.height - verticalEdge)
        {
            output.screenPoint.y = Screen.height - verticalEdge;
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
