using UnityEngine;

public static class FootnoteVector3Extensions
{
    public const float EPSILON = 0.0001f;
    public const float EPSILON_SQR = 0.0000001f;
    public const double DBL_EPSILON = 9.99999943962493E-11;

    public static bool FuzzyEquals(this Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < EPSILON_SQR;
    }

    public static bool FuzzyEquals(this Vector3 a, Vector3 b, float epsilon)
    {
        return Vector3.SqrMagnitude(a - b) < epsilon;
    }

    public static Vector3 Horizontal(this Vector3 input)
    {
        return new Vector3(input.x, 0, input.z);
    }

    public static Vector3 Vertical(this Vector3 input)
    {
        return new Vector3(0, input.y, 0);
    }
}
