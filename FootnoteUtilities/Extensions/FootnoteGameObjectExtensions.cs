/// <summary>
///     Extension methods for UnityEngine.GameObject.
///     Ref: https://github.com/mminer/unity-extensions/blob/master/GameObjectExtensions.cs
/// </summary>
public static class FootnoteGameObjectExtensions
{
    public static bool HasComponent<T>(this UnityEngine.GameObject gameObject)
        where T : UnityEngine.Component => gameObject.GetComponent<T>() != null;

    public static T GetOrAddComponent<T>(this UnityEngine.GameObject gameObject)
        where T : UnityEngine.Component
    {
        T other = gameObject.GetComponent<T>();

        if (other != null)
            return other;

        return gameObject.AddComponent<T>();
    }
}
