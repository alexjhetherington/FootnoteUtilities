using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FootnoteLineRendererExtensions
{
    //public const int MAX_TRAIL_POSITIONS = 200;

    public static Vector3[] GetPositions(this LineRenderer lineRenderer)
    {
        int trailPointsCount = lineRenderer.GetPositions(new Vector3[lineRenderer.positionCount]);
        Vector3[] positions = new Vector3[trailPointsCount];
        lineRenderer.GetPositions(positions);

        if (!lineRenderer.useWorldSpace)
        {
            for (int i = 0; i < trailPointsCount; i++)
            {
                positions[i] = positions[i] + lineRenderer.transform.position;
            }
        }

        return positions;
    }
}
