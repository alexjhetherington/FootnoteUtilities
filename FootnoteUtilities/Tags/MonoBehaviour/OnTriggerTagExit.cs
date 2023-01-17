using UnityEngine;
using UnityEngine.Events;

public class OnTriggerTagExit : MonoBehaviour
{
    public Tag checkTag;
    public bool compound = false;
    public UnityEvent unityEvent;

    void OnTriggerExit(Collider other)
    {
        if (compound)
            return;

        if (other.HasTag(checkTag))
        {
            unityEvent.Invoke();
        }
    }

    void OnCompoundTriggerExit(Collider other)
    {
        if (!compound)
            return;

        if (other.HasTag(checkTag))
        {
            unityEvent.Invoke();
        }
    }
}
